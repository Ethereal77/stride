// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using System.Reflection;

using Stride.Core.Annotations;

namespace Stride.Core
{
    /// <summary>
    ///   Defines helper members to access the folders used for the running platform.
    /// </summary>
    public static class PlatformFolders
    {
        // TODO: This class should not try to initialize directories, etc. Try to find another way to do this.

        /// <summary>
        ///   The system temporary directory.
        /// </summary>
        public static readonly string TemporaryDirectory = GetTemporaryDirectory();

        /// <summary>
        ///   The application temporary directory.
        /// </summary>
        public static readonly string ApplicationTemporaryDirectory = GetApplicationTemporaryDirectory();

        /// <summary>
        ///   The application local directory, where the user can write local data (included in backup).
        /// </summary>
        public static readonly string ApplicationLocalDirectory = GetApplicationLocalDirectory();

        /// <summary>
        ///   The application roaming directory, where the user can write roaming data (included in backup).
        /// </summary>
        public static readonly string ApplicationRoamingDirectory = GetApplicationRoamingDirectory();

        /// <summary>
        ///   The application cache directory, where the user can write data that won't be backup.
        /// </summary>
        public static readonly string ApplicationCacheDirectory = GetApplicationCacheDirectory();

        /// <summary>
        ///   The application data directory, where data is deployed. It could be read-only on some platforms.
        /// </summary>
        public static readonly string ApplicationDataDirectory = GetApplicationDataDirectory();

        /// <summary>
        ///   The optional application data subdirectory. If not <c>null</c> or empty, <c>/data</c> will be mounted on
        ///   <c>"<see cref="ApplicationDataDirectory"/>/<see cref="ApplicationDataSubDirectory"/>"</c>.
        /// </summary>
        /// <remarks>
        ///   This property should not be written after the VirtualFileSystem static initialization. If so, an
        ///   <see cref="InvalidOperationExeception"/> will be thrown.
        /// </remarks>
        public static string ApplicationDataSubDirectory
        {
            get => applicationDataSubDirectory;

            set
            {
                if (IsVirtualFileSystemInitialized)
                    throw new InvalidOperationException("ApplicationDataSubDirectory cannot be modified after the VirtualFileSystem has been initialized.");

                applicationDataSubDirectory = value;
            }
        }

        /// <summary>
        ///   The application directory, where assemblies are deployed. It could be read-only on some platforms.
        /// </summary>
        public static readonly string ApplicationBinaryDirectory = GetApplicationBinaryDirectory();

        /// <summary>
        ///   The application executable path.
        /// </summary>
        /// <remarks>Might be <c>null</c> if the starting executable is unknown.</remarks>
        public static readonly string ApplicationExecutablePath = GetApplicationExecutablePath();

        private static string applicationDataSubDirectory = string.Empty;

        /// <summary>
        ///   Gets a value indicating whether the virtual file system has been initialized.
        /// </summary>
        public static bool IsVirtualFileSystemInitialized { get; internal set; }

        [NotNull]
        private static string GetApplicationLocalDirectory()
        {
            // TODO: Should we add "local" ?
            var directory = Path.Combine(GetApplicationBinaryDirectory(), "local");
            Directory.CreateDirectory(directory);
            return directory;
        }

        [NotNull]
        private static string GetApplicationRoamingDirectory()
        {
            // TODO: Should we add "roaming" ?
            var directory = Path.Combine(GetApplicationBinaryDirectory(), "roaming");
            Directory.CreateDirectory(directory);
            return directory;
        }

        [NotNull]
        private static string GetApplicationCacheDirectory()
        {
            // TODO: Should we add "cache" ?
            var directory = Path.Combine(GetApplicationBinaryDirectory(), "cache");
            Directory.CreateDirectory(directory);
            return directory;
        }

        private static string GetApplicationExecutablePath() => Assembly.GetEntryAssembly()?.Location;

        [NotNull]
        private static string GetTemporaryDirectory() => GetApplicationTemporaryDirectory();

        [NotNull]
        private static string GetApplicationTemporaryDirectory() => Path.GetTempPath();

        [NotNull]
        private static string GetApplicationBinaryDirectory() => FindCoreAssemblyDirectory(GetApplicationExecutableDirectory());

        private static string GetApplicationExecutableDirectory()
        {
            var executableName = GetApplicationExecutablePath();
            if (!string.IsNullOrEmpty(executableName))
                return Path.GetDirectoryName(executableName);

            return AppContext.BaseDirectory;
        }

        private static string FindCoreAssemblyDirectory(string entryDirectory)
        {
            // Simple case
            var corePath = Path.Combine(entryDirectory, "Stride.Core.dll");
            if (File.Exists(corePath))
                return entryDirectory;

            // Search one level down
            foreach (var subfolder in Directory.GetDirectories(entryDirectory))
            {
                corePath = Path.Combine(subfolder, "Stride.Core.dll");
                if (File.Exists(corePath))
                    return subfolder;
            }

            // If nothing found, return input
            return entryDirectory;
        }

        [NotNull]
        private static string GetApplicationDataDirectory()
        {
            return Path.Combine(GetApplicationBinaryDirectory(), "data");
        }
    }
}
