// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

// -----------------------------------------------------------------------------
// The following code is a port of DirectXTk http://directxtk.codeplex.com
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Stride.Core;
using Stride.Core.Mathematics;

namespace Stride.Graphics.GeometricPrimitives
{
    public partial class GeometricPrimitive
    {
        /// <summary>
        /// A Cylinder primitive.
        /// </summary>
        public static class Cylinder
        {
            // Helper computes a point on a unit circle, aligned to the x/z plane and centered on the origin.
            private static Vector3 GetCircleVector(int i, int tessellation)
            {
                var angle = (float)(i * 2.0 * Math.PI / tessellation);
                var dx = (float)Math.Sin(angle);
                var dz = (float)Math.Cos(angle);

                return new Vector3(dx, 0, dz);
            }

            // Helper creates a triangle fan to close the end of a cylinder.
            private static void CreateCylinderCap(List<VertexPositionNormalTexture> vertices, List<int> indices, int tessellation, float height, float radius, float uScale, float vScale, bool isTop)
            {
                // Create cap indices.
                for (int i = 0; i < tessellation - 2; i++)
                {
                    int i1 = (i + 1) % tessellation;
                    int i2 = (i + 2) % tessellation;

                    if (isTop)
                    {
                        Utilities.Swap(ref i1, ref i2);
                    }

                    int vbase = vertices.Count;
                    indices.Add(vbase);
                    indices.Add(vbase + i1);
                    indices.Add(vbase + i2);
                }

                // Which end of the cylinder is this?
                var normal = Vector3.UnitY;
                var textureScale = new Vector2(-0.5f);

                if (!isTop)
                {
                    normal = -normal;
                    textureScale.X = -textureScale.X;
                }

                // Create cap vertices.
                for (int i = 0; i < tessellation; i++)
                {
                    var circleVector = GetCircleVector(i, tessellation);
                    var position = (circleVector * radius) + (normal * height);
                    var textureCoordinate = new Vector2(uScale * (circleVector.X * textureScale.X + 0.5f), vScale * (circleVector.Z * textureScale.Y + 0.5f));

                    vertices.Add(new VertexPositionNormalTexture(position, normal, textureCoordinate));
                }
            }

            /// <summary>
            /// Creates a cylinder primitive.
            /// </summary>
            /// <param name="device">The device.</param>
            /// <param name="height">The height.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="tessellation">The tessellation.</param>
            /// <param name="uScale">Scale U coordinates between 0 and the values of this parameter.</param>
            /// <param name="vScale">Scale V coordinates 0 and the values of this parameter.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is false.</param>
            /// <returns>A cylinder primitive.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">tessellation;tessellation must be &gt;= 3</exception>
            public static GeometricPrimitive New(GraphicsDevice device, float height = 1.0f, float radius = 0.5f, int tessellation = 32, float uScale = 1.0f, float vScale = 1.0f, bool toLeftHanded = false)
            {
                // Create the primitive object.
                return new GeometricPrimitive(device, New(height, radius, tessellation, uScale, vScale, toLeftHanded));
            }

            /// <summary>
            /// Creates a cylinder primitive.
            /// </summary>
            /// <param name="height">The height.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="tessellation">The tessellation.</param>
            /// <param name="uScale">Scale U coordinates between 0 and the values of this parameter.</param>
            /// <param name="vScale">Scale V coordinates 0 and the values of this parameter.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is false.</param>
            /// <returns>A cylinder primitive.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">tessellation;tessellation must be &gt;= 3</exception>
            public static GeometricMeshData<VertexPositionNormalTexture> New(float height = 1.0f, float radius = 0.5f, int tessellation = 32, float uScale = 1.0f, float vScale = 1.0f, bool toLeftHanded = false)
            {
                if (tessellation < 3)
                    tessellation = 3;

                var vertices = new List<VertexPositionNormalTexture>();
                var indices = new List<int>();

                height /= 2;

                var topOffset = Vector3.UnitY * height;

                int stride = tessellation + 1;

                // Create a ring of triangles around the outside of the cylinder.
                for (int i = 0; i <= tessellation; i++)
                {
                    var normal = GetCircleVector(i, tessellation);

                    var sideOffset = normal * radius;

                    var textureCoordinate = new Vector2((float)i / tessellation, 0);
                    
                    vertices.Add(new VertexPositionNormalTexture(sideOffset + topOffset, normal, textureCoordinate * new Vector2(uScale, vScale)));
                    vertices.Add(new VertexPositionNormalTexture(sideOffset - topOffset, normal, (textureCoordinate + Vector2.UnitY) * new Vector2(uScale, vScale)));

                    indices.Add(i * 2);
                    indices.Add((i * 2 + 2) % (stride * 2));
                    indices.Add(i * 2 + 1);

                    indices.Add(i * 2 + 1);
                    indices.Add((i * 2 + 2) % (stride * 2));
                    indices.Add((i * 2 + 3) % (stride * 2));
                }

                // Create flat triangle fan caps to seal the top and bottom.
                CreateCylinderCap(vertices, indices, tessellation, height, radius, uScale, vScale, true);
                CreateCylinderCap(vertices, indices, tessellation, height, radius, uScale, vScale, false);

                // Create the primitive object.
                return new GeometricMeshData<VertexPositionNormalTexture>(vertices.ToArray(), indices.ToArray(), toLeftHanded) { Name = "Cylinder" };
            }
        }
    }
}
