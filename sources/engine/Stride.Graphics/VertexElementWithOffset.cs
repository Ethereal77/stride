// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Graphics
{
    public struct VertexElementWithOffset
    {
        public VertexElement VertexElement;
        public int Offset;
        public int Size;

        public VertexElementWithOffset(VertexElement vertexElement, int offset, int size)
        {
            VertexElement = vertexElement;
            Offset = offset;
            Size = size;
        }
    }
}
