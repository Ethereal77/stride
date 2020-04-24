// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Linq;

using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Physics
{
    public static class HeightmapExtensions
    {
        public static bool IsValid([NotNull] this Heightmap heightmap)
        {
            if (heightmap is null)
                throw new ArgumentNullException(nameof(heightmap));

            var length = heightmap.Size.X * heightmap.Size.Y;

            bool hasValidHeights = false;
            switch (heightmap.HeightType)
            {
                case HeightfieldTypes.Float when heightmap.Floats != null && heightmap.Floats.Length == length:
                    hasValidHeights = true;
                    break;

                case HeightfieldTypes.Short when heightmap.Shorts != null && heightmap.Shorts.Length == length:
                    hasValidHeights = true;
                    break;

                case HeightfieldTypes.Byte when heightmap.Bytes != null && heightmap.Bytes.Length == length:
                    hasValidHeights = true;
                    break;
            }

            return HeightmapUtils.CheckHeightParameters(heightmap.Size, heightmap.HeightType, heightmap.HeightRange, heightmap.HeightScale, false) &&
                   hasValidHeights;
        }

        public static Texture CreateTexture([NotNull] this Heightmap heightmap, GraphicsDevice device)
        {
            if (heightmap is null)
                throw new ArgumentNullException(nameof(heightmap));

            if (device is null || !heightmap.IsValid())
                return null;

            var min = heightmap.HeightRange.X / heightmap.HeightScale;
            var max = heightmap.HeightRange.Y / heightmap.HeightScale;

            switch (heightmap.HeightType)
            {
                case HeightfieldTypes.Float:
                    return Texture.New2D(device, heightmap.Size.X, heightmap.Size.Y, PixelFormat.R8_UNorm, HeightmapUtils.ConvertToByteHeights(heightmap.Floats, min, max));

                case HeightfieldTypes.Short:
                    return Texture.New2D(device, heightmap.Size.X, heightmap.Size.Y, PixelFormat.R8_UNorm,
                        heightmap.Shorts.Select((h) => (byte)MathUtil.Clamp(MathUtil.Lerp(byte.MinValue, byte.MaxValue, MathUtil.InverseLerp(min, max, h)), byte.MinValue, byte.MaxValue)).ToArray());

                case HeightfieldTypes.Byte:
                    return Texture.New2D(device, heightmap.Size.X, heightmap.Size.Y, PixelFormat.R8_UNorm,
                        heightmap.Bytes.Select((h) => (byte)MathUtil.Clamp(MathUtil.Lerp(byte.MinValue, byte.MaxValue, MathUtil.InverseLerp(min, max, h)), byte.MinValue, byte.MaxValue)).ToArray());

                default:
                    return null;
            }
        }
    }
}
