// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Runtime.InteropServices;

namespace Stride.Core.Native
{
    public abstract class NativeLz4Base
    {
        static NativeLz4Base()
        {
            NativeLibrary.PreloadLibrary(NativeInvoke.Library, typeof(NativeLz4Base));
        }

        [DllImport(NativeInvoke.Library, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe int LZ4_uncompress(byte* source, byte* dest, int maxOutputSize);

        [DllImport(NativeInvoke.Library, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe int LZ4_uncompress_unknownOutputSize(byte* source, byte* dest, int inputSize, int maxOutputSize);

        [DllImport(NativeInvoke.Library, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe int LZ4_compress_limitedOutput(byte* source, byte* dest, int inputSize, int maxOutputSize);

        [DllImport(NativeInvoke.Library, CallingConvention = CallingConvention.Cdecl)]
        protected static extern unsafe int LZ4_compressHC_limitedOutput(byte* source, byte* dest, int inputSize, int maxOutputSize);
    }
}
