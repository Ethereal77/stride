// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

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
