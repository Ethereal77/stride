// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1300 // Element must begin with upper-case letter
#pragma warning disable SA1649 // File name must match first type name

using System;
using System.Runtime.CompilerServices;

namespace Stride.Core
{
    /// <summary>
    /// Utility class.
    /// </summary>
    internal sealed class Interop
    {
        public static T Pin<T>(ref T source) where T : struct
        {
            throw new NotImplementedException();
        }

        public static T IncrementPinned<T>(T source) where T : struct
        {
            throw new NotImplementedException();
        }

        public static T AddPinned<T>(T source, int offset) where T : struct
        {
            throw new NotImplementedException();
        }

        public static void Pin<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }

        public static unsafe void* Fixed<T>(ref T data)
        {
            throw new NotImplementedException();
        }

        public static unsafe void* FixedOut<T>(out T data)
        {
            throw new NotImplementedException();
        }

        public static unsafe void* Fixed<T>(T[] data)
        {
            throw new NotImplementedException();
        }

        public static unsafe void* Cast<T>(ref T data) where T : struct
        {
            throw new NotImplementedException();
        }

        public static unsafe void* CastOut<T>(out T data) where T : struct
        {
            throw new NotImplementedException();
        }

        public static TCAST[] CastArray<TCAST, T>(T[] arrayData)
            where T : struct
            where TCAST : struct
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void memcpy(void* pDest, void* pSrc, int count)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void memset(void* pDest, byte value, int count)
        {
            throw new NotImplementedException();
        }

        public static unsafe void* Read<T>(void* pSrc, ref T data) where T : struct
        {
            throw new NotImplementedException();
        }

        public static unsafe T ReadInline<T>(void* pSrc) where T : struct
        {
            throw new NotImplementedException();
        }

        public static unsafe void WriteInline<T>(void* pDest, ref T data) where T : struct
        {
            throw new NotImplementedException();
        }

        public static unsafe void CopyInline<T>(ref T data, void* pSrc) where T : struct
        {
            throw new NotImplementedException();
        }

        public static unsafe void CopyInline<T>(void* pDest, ref T srcData)
        {
            throw new NotImplementedException();
        }

        public static unsafe void CopyInlineOut<T>(out T data, void* pSrc)
        {
            throw new NotImplementedException();
        }

        public static unsafe void* ReadOut<T>(void* pSrc, out T data) where T : struct
        {
            throw new NotImplementedException();
        }

        public static unsafe void* Read<T>(void* pSrc, T[] data, int offset, int count) where T : struct
        {
            throw new NotImplementedException();
        }

        public static unsafe void* Read2D<T>(void* pSrc, T[,] data, int offset, int count) where T : struct
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<T>()
        {
            throw new NotImplementedException();
        }

        public static unsafe void* Write<T>(void* pDest, ref T data) where T : struct
        {
            throw new NotImplementedException();
        }

        public static unsafe void* Write<T>(void* pDest, T[] data, int offset, int count) where T : struct
        {
            throw new NotImplementedException();
        }

        public static unsafe void* Write2D<T>(void* pDest, T[,] data, int offset, int count) where T : struct
        {
            throw new NotImplementedException();
        }
    }
}
