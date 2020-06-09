// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

using ServiceWire.NamedPipes;

using Stride.Core;

namespace Stride.ExecServer
{
    /// <summary>
    ///   An container for managing shadow copy <see cref="AppDomain"/>s and also native DLLs.
    /// </summary>
    internal class AppDomainShadow : MarshalByRefObject, IDisposable
    {
        // NOTE: This keys should not be changed unless changing them also in the ExecServer.
        // They are used when multiple appdomain are sharing the same console
        private const string AppDomainLogToActionKey = "AppDomainLogToAction";

        private const string CacheFolder = ".shadow";

        private readonly object singletonLock = new object();

        private readonly string applicationPath;

        private readonly string[] nativeDllsPathOrFolderList;
        private readonly string entryAssemblyPath;
        private readonly string mainAssemblyPath;
        private bool isDllImportShadowCopy;

        private AssemblyLoaderCallback appDomainCallback;

        private readonly List<FileLoaded> filesLoaded;

        private bool isRunning;
        private bool isUpToDate = true;

        /// <summary>
        ///   Gets the name of the <see cref="AppDomain"/>.
        /// </summary>
        /// <value>The name of the application domain managed by this container.</value>
        public string Name { get; }

        /// <summary>
        ///   Gets the application domain managed by this container.
        /// </summary>
        /// <value>The application domain.</value>
        public AppDomain AppDomain { get; private set; }

        public bool ShadowCache { get; }

