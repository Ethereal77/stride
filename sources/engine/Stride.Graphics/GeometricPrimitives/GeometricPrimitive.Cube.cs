// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

// -----------------------------------------------------------------------------
// The following code is a port of DirectXTk http://directxtk.codeplex.com
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System;

using Xenko.Core.Mathematics;

namespace Xenko.Graphics.GeometricPrimitives
{
    public partial class GeometricPrimitive
    {
        /// <summary>
        /// A cube has six faces, each one pointing in a different direction.
        /// </summary>
        public static class Cube
        {
            // TODO: Add support to tesselate the faces of the cube

            private const int CubeFaceCount = 6;

            private static readonly Vector3[] FaceNormals = new Vector3[CubeFaceCount]
                {
                    new Vector3(0, 0, 1),
                    new Vector3(0, 0, -1),
                    new Vector3(1, 0, 0),
                    new Vector3(-1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, -1, 0),
                };

            private static readonly Vector2[] TextureCoordinates = new Vector2[4]
                {
                    new Vector2(1, 0),
                    new Vector2(1, 1),
                    new Vector2(0, 1),
                    new Vector2(0, 0),
                };

            /// <summary>
            /// Creates a cube with six faces each one pointing in a different direction.
            /// </summary>
            /// <param name="device">The device.</param>
            /// <param name="size">The size.</param>
            /// <param name="uScale">Scale U coordinates between 0 and the values of this parameter.</param>
            /// <param name="vScale">Scale V coordinates 0 and the values of this parameter.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is false.</param>
            /// <returns>A cube.</returns>
            public static GeometricPrimitive New(GraphicsDevice device, float size = 1.0f, float uScale = 1.0f, float vScale = 1.0f, bool toLeftHanded = false)
            {
                // Create the primitive object.
                return new GeometricPrimitive(device, New(size, uScale, vScale, toLeftHanded));
            }

            /// <summary>
            /// Creates a cube with six faces each one pointing in a different direction.
            /// </summary>
            /// <param name="device">The device.</param>
            /// <param name="size">The size.</param>
            /// <param name="uScale">Scale U coordinates between 0 and the values of this parameter.</param>
            /// <param name="vScale">Scale V coordinates 0 and the values of this parameter.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is false.</param>
            /// <returns>A cube.</returns>
            public static GeometricPrimitive New(GraphicsDevice device, Vector3 size, float uScale = 1.0f, float vScale = 1.0f, bool toLeftHanded = false)
            {
                // Create the primitive object.
                return new GeometricPrimitive(device, New(size, uScale, vScale, toLeftHanded));
            }

            /// <summary>
            /// Creates a cube with six faces each one pointing in a different direction.
            /// </summary>
            /// <param name="size">The size.</param>
            /// <param name="uScale">Scale U coordinates between 0 and the values of this parameter.</param>
            /// <param name="vScale">Scale V coordinates 0 and the values of this parameter.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is false.</param>
            /// <returns>A cube.</returns>
            public static GeometricMeshData<VertexPositionNormalTexture> New(float size = 1.0f, float uScale = 1.0f, float vScale = 1.0f, bool toLeftHanded = false)
            {
                return New(new Vector3(size), uScale, vScale, toLeftHanded);
            }

            /// <summary>
            /// Creates a cube with six faces each one pointing in a different direction.
            /// </summary>
            /// <param name="size">The size.</param>
            /// <param name="uScale">Scale U coordinates between 0 and the values of this parameter.</param>
            /// <param name="vScale">Scale V coordinates 0 and the values of this parameter.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is false.</param>
            /// <returns>A cube.</returns>
            public static GeometricMeshData<VertexPositionNormalTexture> New(Vector3 size, float uScale = 1.0f, float vScale = 1.0f, bool toLeftHanded = false)
            {
                var vertices = new VertexPositionNormalTexture[CubeFaceCount * 4];
                var indices = new int[CubeFaceCount * 6];

                var texCoords = new Vector2[4];
                for (var i = 0; i < 4; i++)
                {
                    texCoords[i] = TextureCoordinates[i] * new Vector2(uScale, vScale);
                }

                size /= 2.0f;

                int vertexCount = 0;
                int indexCount = 0;
                // Create each face in turn.
                for (int i = 0; i < CubeFaceCount; i++)
                {
                    Vector3 normal = FaceNormals[i];

                    // Get two vectors perpendicular both to the face normal and to each other.
                    Vector3 basis = (i >= 4) ? Vector3.UnitZ : Vector3.UnitY;

                    Vector3 side1;
                    Vector3.Cross(ref normal, ref basis, out side1);

                    Vector3 side2;
                    Vector3.Cross(ref normal, ref side1, out side2);

                    // Six indices (two triangles) per face.
                    int vbase = i * 4;
                    indices[indexCount++] = (vbase + 0);
                    indices[indexCount++] = (vbase + 1);
                    indices[indexCount++] = (vbase + 2);

                    indices[indexCount++] = (vbase + 0);
                    indices[indexCount++] = (vbase + 2);
                    indices[indexCount++] = (vbase + 3);

                    // Four vertices per face.
                    vertices[vertexCount++] = new VertexPositionNormalTexture((normal - side1 - side2) * size, normal, texCoords[0]);
                    vertices[vertexCount++] = new VertexPositionNormalTexture((normal - side1 + side2) * size, normal, texCoords[1]);
                    vertices[vertexCount++] = new VertexPositionNormalTexture((normal + side1 + side2) * size, normal, texCoords[2]);
                    vertices[vertexCount++] = new VertexPositionNormalTexture((normal + side1 - side2) * size, normal, texCoords[3]);
                }

                // Create the primitive object.
                return new GeometricMeshData<VertexPositionNormalTexture>(vertices, indices, toLeftHanded) { Name = "Cube" };
            }
        }
    }
}
