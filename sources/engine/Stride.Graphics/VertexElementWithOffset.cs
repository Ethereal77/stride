// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Graphics
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
