// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Rendering.Voxels
{
    static class VoxelUtils
    {
        public static bool DisposeBufferBySpecs(Buffer buffer, int count)
        {
            if (buffer is null || buffer.ElementCount != count)
            {
                if (buffer != null)
                    buffer.Dispose();

                return true;
            }
            return false;
        }

        public static bool TextureDimensionsEqual(Texture tex, Vector3 dim)
        {
            return (tex.Width == dim.X &&
                    tex.Height == dim.Y &&
                    tex.Depth == dim.Z);
        }

        public static bool DisposeTextureBySpecs(Texture tex, Vector3 dim, PixelFormat pixelFormat)
        {
            if (tex is null ||
                !TextureDimensionsEqual(tex, dim) ||
                tex.Format != pixelFormat)
            {
                if (tex != null)
                    tex.Dispose();

                return true;
            }
            return false;
        }

        public static bool DisposeTextureBySpecs(Texture tex, Vector3 dim, PixelFormat pixelFormat, MultisampleCount samples)
        {
            if (tex is null ||
                !TextureDimensionsEqual(tex, dim) ||
                tex.Format != pixelFormat ||
                tex.MultisampleCount != samples)
            {
                if (tex != null)
                    tex.Dispose();

                return true;
            }
            return false;
        }
    }
}
