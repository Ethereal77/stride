// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace Stride.Core.Native
{
    public static class NativeInvoke
    {
        internal const string Library = "libcore";

        static NativeInvoke()
        {
            NativeLibraryHelper.PreloadLibrary("libcore", typeof(NativeInvoke));
        }

        /// <summary>
        ///   Suspends the current thread for a specified timespan.
        /// </summary>
        /// <param name="ms">Number of milliseconds to sleep.</param>
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Library, EntryPoint = "cnSleep", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Sleep(int ms);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ManagedLogDelegate(string log);

        private static ManagedLogDelegate managedLogDelegateSingleton;

        private static void ManagedLog(string log)
        {
            Debug.WriteLine(log);
        }

        public static void Setup()
        {
            managedLogDelegateSingleton = ManagedLog;

            var ptr = Marshal.GetFunctionPointerForDelegate(managedLogDelegateSingleton);

            CoreNativeSetup(ptr);
        }

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Library, EntryPoint = "cnSetup", CallingConvention = CallingConvention.Cdecl)]
        private static extern void CoreNativeSetup(IntPtr logger);
    }
}
