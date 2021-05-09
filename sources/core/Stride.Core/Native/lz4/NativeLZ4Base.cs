// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Stride.Core.Native
{
    public abstract class NativeLz4Base
    {
        static NativeLz4Base()
        {
            NativeLibraryHelper.Load(NativeInvoke.Library, typeof(NativeLz4Base));
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
