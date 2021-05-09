// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Graphics;
using Stride.Graphics.GeometricPrimitives;
using Stride.Rendering;

namespace Stride.Extensions
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
