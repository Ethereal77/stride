// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Runtime.InteropServices;

namespace Xenko.Core.Native
{
    public abstract class NativeLz4Base
    {
        static NativeLz4Base()
        {
            NativeLibrary.PreloadLibrary(NativeInvoke.LibraryName, typeof(NativeLz4Base));
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
