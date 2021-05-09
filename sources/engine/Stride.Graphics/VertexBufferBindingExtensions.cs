// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Graphics
{
    public static class VertexBufferBindingExtensions
    {
        public static InputElementDescription[] CreateInputElements(this VertexDeclaration vertexDeclaration)
        {
            var inputElements = new InputElementDescription[vertexDeclaration.VertexElements.Length];
            var inputElementIndex = 0;
            foreach (var element in vertexDeclaration.EnumerateWithOffsets())
            {
                inputElements[inputElementIndex++] = new InputElementDescription
                {
                    SemanticName = element.VertexElement.SemanticName,
                    SemanticIndex = element.VertexElement.SemanticIndex,
                    Format = element.VertexElement.Format,
                    InputSlot = 0,
                    AlignedByteOffset = element.Offset,
                };
            }

            return inputElements;
        }

        public static InputElementDescription[] CreateInputElements(this VertexBufferBinding[] vertexBuffers)
        {
            var inputElementCount = 0;
            foreach (var vertexBuffer in vertexBuffers)
            {
                inputElementCount += vertexBuffer.Declaration.VertexElements.Length;
            }

            var inputElements = new InputElementDescription[inputElementCount];
            var inputElementIndex = 0;
            for (int inputSlot = 0; inputSlot < vertexBuffers.Length; inputSlot++)
            {
                var vertexBuffer = vertexBuffers[inputSlot];
                foreach (var element in vertexBuffer.Declaration.EnumerateWithOffsets())
                {
                    inputElements[inputElementIndex++] = new InputElementDescription
                    {
                        SemanticName = element.VertexElement.SemanticName,
                        SemanticIndex = element.VertexElement.SemanticIndex,
                        Format = element.VertexElement.Format,
                        InputSlot = inputSlot,
                        AlignedByteOffset = element.Offset,
                    };
                }
            }

            return inputElements;
        }
    }
}
