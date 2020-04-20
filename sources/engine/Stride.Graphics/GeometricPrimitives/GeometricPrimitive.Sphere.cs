// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

// -----------------------------------------------------------------------------
// The following code is a port of DirectXTk http://directxtk.codeplex.com
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System;

using Stride.Core.Mathematics;

namespace Stride.Graphics.GeometricPrimitives
{
    public partial class GeometricPrimitive
    {
        /// <summary>
        /// A sphere primitive.
        /// </summary>
        public static class Sphere
        {
            /// <summary>
            /// Creates a sphere primitive.
            /// </summary>
            /// <param name="device">The device.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="tessellation">The tessellation.</param>
            /// <param name="uScale">The u scale.</param>
            /// <param name="vScale">The v scale.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is false.</param>
            /// <returns>A sphere primitive.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">tessellation;Must be &gt;= 3</exception>
            public static GeometricPrimitive New(GraphicsDevice device, float radius = 0.5f, int tessellation = 16, float uScale = 1.0f, float vScale = 1.0f, bool toLeftHanded = false)
            {
                return new GeometricPrimitive(device, New(radius, tessellation, uScale, vScale, toLeftHanded));
            }

            /// <summary>
            /// Creates a sphere primitive.
            /// </summary>
            /// <param name="radius">The radius.</param>
            /// <param name="tessellation">The tessellation.</param>
            /// <param name="uScale">The u scale.</param>
            /// <param name="vScale">The v scale.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is false.</param>
            /// <returns>A sphere primitive.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">tessellation;Must be &gt;= 3</exception>
            public static GeometricMeshData<VertexPositionNormalTexture> New(float radius = 0.5f, int tessellation = 16, float uScale = 1.0f, float vScale = 1.0f, bool toLeftHanded = false)
            {
                if (tessellation < 3) tessellation = 3;

                int verticalSegments = tessellation;
                int horizontalSegments = tessellation * 2;

                var vertices = new VertexPositionNormalTexture[(verticalSegments + 1) * (horizontalSegments + 1)];
                var indices = new int[(verticalSegments) * (horizontalSegments + 1) * 6];

                int vertexCount = 0;

                // generate the first extremity points
                for (int j = 0; j <= horizontalSegments; j++)
                {
                    var normal = new Vector3(0, -1, 0);
                    var textureCoordinate = new Vector2(uScale * j / horizontalSegments, vScale);
                    vertices[vertexCount++] = new VertexPositionNormalTexture(normal * radius, normal, textureCoordinate);
                }

                // Create rings of vertices at progressively higher latitudes.
                for (int i = 1; i < verticalSegments; i++)
                {
                    float v = vScale * (1.0f - (float)i / verticalSegments);

                    var latitude = (float)((i * Math.PI / verticalSegments) - Math.PI / 2.0);
                    var dy = (float)Math.Sin(latitude);
                    var dxz = (float)Math.Cos(latitude);

                    // the first point
                    var firstNormal = new Vector3(0, dy, dxz);
                    var firstHorizontalVertex = new VertexPositionNormalTexture(firstNormal * radius, firstNormal, new Vector2(0, v));
                    vertices[vertexCount++] = firstHorizontalVertex;

                    // Create a single ring of vertices at this latitude.
                    for (int j = 1; j < horizontalSegments; j++)
                    {
                        float u = (uScale * j) / horizontalSegments;

                        var longitude = (float)(j * 2.0 * Math.PI / horizontalSegments);
                        var dx = (float)Math.Sin(longitude);
                        var dz = (float)Math.Cos(longitude);

                        dx *= dxz;
                        dz *= dxz;

                        var normal = new Vector3(dx, dy, dz);
                        var textureCoordinate = new Vector2(u, v);

                        vertices[vertexCount++] = new VertexPositionNormalTexture(normal * radius, normal, textureCoordinate);
                    }

                    // the last point equal to the first point
                    firstHorizontalVertex.TextureCoordinate = new Vector2(uScale, v);
                    vertices[vertexCount++] = firstHorizontalVertex;
                }

                // generate the end extremity points
                for (int j = 0; j <= horizontalSegments; j++)
                {
                    var normal = new Vector3(0, 1, 0);
                    var textureCoordinate = new Vector2(uScale * j / horizontalSegments, 0f);
                    vertices[vertexCount++] = new VertexPositionNormalTexture(normal * radius, normal, textureCoordinate);
                }

                // Fill the index buffer with triangles joining each pair of latitude rings.
                int stride = horizontalSegments + 1;

                int indexCount = 0;
                for (int i = 0; i < verticalSegments; i++)
                {
                    for (int j = 0; j <= horizontalSegments; j++)
                    {
                        int nextI = i + 1;
                        int nextJ = (j + 1) % stride;

                        indices[indexCount++] = (i * stride + j);
                        indices[indexCount++] = (nextI * stride + j);
                        indices[indexCount++] = (i * stride + nextJ);

                        indices[indexCount++] = (i * stride + nextJ);
                        indices[indexCount++] = (nextI * stride + j);
                        indices[indexCount++] = (nextI * stride + nextJ);
                    }
                }

                // Create the primitive object.
                return new GeometricMeshData<VertexPositionNormalTexture>(vertices, indices, toLeftHanded) { Name = "Sphere" };
            }
        }
    }
}
