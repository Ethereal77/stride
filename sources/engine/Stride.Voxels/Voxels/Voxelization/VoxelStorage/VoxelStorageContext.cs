// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Rendering.Voxels
{
    public class VoxelStorageContext
    {
        public GraphicsDevice device;

        public Vector3 Extents;
        public Vector3 Translation;
        public Vector3 VoxelSpaceTranslation;

        public float VoxelSize;

        public Matrix Matrix;


        public float RealVoxelSize()
        {
            Int3 resolution = Resolution();
            return Extents.X / resolution.X;
        }

        public Int3 Resolution()
        {
            var resolution = Extents / VoxelSize;

            // Calculate closest power of 2 on each axis
            resolution.X = (float) Math.Pow(2, Math.Round(Math.Log(resolution.X, 2)));
            resolution.Y = (float) Math.Pow(2, Math.Round(Math.Log(resolution.Y, 2)));
            resolution.Z = (float) Math.Pow(2, Math.Round(Math.Log(resolution.Z, 2)));

            return new Int3((int) resolution.X, (int) resolution.Y, (int) resolution.Z);
        }
    }
}
