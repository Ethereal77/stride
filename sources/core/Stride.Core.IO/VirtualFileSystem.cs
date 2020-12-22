// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Represents a virtual abstraction over a file system, handling access to files, HTTP, packages, path rewrite, etc.
    /// </summary>
    public static partial class VirtualFileSystem
    {
        public static readonly char DirectorySeparatorChar = '/';
        public static readonly char AltDirectorySeparatorChar = '\\';
        public static readonly char[] AllDirectorySeparatorChars = { DirectorySeparatorChar, AltDirectorySeparatorChar };

        public static readonly string LocalDatabasePath = "/local/db";
        public static readonly string ApplicationDatabasePath = "/data/db";
        public static readonly string ApplicationDatabaseIndexName = "index";
        public static readonly string ApplicationDatabaseIndexPath = ApplicationDatabasePath + DirectorySeparatorChar + ApplicationDatabaseIndexName;

        private static readonly Regex PathSplitRegex = new Regex(@"(\\|/)");

        // As opposed to real Path.GetTempFileName, we don't have a 65536 character limit.
        // This can be achieved by having a fixed random seed.
        // However, if activated, it would probably test too many files in the same order, if some already exists.
        private static readonly Random tempFileRandom = new Random(Environment.TickCount);

        private static readonly Dictionary<string, IVirtualFileProvider> providers = new Dictionary<string, IVirtualFileProvider>();


        /// <summary>
        ///   The application data file provider.
        /// </summary>
        public static readonly IVirtualFileProvider ApplicationData;

        /// <summary>
        ///   The application database file provider (ObjectId level).
        /// </summary>
        public static IVirtualFileProvider ApplicationObjectDatabase;

        /// <summary>
        ///   The application database file provider (Index level).
        /// </summary>
        public static readonly IVirtualFileProvider ApplicationDatabase;

        /// <summary>
        ///   The application cache folder.
        /// </summary>
        public static readonly IVirtualFileProvider ApplicationCache;

        /// <summary>
        ///   The application user roaming folder. Included in backup.
        /// </summary>
        public static readonly IVirtualFileProvider ApplicationRoaming;

        /// <summary>
        ///   The application user local folder. Included in backup.
        /// </summary>
        public static readonly IVirtualFileProvider ApplicationLocal;

        /// <summary>
        ///   The application temporary data provider.
        /// </summary>
        public static readonly IVirtualFileProvider ApplicationTemporary;

        /// <summary>
        ///   The application binary folder.
        /// </summary>
        public static readonly IVirtualFileProvider ApplicationBinary;

        /// <summary>
        ///   The whole host file system. This should be used only in tools.
        /// </summary>
        public static readonly DriveFileProvider Drive;

        /// <summary>
        ///   Initializes static members of the <see cref="VirtualFileSystem"/> class.
        /// </summary>
        static VirtualFileSystem()
        {
            PlatformFolders.IsVirtualFileSystemInitialized = true;

            // TODO: Find a better solution to customize the ApplicationDataDirectory, now we're very limited due to the initialization from a static constructor
            ApplicationData = new FileSystemProvider("/data", Path.Combine(PlatformFolders.ApplicationDataDirectory, PlatformFolders.ApplicationDataSubDirectory));
            ApplicationCache = new FileSystemProvider("/cache", PlatformFolders.ApplicationCacheDirectory);

            ApplicationRoaming = new FileSystemProvider("/roaming", PlatformFolders.ApplicationRoamingDirectory);
            ApplicationLocal = new FileSystemProvider("/local", PlatformFolders.ApplicationLocalDirectory);
            ApplicationTemporary = new FileSystemProvider("/tmp", PlatformFolders.ApplicationTemporaryDirectory);
            ApplicationBinary = new FileSystemProvider("/binary", PlatformFolders.ApplicationBinaryDirectory);
            Drive = new DriveFileProvider(DriveFileProvider.DefaultRootPath);
        }

        /// <summary>
        ///   Gets the registered providers.
        /// </summary>
        public static IEnumerable<IVirtualFileProvider> Providers => providers.Values;

        /// <summary>
        ///   Registers a virtual file provider at the specified mount location.
        /// </summary>
        /// <param name="provider">The provider to register.</param>
        public static void RegisterProvider(IVirtualFileProvider provider)
        {
            if (provider.RootPath != null)
            {
               if (providers.ContainsKey(provider.RootPath))
                   throw new InvalidOperationException($@"A Virtual File Provider with the root path ""{provider.RootPath}"" already exists.");

               providers.Add(provider.RootPath, provider);
            }
        }

        /// <summary>
        ///   Unregisters a virtual file provider.
        /// </summary>
        /// <param name="provider">The provider to remove.</param>
        /// <param name="dispose">A value indicating if the provider should be disposed. Default is <c>true</c>.</param>
        public static void UnregisterProvider(IVirtualFileProvider provider, bool dispose = true)
        {
            var mountPoints = providers.Where(x => x.Value == provider).ToArray();

            foreach (var mountPoint in mountPoints)
            {
                providers.Remove(mountPoint.Key);
                if (dispose)
                    mountPoint.Value.Dispose();
            }
        }

        /// <summary>
        ///   Mounts the specified path in the specified virtual file mount point.
        /// </summary>
        /// <param name="mountPoint">The mount point in the virtual file system.</param>
        /// <param name="path">The directory path.</param>
        /// <returns>
        ///   An instance of <see cref="IVirtualFileProvider"/> representing the mounted file system.
        /// </returns>
        public static IVirtualFileProvider MountFileSystem(string mountPoint, string path)
        {
            return new FileSystemProvider(mountPoint, path);
        }

        /// <summary>
        ///   Mounts or remounts the specified path in the specified virtual file mount point.
        /// </summary>
        /// <param name="mountPoint">The mount point in the virtual file system.</param>
        /// <param name="path">The directory path.</param>
        /// <returns>
        ///   An instance of <see cref="IVirtualFileProvider"/> representing the mounted file system.
        /// </returns>
        public static IVirtualFileProvider RemountFileSystem(string mountPoint, string path)
        {
            // Ensure mount point is terminated with a /
            if (!mountPoint.EndsWith(DirectorySeparatorChar))
                mountPoint += DirectorySeparatorChar;

            // Find existing provider
            var provider = providers.FirstOrDefault(x => x.Key == mountPoint);
            if (provider.Value != null)
            {
                ((FileSystemProvider) provider.Value).ChangeBasePath(path);
                return provider.Value;
            }

            // Otherwise create new one
            return new FileSystemProvider(mountPoint, path);
        }

        /// <summary>
        ///   Determines whether the specified path points to an existing file.
        /// </summary>
        /// <param name="path">The path of the file to check.</param>
        /// <returns><c>true</c> if the file exists; <c>false</c> otherwise.</returns>
        public static bool FileExists(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            var result = ResolveProviderUnsafe(path, resolveTop: true);
            if (result.Provider is null)
                return false;

            return result.Provider.FileExists(result.Path);
        }

        /// <summary>
        ///   Determines whether the specified path points to an existing directory.
        /// </summary>
        /// <param name="path">The path of the directory to check.</param>
        /// <returns><c>true</c> if the directory exists; <c>false</c> otherwise.</returns>
        public static bool DirectoryExists(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            var result = ResolveProviderUnsafe(path, resolveTop: true);
            if (result.Provider is null)
                return false;

            return result.Provider.DirectoryExists(result.Path);
        }

        /// <summary>
        ///   Deletes a file.
        /// </summary>
        /// <param name="path">The path of the file to delete.</param>
        public static void FileDelete(string path)
        {
            var result = ResolveProvider(path, resolveTop: true);

            result.Provider.FileDelete(result.Path);
        }

        /// <summary>
        ///   Moves a file from a source path to a destination path.
        /// </summary>
        /// <param name="sourcePath">The source path of the file to move.</param>
        /// <param name="destinationPath">The destination path where the file will be located.</param>
        public static void FileMove(string sourcePath, string destinationPath)
        {
            ResolveProviderResult sourceResult = ResolveProvider(sourcePath, resolveTop: true);
            ResolveProviderResult destinationResult = ResolveProvider(destinationPath, resolveTop: true);

            if (sourceResult.Provider == destinationResult.Provider)
                sourceResult.Provider.FileMove(sourceResult.Path, destinationResult.Path);
            else
                sourceResult.Provider.FileMove(sourceResult.Path, destinationResult.Provider, destinationResult.Path);
        }

        /// <summary>
        ///   Returns the size of the specified file, in bytes
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>The file size, in bytes.</returns>
        public static long FileSize(string path)
        {
            var result = ResolveProvider(path, resolveTop: true);

            return result.Provider.FileSize(result.Path);
        }

        /// <summary>
        ///   Returns the date and time the specified file or directory was last written to.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>
        ///   A <see cref="DateTime"/> set to the date and time that the specified file or directory was last written to.
        /// </returns>
        public static DateTime GetLastWriteTime(string path)
        {
            var result = ResolveProvider(path, resolveTop:  true);

            return result.Provider.GetLastWriteTime(result.Path);
        }

        /// <summary>
        ///   Determines whether the specified path points to an existing file.
        /// </summary>
        /// <param name="path">The path of the file to check.</param>
        /// <returns>
        ///   A <see cref="Task{bool}"/> that, when completed, will return <c>true</c> if the file exists;
        ///   <c>false</c> otherwise.
        /// </returns>
        public static Task<bool> FileExistsAsync(string path)
        {
            return Task<bool>.Factory.StartNew(() => FileExists(path));
        }

        /// <summary>
        ///   Creates the specified directory. Also creates all directories needed for the specified
        ///   path to exist.
        /// </summary>
        /// <param name="path">The path of the directory to create.</param>
        public static void CreateDirectory(string path)
        {
            var resolveProviderResult = ResolveProvider(path, resolveTop: true);

            resolveProviderResult.Provider.CreateDirectory(resolveProviderResult.Path);
        }

        /// <summary>
        ///   Opens a stream from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode in which the operating system will open the file..</param>
        /// <param name="access">The file access mode..</param>
        /// <param name="share">Specifies how other processes share the access to this same file. Default is <see cref="VirtualFileShare.Read"/>.</param>
        /// <returns>The opened <see cref="Stream"/>.</returns>
        public static Stream OpenStream(string path, VirtualFileMode mode, VirtualFileAccess access, VirtualFileShare share = VirtualFileShare.Read)
        {
            var resolveProviderResult = ResolveProvider(path, resolveTop: false);

            return resolveProviderResult.Provider.OpenStream(resolveProviderResult.Path, mode, access, share);
        }

        /// <summary>
        ///   Opens a stream from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode in which the operating system will open the file..</param>
        /// <param name="access">The file access mode..</param>
        /// <param name="share">Specifies how other processes share the access to this same file. Default is <see cref="VirtualFileShare.Read"/>.</param>
        /// <param name="provider">When the method returns, contains the provider used to load the stream.</param>
        /// <returns>The opened <see cref="Stream"/>.</returns>
        public static Stream OpenStream(string path, VirtualFileMode mode, VirtualFileAccess access, VirtualFileShare share, out IVirtualFileProvider provider)
        {
            var resolveProviderResult = ResolveProvider(path, resolveTop: false);
            provider = resolveProviderResult.Provider;

            return provider.OpenStream(resolveProviderResult.Path, mode, access, share);
        }

        /// <summary>
        ///   Opens a stream from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode in which the operating system will open the file..</param>
        /// <param name="access">The file access mode..</param>
        /// <param name="share">Specifies how other processes share the access to this same file. Default is <see cref="VirtualFileShare.Read"/>.</param>
        /// <returns>
        ///   A <see cref="Task{Stream}"/> that, when completed, will return the opened <see cref="Stream"/>.
        /// </returns>
        public static Task<Stream> OpenStreamAsync(string path, VirtualFileMode mode, VirtualFileAccess access, VirtualFileShare share = VirtualFileShare.Read)
        {
            return Task<Stream>.Factory.StartNew(() => OpenStream(path, mode, access, share));
        }

        /// <summary>
        ///   Gets the absolute path for the specified virtual path.
        /// </summary>
        /// <param name="path">The path local to the virtual file system.</param>
        /// <returns>An absolute path. The returned path is system dependent (i.e <c>C:\Path\To\Your\File.x</c>).</returns>
        public static string GetAbsolutePath(string path)
        {
            var resolveProviderResult = ResolveProvider(path, resolveTop: true);

            return resolveProviderResult.Provider.GetAbsolutePath(resolveProviderResult.Path);
        }

        /// <summary>
        ///   Resolves a relative path to a full virtual path.
        /// </summary>
        /// <param name="path">The path to resolve.</param>
        /// <returns>The resolved path.</returns>
        public static string ResolvePath(string path)
        {
            var resolveProviderResult = ResolveProvider(path, resolveTop: false);

            var sb = new StringBuilder();
            if (resolveProviderResult.Provider.RootPath != ".")
            {
                sb.Append(resolveProviderResult.Provider.RootPath);
                sb.Append("/");
            }
            sb.Append(resolveProviderResult.Path);

            return sb.ToString();
        }

        /// <summary>
        ///   Returns a list of files from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="searchOption">The search option.</param>
        /// <returns>A list of files from the specified path.</returns>
        public static Task<string[]> ListFiles(string path, string searchPattern, VirtualSearchOption searchOption)
        {
            var resolveProviderResult = ResolveProvider(path, resolveTop: true);

            return Task.Factory.StartNew(() => resolveProviderResult.Provider
                .ListFiles(resolveProviderResult.Path, searchPattern, searchOption)
                .Select(x => resolveProviderResult.Provider.RootPath + x)
                .ToArray());
        }

        /// <summary>
        ///   Creates a temporary zero-byte file and returns its full path.
        /// </summary>
        /// <returns>The full path of the created temporary file.</returns>
        public static string GetTempFileName()
        {
            int tentatives = 0;
            Stream stream = null;
            string filename;
            do
            {
                filename = "sd" + (tempFileRandom.Next() + 1).ToString("x") + ".tmp";
                try
                {
                    stream = ApplicationTemporary.OpenStream(filename, VirtualFileMode.CreateNew, VirtualFileAccess.ReadWrite);
                }
                catch (IOException)
                {
                    // No more than 65536 files
                    if (tentatives++ > 0x10000)
                        throw;
                }
            }
            while (stream is null);

            stream.Dispose();

            return ApplicationTemporary.RootPath + "/" + filename;
        }

        public static string BuildPath(string path, string relativePath)
        {
            return path.Substring(0, LastIndexOfDirectorySeparator(path) + 1) + relativePath;
        }

        /// <summary>
        ///   Returns the path with its .. or . parts simplified.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The resolved absolute path.</returns>
        public static string ResolveAbsolutePath(string path)
        {
            if (!path.Contains(DirectorySeparatorChar + ".."))
                return path;

            var pathElements = PathSplitRegex.Split(path).ToList();

            // Remove duplicate directory separators
            for (int i = 0; i < pathElements.Count; ++i)
            {
                if (pathElements[i].Length > 1 && (pathElements[i][0] == '/' || pathElements[i][0] == '\\'))
                    pathElements[i] = pathElements[i][0].ToString();
            }

            for (int i = 0; i < pathElements.Count; ++i)
            {
                if (pathElements[i] == "..")
                {
                    // Remove .. and the item preceding that, if any
                    if (i >= 3 && (pathElements[i - 1] == "/" || pathElements[i - 1] == "\\"))
                    {
                        pathElements.RemoveRange(i - 3, 4);
                        i -= 4;
                    }
                }
                else if (pathElements[i] == ".")
                {
                    if (i >= 1 && (pathElements[i - 1] == "/" || pathElements[i - 1] == "\\"))
                    {
                        pathElements.RemoveRange(i - 1, 2);
                        i -= 2;
                    }
                    else if (i + 1 < pathElements.Count && (pathElements[i + 1] == "/" || pathElements[i + 1] == "\\"))
                    {
                        pathElements.RemoveRange(i, 2);
                        --i;
                    }
                }
            }

            return string.Join(string.Empty, pathElements);
        }

        /// <summary>
        ///   Combines two strings into a virtual path.
        /// </summary>
        /// <param name="path1">The first path to combine.</param>
        /// <param name="path2">The second path to combine.</param>
        /// <returns>
        ///   The combined paths. If one of the specified paths is a zero-length string, this
        ///   method returns the other path. If <paramref name="path2"/> contains an absolute path,
        ///   this method returns <paramref name="path2"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///   <paramref name="path1"/> or <paramref name="path2"/> contains one or more of the invalid characters
        ///   defined in <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="path1"/> or <paramref name="path2"/> is a <c>null</c> reference.
        /// </exception>
        public static string Combine(string path1, string path2)
        {
            if (path1.Length == 0)
                return path2;
            if (path2.Length == 0)
                return path1;

            var lastPath1 = path1[path1.Length - 1];
            if (lastPath1 != DirectorySeparatorChar && lastPath1 != AltDirectorySeparatorChar)
                return path1 + DirectorySeparatorChar + path2;

            return path1 + path2;
        }

        /// <summary>
        ///   Gets the parent folder of a path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The parent folder.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="path"/> doesn't contain any directory separator <c>/</c>.
        /// </exception>
        public static string GetParentFolder(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            var lastSlashIndex = LastIndexOfDirectorySeparator(path);
            if (lastSlashIndex == -1)
                throw new ArgumentException($"Path [{path}] doesn't contain a directory separator.", nameof(path));

            return path.Substring(0, lastSlashIndex);
        }

        /// <summary>
        ///   Gets a file's name with its extension.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>The name of the file with its extension.</returns>
        public static string GetFileName(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            var lastSlashIndex = LastIndexOfDirectorySeparator(path);

            return path.Substring(lastSlashIndex + 1);
        }

        /// <summary>
        ///   Creates a relative path.
        /// </summary>
        /// <param name="target">The target path to make relativo to <paramref name="sourcePath"/>.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <returns>The relative path of <paramref name="target"/>.</returns>
        public static string CreateRelativePath(string target, string sourcePath)
        {
            var targetDirectories = target.Split(AllDirectorySeparatorChars, StringSplitOptions.RemoveEmptyEntries);
            var sourceDirectories = sourcePath.Split(AllDirectorySeparatorChars, StringSplitOptions.RemoveEmptyEntries);

            // Find common root
            int length = Math.Min(targetDirectories.Length, sourceDirectories.Length);
            int commonRoot;
            for (commonRoot = 0; commonRoot < length; ++commonRoot)
            {
                if (targetDirectories[commonRoot] != sourceDirectories[commonRoot])
                    break;
            }

            var result = new StringBuilder();

            // Append .. for each path only in source
            for (int i = commonRoot; i < sourceDirectories.Length; ++i)
            {
                result.Append(".." + DirectorySeparatorChar);
            }

            // Append path in destination
            for (int i = commonRoot; i < targetDirectories.Length; ++i)
            {
                result.Append(targetDirectories[i]);
                if (i < targetDirectories.Length - 1)
                    result.Append(DirectorySeparatorChar);
            }

            return result.ToString();
        }

        /// <summary>
        ///   Resolves the virtual file provider for a given path.
        /// </summary>
        /// <param name="path">The path to resolve.</param>
        /// <returns>
        ///   A <see cref="ResolveProviderResult"/> containing the virtual file system provider and local path in it.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is a <c>null</c> reference.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="path"/> could not be resolved to a provider.</exception>
        public static ResolveProviderResult ResolveProvider(string path, bool resolveTop)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            var result = ResolveProviderUnsafe(path, resolveTop);
            if (result.Provider is null)
                throw new InvalidOperationException($"Path [{path}] could not be resolved to a provider.");

            return result;
        }

        private static int LastIndexOfDirectorySeparator(string path)
        {
            return path.LastIndexOfAny(AllDirectorySeparatorChars);
        }

        /// <summary>
        ///   Resolves the virtual file provider for a given path. This method does not check the validity
        ///   of the path nor the existence of a provider for that path. In the majority of cases, prefer
        ///   to use <see cref="ResolveProvider"/>.
        /// </summary>
        /// <param name="path">The path to resolve.</param>
        /// <returns>
        ///   A <see cref="ResolveProviderResult"/> containing the virtual file system provider and local path in it.
        /// </returns>
        public static ResolveProviderResult ResolveProviderUnsafe(string path, bool resolveTop)
        {
            // Slow path for path using \ instead of /
            if (path.Contains(AltDirectorySeparatorChar))
                path = path.Replace(AltDirectorySeparatorChar, DirectorySeparatorChar);

            // Resolve using providers at every level of the path (deep first)
            //   i.e. provider for path /a/b/c/file will be searched in the following order:
            //   /a/b/c/ then /a/b/ then /a/.
            for (int i = path.Length - 1; i >= 0; --i)
            {
                var pathChar = path[i];
                var isResolvingTop = resolveTop && i == path.Length - 1;
                if (!isResolvingTop && pathChar != DirectorySeparatorChar)
                    continue;

                string providerPath = isResolvingTop && pathChar != DirectorySeparatorChar ?
                    new StringBuilder(path.Length + 1).Append(path)
                                                      .Append(DirectorySeparatorChar)
                                                      .ToString() :

                    (i + 1) == path.Length ? path : path.Substring(0, i + 1);

                if (providers.TryGetValue(providerPath, out IVirtualFileProvider provider))
                {
                    // If resolving top, we want the / at the end of "path" if it wasn't there already (should be in providerPath)
                    if (isResolvingTop)
                        path = providerPath;

                    return new ResolveProviderResult { Provider = provider, Path = path.Substring(providerPath.Length) };
                }
            }

            return default;
        }

        /// <summary>
        ///   Result of a path resolve operation, containing the provider responsible for a given path.
        /// </summary>
        public struct ResolveProviderResult
        {
            /// <summary>
            ///   The virtual file provider responsible for the specified virtual path.
            /// </summary>
            public IVirtualFileProvider Provider;

            /// <summary>
            ///   The virtual path relative to the provider's <see cref="IVirtualFileProvider.RootPath"/>.
            /// </summary>
            public string Path;
        }
    }
}
