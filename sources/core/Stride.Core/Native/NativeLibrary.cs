// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma warning disable SA1307 // Accessible fields must begin with upper-case letter
#pragma warning disable SA1310 // Field names must not contain underscore

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Stride.Core
{
    public static class NativeLibrary
    {
        private static readonly Dictionary<string, IntPtr> LoadedLibraries = new Dictionary<string, IntPtr>();

        [DllImport("kernel32", EntryPoint = "LoadLibrary", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int FreeLibrary(IntPtr libraryHandle);

        /// <summary>
        ///   Try to preload a library.
        ///   This is useful when we want to have AnyCPU .NET and CPU-specific native code.
        /// </summary>
        /// <param name="libraryName">Name of the library.</param>
        /// <param name="owner">Type whose Assembly location is related to the native library (we can't use GetCallingAssembly as it might be wrong due to optimizations).</param>
        /// <exception cref="System.InvalidOperationException">
        ///   Library could not be loaded.
        /// </exception>
        public static void PreloadLibrary(string libraryName, Type owner)
        {
            NormalizeLibName(ref libraryName);
            lock (LoadedLibraries)
            {
                // If already loaded, just exit as we want to load it just once
                if (LoadedLibraries.ContainsKey(libraryName))
                {
                    return;
                }

                GetNativeSystemInfo(out var systemInfo);
                string cpu = IntPtr.Size == 8 ? "x64" : "x86";

                // We are trying to load the dll from a shadow path if it is already registered, otherwise we use it directly from the folder
                string basePath;
                {
                    var dllFolder = NativeLibraryInternal.GetShadowPathForNativeDll(libraryName);
                    if (dllFolder == null)
                        dllFolder = Path.Combine(Path.GetDirectoryName(owner.GetTypeInfo().Assembly.Location), cpu);
                    if (!Directory.Exists(dllFolder))
                        dllFolder = Path.Combine(Environment.CurrentDirectory, cpu);
                    var libraryFilename = Path.Combine(dllFolder, libraryName);
                    basePath = libraryFilename;

                    if (File.Exists(libraryFilename))
                    {
                        var result = LoadLibrary(libraryFilename);
                        if (result != IntPtr.Zero)
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

                    if (File.Exists(libraryFilename))
                    {
                        var result = LoadLibrary(libraryFilename);
                        if (result != IntPtr.Zero)
                        {
                            LoadedLibraries.Add(libraryName.ToLowerInvariant(), result);
                            return;
                        }
                    }
                }

                throw new InvalidOperationException($"Could not load native library {libraryName} from path [{basePath}] using CPU architecture {cpu}.");
            }
        }

        /// <summary>
        ///   Unloads a specific native dynamic library loaded previously by <see cref="LoadLibrary" />.
        /// </summary>
        /// <param name="libraryName">Name of the library to unload.</param>
        public static void UnLoad(string libraryName)
        {
            NormalizeLibName(ref libraryName);
            lock (LoadedLibraries)
            {
                if (LoadedLibraries.TryGetValue(libraryName, out var libHandle))
                {
                    FreeLibrary(libHandle);
                    LoadedLibraries.Remove(libraryName);
                }
            }
        }

        /// <summary>
        ///   Unloads all native dynamic libraries loaded previously by <see cref="LoadLibrary"/>.
        /// </summary>
        public static void UnLoadAll()
        {
            lock (LoadedLibraries)
            {
                foreach (var libraryItem in LoadedLibraries)
                {
                    FreeLibrary(libraryItem.Value);
                }
                LoadedLibraries.Clear();
            }
        }

        private static void NormalizeLibName(ref string libName)
        {
            libName = libName.ToLowerInvariant();
            if (libName.EndsWith(".dll") == false)
            {
                libName += ".dll";
            }
        }

        private const string SYSINFO_FILE = "kernel32.dll";

        [DllImport(SYSINFO_FILE)]
        private static extern void GetNativeSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEM_INFO
        {
            public PROCESSOR_ARCHITECTURE processorArchitecture;
            private ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }

        private enum PROCESSOR_ARCHITECTURE : ushort
        {
            PROCESSOR_ARCHITECTURE_AMD64 = 9,
            PROCESSOR_ARCHITECTURE_ARM = 5,
            PROCESSOR_ARCHITECTURE_IA64 = 6,
            PROCESSOR_ARCHITECTURE_INTEL = 0,
            PROCESSOR_ARCHITECTURE_UNKNOWN = 0xffff,
        }
    }
}
