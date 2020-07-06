// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Graphics
{
    internal class VertexElementValidator
    {
        internal static int GetVertexStride(VertexElement[] elements)
        {
            int stride = 0;
            for (int i = 0; i < elements.Length; i++)
                stride += elements[i].Format.SizeInBytes();
            return stride;
        }

        internal static void Validate(int vertexStride, VertexElement[] elements)
        {
            if (vertexStride <= 0)
                throw new ArgumentOutOfRangeException(nameof(vertexStride));
            if ((vertexStride & 3) != 0)
                throw new ArgumentException("The stride of a vertex must be a multiple of 4 bytes,", nameof(vertexStride));

            var elementBytes = new sbyte[vertexStride];
            for (int i = 0; i < vertexStride; i++)
                elementBytes[i] = -1;

            int totalOffset = 0;
            for (int elementIndex = 0; elementIndex < elements.Length; elementIndex++)
            {
                int offset = elements[elementIndex].AlignedByteOffset;
                if (offset == VertexElement.AppendAligned)
                    // The first element has offset 0
                    offset = (elementIndex > 0)
                        ? totalOffset + elements[elementIndex - 1].Format.SizeInBytes()
                        : 0;

                totalOffset = offset;
                int typeSize = elements[elementIndex].Format.SizeInBytes();
                if ((offset < 0) || ((offset + typeSize) > vertexStride))
                    throw new ArgumentException("The vertex elements' size is greater than the declared vertex stride.", nameof(elements));
                if ((offset & 3) != 0)
                    throw new ArgumentException("The offset of a vertex element must be aligned to 4 bytes,", nameof(elements));

                for (int j = 0; j < elementIndex; j++)
                {
                    if (elements[elementIndex].SemanticName == elements[j].SemanticName &&
                        elements[elementIndex].SemanticIndex == elements[j].SemanticIndex)
                        throw new ArgumentException("Duplicate vertex element.", nameof(elements));
                }

                for (int byteIndex = offset; byteIndex < (offset + typeSize); byteIndex++)
                {
                    if (elementBytes[byteIndex] >= 0)
                        throw new ArgumentException("Detected overlapping vertex elements.", nameof(elements));

                    elementBytes[byteIndex] = (sbyte) elementIndex;
                }
            }
        }
    }
}