        public DateTime LastRunTime { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AppDomainShadow" /> class.
        /// </summary>
        /// <param name="appDomainName">Name of the application domain.</param>
        /// <param name="entryAssemblyPath">Path to the client assembly in case we need to start another instance of same process.</param>
        /// <param name="mainAssemblyPath">The main assembly path.</param>
        /// <param name="shadowCache">Whether to use shadow cache.</param>
        /// <param name="nativeDllsPathOrFolderList">An array of folders path (containing only native DLLs) or directly a specific path to a DLL.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mainAssemblyPath"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="nativeDllsPathOrFolderList"/> is a <c>null</c> reference.</exception>
        /// <exception cref="FileNotFoundException">The assembly specified by <paramref name="mainAssemblyPath"/> does not exist.</exception>
        public AppDomainShadow(string appDomainName, string entryAssemblyPath, string mainAssemblyPath, bool shadowCache, params string[] nativeDllsPathOrFolderList)
        {
            if (mainAssemblyPath is null)
                throw new ArgumentNullException(nameof(mainAssemblyPath));
            if (nativeDllsPathOrFolderList is null)
                throw new ArgumentNullException(nameof(nativeDllsPathOrFolderList));
            if (!File.Exists(mainAssemblyPath))
                throw new FileNotFoundException($"Assembly [{mainAssemblyPath}] does not exist.");

            this.Name = appDomainName;
            this.entryAssemblyPath = entryAssemblyPath;
            this.mainAssemblyPath = mainAssemblyPath;
            this.nativeDllsPathOrFolderList = nativeDllsPathOrFolderList;
            this.ShadowCache = shadowCache;

            applicationPath = Path.GetDirectoryName(mainAssemblyPath);
            filesLoaded = new List<FileLoaded>();

            CreateAppDomain();

            LastRunTime = DateTime.Now;
        }

        /// <summary>
        ///   Tries to take ownership of this container to run an executable/method from the application domain.
        /// </summary>
        /// <returns><c>true</c> if ownership was successfully taken and you can then use <see cref="Run"/>; <c>false</c> otherwise.</returns>
        public bool TryLock()
        {
            lock (singletonLock)
            {
                if (!isRunning)
                {
                    isRunning = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///   Determines whether all assemblies and native DLLs are unmodified.
        /// </summary>
        /// <returns><c>true</c> if the application domain is up-to-date; otherwise, <c>false</c>.</returns>
        public bool IsUpToDate()
        {
            if (isUpToDate)
            {
                var filesToCheck = new List<FileLoaded>();
                lock (filesLoaded)
                {
                    filesToCheck.AddRange(filesLoaded);
                }

                foreach (var fileLoaded in filesToCheck)
                {
                    if (!fileLoaded.IsUpToDate())
                    {
                        Console.WriteLine("DLL file changed: {0}", fileLoaded.FilePath);

                        isUpToDate = false;
                        break;
                    }
                }
            }

            return isUpToDate;
        }

        /// <summary>
        ///   Runs the main entry point method passing arguments to it.
        /// </summary>
        /// <param name="workingDirectory">The working directory on which to start the executable.</param>
        /// <param name="environmentVariables">The environment variables.</param>
        /// <param name="args">The arguments for the entry point.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>Return code.</returns>
        /// <exception cref="InvalidOperationException">Must call <see cref="TryLock"/> before calling this method.</exception>
        public int Run(string workingDirectory, Dictionary<string, string> environmentVariables, string[] args, NpClient<IServerLogger> callbackChannel)
        {
            if (!isRunning)
                throw new InvalidOperationException("Must call TryLock before calling this method");

            try
            {
                LastRunTime = DateTime.Now;
                appDomainCallback.CallbackChannel = callbackChannel;
                appDomainCallback.CurrentDirectory = workingDirectory;
                appDomainCallback.EnvironmentVariables = environmentVariables;
                appDomainCallback.Arguments = args;
                AppDomain.DoCallBack(appDomainCallback.Run);
                var result = AppDomain.GetData("Result") as int?;

                //var result = appDomain.ExecuteAssembly(mainAssemblyPath, args);
                Console.WriteLine("Return result: {0}", result);
                return result ?? -1;
            }
            catch (Exception ex)
            {
                callbackChannel.Proxy.OnLog(string.Format("Unexpected exception: {0}", ex), ConsoleColor.Red);
                return 1;
            }
            finally
            {
                LastRunTime = DateTime.Now;
            }
        }

        public void EndRun()
        {
            lock (singletonLock)
            {
                isRunning = false;
            }
        }

        private void AssemblyLoaded(string location)
        {
            if (!location.StartsWith(applicationPath, true, CultureInfo.InvariantCulture))
            {
                return;
            }

            if (ShadowCache && !isDllImportShadowCopy)
            {
                var cachePath = GetRootCachePath(location);
                if (cachePath != null)
                {
                    ShadowCopyNativeDlls(cachePath.FullName);
                    isDllImportShadowCopy = true;
                }
            }

            // Register the assembly in order to unload this appdomain if it is no longer relevant
            var assemblyFileName = Path.GetFileName(location);
            RegisterFileLoaded(new FileInfo(Path.Combine(applicationPath, assemblyFileName)));
        }

        // In this method, we copy all native DLLs to a subfolder under the shadow cache.
        // Each DLL has a hash computed from its name and last timestamp. This hash is used to create a directory in
        // which the DLLs will be stored.
        // Later when the AppDomain is running, using NativeLibrary.PreLoadLibrary() will use the DLL that have been copied by this instance.
        private void ShadowCopyNativeDlls(string cachePath)
        {
            // Get the shadow folder for native DLLs
            var nativeDllShadowRootFolder = Path.Combine(cachePath, "native");
            Directory.CreateDirectory(nativeDllShadowRootFolder);

            // Copy check any new native DLLs
            var appPath = Path.GetDirectoryName(mainAssemblyPath);

            foreach (var nativeDllFolderOrPath in nativeDllsPathOrFolderList)
            {
                var absolutePathOrFolder = Path.Combine(appPath, nativeDllFolderOrPath);

                // Native DLL files to load
                var files = File.Exists(absolutePathOrFolder)
                    ? new[] { new FileInfo(absolutePathOrFolder) }
                    : new DirectoryInfo(absolutePathOrFolder).EnumerateFiles("*.dll");

                var hashBuffer = new MemoryStream(new byte[1024]);
                foreach (var file in files)
                {
                    var fileHash = GetFileHash(hashBuffer, file);
                    var shadowDllPath = Path.Combine(nativeDllShadowRootFolder, fileHash, file.Name);
                    if (!File.Exists(shadowDllPath))
                    {
                        SafeCopy(file.FullName, shadowDllPath);
                    }

                    // Register our native path
                    NativeLibraryInternal.SetShadowPathForNativeDll(AppDomain, file.Name, Path.GetDirectoryName(shadowDllPath));

                    // Register this DLL
                    RegisterFileLoaded(file);
                }
            }
        }

        private DirectoryInfo GetRootCachePath(string currentPath)
        {
            var info = new DirectoryInfo(currentPath);
            while (info != null)
            {
                if (string.Compare(info.Name, "dl3", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return info;
                }
                info = info.Parent;
            }
            return null;
        }

        private static string Hash(byte[] buffer)
        {
            uint hash = 2166136261;
            for (int i = 0; i < buffer.Length; i++)
            {
                hash ^= buffer[i];
                hash *= 16777619;
            }
            return hash.ToString("x");
        }

        private static string GetFileHash(MemoryStream hashBuffer, FileInfo file)
        {
            hashBuffer.Position = 0;
            var nameAsBytes = Encoding.UTF8.GetBytes(file.FullName);
            hashBuffer.Write(nameAsBytes, 0, nameAsBytes.Length);
            var timeAsBytes = BitConverter.GetBytes(file.LastWriteTimeUtc.Ticks);
            hashBuffer.Write(timeAsBytes, 0, timeAsBytes.Length);
            return Hash(hashBuffer.ToArray());
        }

        private void RegisterFileLoaded(FileInfo file)
        {
            lock (filesLoaded)
            {
                filesLoaded.Add(new FileLoaded(file));
            }
        }

        private static void SafeCopy(string sourceFilePath, string destinationFilePath)
        {
            var fileName = Path.GetFileName(sourceFilePath);

            var destinationDirectory = Path.GetDirectoryName(destinationFilePath);

            // Case where the directory exists but the file not (not expected but got this case, will have to check why)
            if (Directory.Exists(destinationDirectory) && !File.Exists(destinationFilePath))
            {
                try
                {
                    File.Copy(sourceFilePath, destinationFilePath, true);
                    return;
                }
                catch (IOException)
                {
                }
            }

            var destinationParentDirectory = Directory.GetParent(destinationDirectory).FullName;
            var destinationTempDirectory = Path.Combine(destinationParentDirectory, Guid.NewGuid().ToString());
            Directory.CreateDirectory(destinationTempDirectory);
            bool tempDirDeleted = false;
            try
            {
                File.Copy(sourceFilePath, Path.Combine(destinationTempDirectory, fileName), true);
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.Move(destinationTempDirectory, destinationDirectory);
                    tempDirDeleted = true;
                }
            }
            catch (IOException) { }
            finally
            {
                if (!tempDirDeleted)
                {
                    try
                    {
                        Directory.Delete(destinationTempDirectory, true);
                    }
                    catch { }
                }
            }
        }

        private void CreateAppDomain()
        {
            var appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = applicationPath,
            };

            if (!appDomainSetup.ApplicationBase.EndsWith("\\"))
                appDomainSetup.ApplicationBase += "\\";

            if (ShadowCache)
            {
                // NOTE: disabled until https://developercommunity.visualstudio.com/content/problem/214568/when-using-loaderoptimizationmultidomain-assemblyr.html is fixed
                // This seems not necessary if we reuse same AppDomain
                //appDomainSetup.LoaderOptimization = LoaderOptimization.MultiDomain;
                appDomainSetup.ShadowCopyFiles = "true";
                appDomainSetup.ShadowCopyDirectories = applicationPath;
                appDomainSetup.CachePath = Path.Combine(applicationPath, CacheFolder);
            }

            // Create AppDomain
            AppDomain = AppDomain.CreateDomain(Name, AppDomain.CurrentDomain.Evidence, appDomainSetup);
            AppDomain.SetData("RealEntryAssemblyFile", entryAssemblyPath);

            // Create appDomain Callback
            appDomainCallback = new AssemblyLoaderCallback(AssemblyLoaded, mainAssemblyPath);

            // Install the appDomainCallback to prepare the new app domain
            AppDomain.DoCallBack(appDomainCallback.RegisterAssemblyLoad);
        }

        private struct FileLoaded
        {
            public FileLoaded(FileInfo file)
            {
                FilePath = file.FullName;
                lastWriteTime = file.LastWriteTimeUtc;
            }

            public readonly string FilePath;

            private readonly DateTime lastWriteTime;

            public bool IsUpToDate()
            {
                if (!File.Exists(FilePath))
                    return false;

                try
                {
                    var currentTime = new FileInfo(FilePath).LastWriteTimeUtc;
                    return currentTime == lastWriteTime;
                }
                catch (IOException) { }

                return false;
            }
        }

        [Serializable]
        private class AssemblyLoaderCallback
        {
            private const string AppDomainExecServerEntryAssemblyKey = "AppDomainExecServerEntryAssembly";
            private readonly Action<string> callback;

            private readonly string executablePath;

            public AssemblyLoaderCallback(Action<string> callback, string executablePath)
            {
                this.callback = callback;
                this.executablePath = executablePath;
            }

            public NpClient<IServerLogger> CallbackChannel { get; set; }

            public string CurrentDirectory { get; set; }

            public Dictionary<string, string> EnvironmentVariables { get; set; }

            public string[] Arguments { get; set; }

            public void RegisterAssemblyLoad()
            {
                var currentDomain = AppDomain.CurrentDomain;

                // NOTE: This part is important to have native DLLs resolved correctly by Mixed Assemblies
                var path = Environment.GetEnvironmentVariable("PATH");
                if (!path.Contains(currentDomain.BaseDirectory))
                {
                    path = currentDomain.BaseDirectory + ";" + path;
                    Environment.SetEnvironmentVariable("PATH", path);
                }

                // This method is executed in the child application domain
                currentDomain.AssemblyLoad += AppDomainOnAssemblyLoad;

                // Preload main entry point assembly
                var mainAssembly = currentDomain.Load(Path.GetFileNameWithoutExtension(executablePath));
                currentDomain.SetData(AppDomainExecServerEntryAssemblyKey, mainAssembly);
            }

            public void Run()
            {
                var currentDomain = AppDomain.CurrentDomain;
                Environment.CurrentDirectory = CurrentDirectory;

                // Set environment variables
                // TODO: We might want to reset them after; if we do so, we should make sure that the server process start without inheriting client environment in ExecServerApp.RunServerProcess()
                //       Also this probably work only if we solve PDX-2875 before
                foreach (var environmentVariable in EnvironmentVariables)
                    Environment.SetEnvironmentVariable(environmentVariable.Key, environmentVariable.Value);

                currentDomain.SetData(AppDomainLogToActionKey, new Action<string, ConsoleColor>((text, color) => CallbackChannel.Proxy.OnLog(text, color)));
                var assembly = (Assembly) currentDomain.GetData(AppDomainExecServerEntryAssemblyKey);
                AppDomain.CurrentDomain.SetData("Result", currentDomain.ExecuteAssemblyByName(assembly.FullName, Arguments));
                //AppDomain.CurrentDomain.SetData("Result", Convert.ToInt32(assembly.EntryPoint.Invoke(null, new object[] { Arguments })));

                // Force a GC after the process is finished
                GC.Collect(2, GCCollectionMode.Forced);
            }

            private void AppDomainOnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
            {
                var assembly = args.LoadedAssembly;
                if (!assembly.IsDynamic)
                {
                    // This method will be executed in the ExecServer application domain
                    callback(assembly.Location);
                }
            }
        }

        public void Dispose()
        {
            System.AppDomain.Unload(AppDomain);
            Console.WriteLine("AppDomain {0} Disposed", Name);
        }
    }
}
