// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Linq;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Represents a file system implementation.
    /// </summary>
    public partial class FileSystemProvider : VirtualFileProviderBase
    {
        public static readonly char VolumeSeparatorChar = Path.VolumeSeparatorChar;
        public static readonly char DirectorySeparatorChar = Path.DirectorySeparatorChar;
        public static readonly char AltDirectorySeparatorChar = AltDirectorySeparatorChar == '/' ? '\\' : '/';

        /// <summary>
        ///   Base path of this provider (every path will be relative to this one).
        /// </summary>
        private string localBasePath;

        /// <summary>
        ///   Initializes a new instance of the <see cref="FileSystemProvider" /> class with the given base path.
        /// </summary>
        /// <param name="rootPath">The root path of this provider.</param>
        /// <param name="localBasePath">The path to a local directory where this instance will load the files from.</param>
        public FileSystemProvider(string rootPath, string localBasePath) : base(rootPath)
        {
            ChangeBasePath(localBasePath);
        }

        /// <summary>
        ///   Sets the base local directory where this provider will load files from.
        /// </summary>
        /// <param name="basePath">The new base path.</param>
        public void ChangeBasePath(string basePath)
        {
            localBasePath = basePath;

            if (localBasePath != null)
                localBasePath = localBasePath.Replace(AltDirectorySeparatorChar, DirectorySeparatorChar);

            // Ensure localBasePath ends with a \
            if (localBasePath != null && !localBasePath.EndsWith(DirectorySeparatorChar))
                localBasePath += DirectorySeparatorChar;
        }

        /// <summary>
        ///   Gets the full path corresponding to a virtual path.
        /// </summary>
        /// <param name="url">The virtual path to convert.</param>
        /// <returns>The full path in the file system.</returns>
        protected virtual string ConvertUrlToFullPath(string url)
        {
            if (localBasePath is null)
                return url;

            return localBasePath + url.Replace(VirtualFileSystem.DirectorySeparatorChar, DirectorySeparatorChar);
        }

        /// <summary>
        ///   Gets the virtual path corresponding to a file system path.
        /// </summary>
        /// <param name="path">The file system path to convert.</param>
        /// <returns>The virtual path in this provider.</returns>
        protected virtual string ConvertFullPathToUrl(string path)
        {
            if (localBasePath is null)
                return path;

            if (!path.StartsWith(localBasePath, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Trying to convert back a path that is not in this file system provider.");

            return path.Substring(localBasePath.Length).Replace(DirectorySeparatorChar, VirtualFileSystem.DirectorySeparatorChar);
        }

        /// <inheritdoc/>
        public override bool DirectoryExists(string url)
        {
            var path = ConvertUrlToFullPath(url);

            return Directory.Exists(path);
        }

        /// <inheritdoc/>
        public override void CreateDirectory(string url)
        {
            var path = ConvertUrlToFullPath(url);

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unable to create directory [{path}].", ex);
            }
        }

        /// <inheritdoc/>
        public override bool FileExists(string url)
        {
            var path = ConvertUrlToFullPath(url);

            return File.Exists(path);
        }

        /// <inheritdoc/>
        public override long FileSize(string url)
        {
            var path = ConvertUrlToFullPath(url);
            var fileInfo = new FileInfo(path);

            return fileInfo.Length;
        }

        /// <inheritdoc/>
        public override void FileDelete(string url)
        {
            var path = ConvertUrlToFullPath(url);

            File.Delete(path);
        }

        /// <inheritdoc/>
        public override void FileMove(string sourceUrl, string destinationUrl)
        {
            var sourcePath = ConvertUrlToFullPath(sourceUrl);
            var destPath = ConvertUrlToFullPath(destinationUrl);

            File.Move(sourcePath, destPath);
        }

        /// <inheritdoc/>
        public override void FileMove(string sourceUrl, IVirtualFileProvider destinationProvider, string destinationUrl)
        {
            if (destinationProvider is FileSystemProvider filesystemProvider)
            {
                filesystemProvider.CreateDirectory(destinationUrl.Substring(0, destinationUrl.LastIndexOf(VirtualFileSystem.DirectorySeparatorChar)));

                var sourcePath = ConvertUrlToFullPath(sourceUrl);
                var destPath = filesystemProvider.ConvertUrlToFullPath(destinationUrl);

                File.Move(sourcePath, destPath);
            }
            else
            {
                bool copySuccesful = false;

                using (Stream sourceStream = OpenStream(sourceUrl, VirtualFileMode.Open, VirtualFileAccess.Read))
                using (Stream destinationStream = destinationProvider.OpenStream(destinationUrl, VirtualFileMode.CreateNew, VirtualFileAccess.Write))
                {
                    sourceStream.CopyTo(destinationStream);
                    copySuccesful = true;
                }

                if(copySuccesful)
                    FileDelete(sourceUrl);
            }
        }

        /// <inheritdoc/>
        public override string GetAbsolutePath(string path) => ConvertUrlToFullPath(path);

        /// <inheritdoc/>
        public override bool TryGetFileLocation(string path, out string filePath, out long start, out long end)
        {
            filePath = ConvertUrlToFullPath(path);
            start = 0;
            end = -1;

            return true;
        }

        /// <inheritdoc/>
        public override string[] ListFiles(string url, string searchPattern, VirtualSearchOption searchOption)
        {
            var path = ConvertUrlToFullPath(url);

            return Directory.GetFiles(path, searchPattern, (SearchOption) searchOption)
                            .Select(ConvertFullPathToUrl)
                            .ToArray();
        }

        /// <inheritdoc/>
        public override Stream OpenStream(string url, VirtualFileMode mode, VirtualFileAccess access, VirtualFileShare share = VirtualFileShare.Read, StreamFlags streamType = StreamFlags.None)
        {
            if (localBasePath != null && url.Split(VirtualFileSystem.DirectorySeparatorChar, VirtualFileSystem.AltDirectorySeparatorChar).Contains(".."))
                throw new InvalidOperationException("Relative path is not allowed in FileSystemProvider.");

            var filename = ConvertUrlToFullPath(url);
            var result = new FileStream(filename, (FileMode) mode, (FileAccess) access, (FileShare) share);

            return result;
        }

        /// <inheritdoc/>
        public override DateTime GetLastWriteTime(string url)
        {
            var path = ConvertUrlToFullPath(url);

            return File.GetLastWriteTime(path);
        }
    }
}
