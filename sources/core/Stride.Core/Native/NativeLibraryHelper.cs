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
    ///   Provides helper methods to load and cache native libraries and unload them.
    /// </summary>
    public static class NativeLibraryHelper
    {
        private static readonly string CpuArchitecture = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => "x86",
            Architecture.X64 => "x64",
            _ => throw new PlatformNotSupportedException()
        };

        private static readonly string PlatformName = Platform.Type switch
        {
            PlatformType.Windows => "win",
            _ => throw new PlatformNotSupportedException()
        };

        private static readonly Dictionary<string, IntPtr> loadedLibraries = new();

        /// <summary>
        ///   Tries to load a native library.
        /// </summary>
        /// <param name="libraryName">The name of the native library.</param>
        /// <param name="owner">
        ///   The type whose assembly location is related to the native library
        ///   (we can't use GetCallingAssembly as it might be wrong due to optimizations).
        /// </param>
        /// <remarks>
        ///   Preloading a native library is useful when you want to have an <c>AnyCPU</c> .NET assembly
        ///   and CPU-specific native code.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="libraryName"/> or <paramref name="owner"/> are a <c>null</c> reference,
        ///   or <paramref name="libraryName"/> is an empty <see langword="string"/>.
        /// </exception>
        /// <exception cref="DllNotFoundException">The library could not be loaded.</exception>
        public static void Load(string libraryName, Type owner)
        {
            if (string.IsNullOrWhiteSpace(libraryName))
                throw new ArgumentNullException(nameof(libraryName));
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            lock (loadedLibraries)
            {
                // If already loaded, just exit as we want to load it just once
                if (loadedLibraries.ContainsKey(libraryName))
                    return;

                // We are trying to load the DLL from a shadow path if it is already registered, otherwise we use it directly from the folder
                var ownerTypeAssemblyPath = Path.GetDirectoryName(owner.GetTypeInfo().Assembly.Location);
                var searchPaths = new[]
                {
                    Path.Combine(ownerTypeAssemblyPath, $"{PlatformName}-{CpuArchitecture}"),
                    Path.Combine(Environment.CurrentDirectory, $"{PlatformName}-{CpuArchitecture}"),
                    // Also try without platform for Windows-only packages (backward compat for editor packages)
                    Path.Combine(ownerTypeAssemblyPath, $"{CpuArchitecture}"),
                    Path.Combine(Environment.CurrentDirectory, $"{CpuArchitecture}"),
                };

                foreach (var libraryPath in searchPaths)
                {
                    var libraryFilename = Path.Combine(libraryPath, libraryName);
                    if (NativeLibrary.TryLoad(libraryFilename, out var result))
                    {
                        loadedLibraries.Add(libraryName.ToLowerInvariant(), result);
                        return;
                    }
                }

                // Attempt to load it from PATH
                foreach (var p in Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator))
                {
                    var libraryFilename = Path.Combine(p, libraryName);
                    if (NativeLibrary.TryLoad(libraryFilename, out var result))
                    {
                        loadedLibraries.Add(libraryName.ToLowerInvariant(), result);
                        return;
                    }
                }

                throw new DllNotFoundException($"Could not load native library {libraryName} using CPU architecture {CpuArchitecture}.");
            }
        }

        /// <summary>
        ///   Unloads a specific native library loaded previously by <see cref="Load"/>.
        /// </summary>
        /// <param name="libraryName">The name of the native library to unload.</param>
        public static void Unload(string libraryName)
        {
            lock (loadedLibraries)
            {
                if (loadedLibraries.TryGetValue(libraryName, out IntPtr libHandle))
                {
                    NativeLibrary.Free(libHandle);
                    loadedLibraries.Remove(libraryName);
                }
            }
        }

        /// <summary>
        ///   Unloads all the native libraries loaded previously by <see cref="Load"/>.
        /// </summary>
        public static void UnloadAll()
        {
            lock (loadedLibraries)
            {
                foreach (var libraryItem in loadedLibraries)
                {
                    NativeLibrary.Free(libraryItem.Value);
                }
                loadedLibraries.Clear();
            }
        }
    }
}
