// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Graphics
{
    public struct DataPointer
    {
        public unsafe DataPointer(void* pointer, int size)
        {
            Pointer = (IntPtr)pointer;
            Size = size;
        }

        public DataPointer(IntPtr pointer, int size)
        {
            Pointer = pointer;
            Size = size;
        }

        public IntPtr Pointer;

        public int Size;
    }
}
