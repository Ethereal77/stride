// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Graphics;
using Xenko.Graphics.GeometricPrimitives;
using Xenko.Rendering;

namespace Xenko.Extensions
{
    /// <summary>
    /// An extension class for the <see cref="GeometricPrimitive"/>
    /// </summary>
    public static class GeometricPrimitiveExtensions
    {
        public static MeshDraw ToMeshDraw<T>(this GeometricPrimitive<T> primitive) where T : struct, IVertex
        {
            var vertexBufferBinding = new VertexBufferBinding(primitive.VertexBuffer, new T().GetLayout(), primitive.VertexBuffer.ElementCount);
            var indexBufferBinding = new IndexBufferBinding(primitive.IndexBuffer, primitive.IsIndex32Bits, primitive.IndexBuffer.ElementCount);
            var data = new MeshDraw
            {
                StartLocation = 0, 
                PrimitiveType = PrimitiveType.TriangleList, 
                VertexBuffers = new[] { vertexBufferBinding }, 
                IndexBuffer = indexBufferBinding, 
                DrawCount = primitive.IndexBuffer.ElementCount,
            };

            return data;
        }
    }
}
