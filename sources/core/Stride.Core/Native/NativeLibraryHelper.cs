// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Stride.Core
{
    /// <summary>
    ///   Contains helper methods to load and cache native libraries and unload them.
    /// </summary>
    public static class NativeLibraryHelper
    {
        private static readonly Dictionary<string, IntPtr> LoadedLibraries = new Dictionary<string, IntPtr>();

        /// <summary>
        ///   Tries to preload the library.
        /// </summary>
        /// <param name="libraryName">The name of the library.</param>
        /// <param name="owner">
        ///   The type whose assembly location is related to the native library
        ///   (we can't use GetCallingAssembly as it might be wrong due to optimizations).
        /// </param>
        /// <remarks>
        ///   This is useful when we want to have AnyCPU .NET and CPU-specific native code. Only available on Windows for now.
        /// </remarks>
        /// <exception cref="InvalidOperationException">The library could not be loaded.</exception>
        public static void PreloadLibrary(string libraryName, Type owner)
        {
            lock (LoadedLibraries)
            {
                // If already loaded, just exit as we want to load it just once
                if (LoadedLibraries.ContainsKey(libraryName))
                    return;

                string cpu;
                string platform;

                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X86:
                        cpu = "x86";
                        break;
                    case Architecture.X64:
                        cpu = "x64";
                        break;

                    default:
                        throw new PlatformNotSupportedException();
                }

                switch (Platform.Type)
                {
                    case PlatformType.Windows:
                        platform = "win";
                        break;

                    default:
                        throw new PlatformNotSupportedException();
                }

                // We are trying to load the dll from a shadow path if it is already registered, otherwise we use it directly from the folder
                {
                    foreach (var libraryPath in new[]
                    {
                        Path.Combine(Path.GetDirectoryName(owner.GetTypeInfo().Assembly.Location), $"{platform}-{cpu}"),
                        Path.Combine(Environment.CurrentDirectory, $"{platform}-{cpu}"),
                        // Also try without platform for Windows-only packages (backward compat for editor packages)
                        Path.Combine(Path.GetDirectoryName(owner.GetTypeInfo().Assembly.Location), $"{cpu}"),
                        Path.Combine(Environment.CurrentDirectory, $"{cpu}"),
                    })
                    {
                        var libraryFilename = Path.Combine(libraryPath, libraryName);
                        if (NativeLibrary.TryLoad(libraryFilename, out var result))
                        {
                            LoadedLibraries.Add(libraryName.ToLowerInvariant(), result);
                            return;
                        }
                    }
                }

                // Attempt to load it from PATH
                foreach (var p in Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator))
                {
                    var libraryFilename = Path.Combine(p, libraryName);
                    if (NativeLibrary.TryLoad(libraryFilename, out var result))
                    {
                        LoadedLibraries.Add(libraryName.ToLowerInvariant(), result);
                        return;
                    }
                }

                throw new InvalidOperationException($"Could not load native library {libraryName} using CPU architecture {cpu}.");
            }
        }

        /// <summary>
        ///   Unloads a specific native dynamic library loaded previously by <see cref="PreloadLibrary"/>.
        /// </summary>
        /// <param name="libraryName">The name of the library to unload.</param>
        public static void Unload(string libraryName)
        {
            lock (LoadedLibraries)
            {
                if (LoadedLibraries.TryGetValue(libraryName, out IntPtr libHandle))
                {
                    NativeLibrary.Free(libHandle);
                    LoadedLibraries.Remove(libraryName);
                }
            }
        }

        /// <summary>
        ///   Unloads all the native dynamic libraries loaded previously by <see cref="PreloadLibrary"/>.
        /// </summary>
        public static void UnloadAll()
        {
            lock (LoadedLibraries)
            {
                foreach (var libraryItem in LoadedLibraries)
                {
                    NativeLibrary.Free(libraryItem.Value);
                }
                LoadedLibraries.Clear();
            }
        }
    }
}
