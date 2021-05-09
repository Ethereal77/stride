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

using Stride.Core.Mathematics;

namespace Stride.Graphics.GeometricPrimitives
{
    public partial class GeometricPrimitive
    {
        /// <summary>
        /// A Torus primitive.
        /// </summary>
        public static class Torus
        {
            /// <summary>
            /// Creates a torus primitive.
            /// </summary>
            /// <param name="device">The device.</param>
            /// <param name="majorRadius">The majorRadius.</param>
            /// <param name="minorRadius">The minorRadius.</param>
            /// <param name="tessellation">The tessellation.</param>
            /// <param name="uScale">Scale U coordinates between 0 and the values of this parameter.</param>
            /// <param name="vScale">Scale V coordinates 0 and the values of this parameter.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is false.</param>
            /// <returns>A Torus primitive.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">tessellation;tessellation parameter out of range</exception>
            public static GeometricPrimitive New(GraphicsDevice device, float majorRadius = 0.5f, float minorRadius = 0.16666f, int tessellation = 32, float uScale = 1.0f, float vScale = 1.0f, bool toLeftHanded = false)
            {
                return new GeometricPrimitive(device, New(majorRadius, minorRadius, tessellation, uScale, vScale, toLeftHanded));
            }

            /// <summary>
            /// Creates a torus primitive.
            /// </summary>
            /// <param name="majorRadius">The major radius of the torus.</param>
            /// <param name="minorRadius">The minor radius of the torus.</param>
            /// <param name="tessellation">The tessellation.</param>
            /// <param name="uScale">Scale U coordinates between 0 and the values of this parameter.</param>
            /// <param name="vScale">Scale V coordinates 0 and the values of this parameter.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is false.</param>
            /// <returns>A Torus primitive.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">tessellation;tessellation parameter out of range</exception>
            public static GeometricMeshData<VertexPositionNormalTexture> New(float majorRadius = 0.5f, float minorRadius = 0.16666f, int tessellation = 32, float uScale = 1.0f, float vScale = 1.0f, bool toLeftHanded = false)
            {
                var vertices = new List<VertexPositionNormalTexture>();
                var indices = new List<int>();

                if (tessellation < 3)
                    tessellation = 3;

                int stride = tessellation + 1;

                var texFactor = new Vector2(uScale, vScale);

                // First we loop around the main ring of the torus.
                for (int i = 0; i <= tessellation; i++)
                {
                    float u = (float)i / tessellation;

                    float outerAngle = i * MathUtil.TwoPi / tessellation - MathUtil.PiOverTwo;

                    // Create a transform matrix that will align geometry to
                    // slice perpendicularly though the current ring position.
                    var transform = Matrix.Translation(majorRadius, 0, 0) * Matrix.RotationY(outerAngle);

                    // Now we loop along the other axis, around the side of the tube.
                    for (int j = 0; j <= tessellation; j++)
                    {
                        float v = 1 - (float)j / tessellation;

                        float innerAngle = j * MathUtil.TwoPi / tessellation + MathUtil.Pi;
                        float dx = (float)Math.Cos(innerAngle), dy = (float)Math.Sin(innerAngle);

                        // Create a vertex.
                        var normal = new Vector3(dx, dy, 0);
                        var position = normal * minorRadius;
                        var textureCoordinate = new Vector2(u, v);

                        Vector3.TransformCoordinate(ref position, ref transform, out position);
                        Vector3.TransformNormal(ref normal, ref transform, out normal);

                        vertices.Add(new VertexPositionNormalTexture(position, normal, textureCoordinate * texFactor));

                        // And create indices for two triangles.
                        int nextI = (i + 1) % stride;
                        int nextJ = (j + 1) % stride;

                        indices.Add(i * stride + j);
                        indices.Add(i * stride + nextJ);
                        indices.Add(nextI * stride + j);

                        indices.Add(i * stride + nextJ);
                        indices.Add(nextI * stride + nextJ);
                        indices.Add(nextI * stride + j);
                    }
                }

                // Create the primitive object.
                return new GeometricMeshData<VertexPositionNormalTexture>(vertices.ToArray(), indices.ToArray(), toLeftHanded) { Name = "Torus" };
            }
        }
    }
}
