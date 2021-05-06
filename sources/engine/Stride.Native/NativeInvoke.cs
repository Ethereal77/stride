// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Security;

using Stride.Core;
using Stride.Core.Mathematics;

namespace Stride.Native
{
    internal static class NativeInvoke
    {
        internal const string Library = "libstride";

        internal static void PreLoad()
        {
            NativeLibraryHelper.Load("libstride", typeof(NativeInvoke));
        }

        static NativeInvoke()
        {
            PreLoad();
        }

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateBufferValuesFromElementInfo(IntPtr drawInfo, IntPtr vertexPtr, IntPtr indexPtr, int vertexOffset);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern void xnGraphicsFastTextRendererGenerateVertices(RectangleF constantInfos, RectangleF renderInfos, string text, out IntPtr textLength, out IntPtr vertexBufferPointer);
    }

    internal class Module
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            Core.Native.NativeInvoke.Setup();
        }
    }
}
