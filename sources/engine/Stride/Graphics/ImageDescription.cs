// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Runtime.InteropServices;

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    ///   Describes the characteristics of an <see cref="Image"/>.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageDescription : IEquatable<ImageDescription>
    {
        /// <summary>
        ///   Dimension of the image.
        /// </summary>
        public TextureDimension Dimension;

        /// <summary>
        ///   Texture width, in pixels.
        ///   The range is from 1 to 16384 or whichever the maximum allowed width for an 2D image is supported by the GPU and graphics API.
        ///   Also, the range is actually constrained by the feature level at which you create the rendering device.
        /// </summary>
        /// <remarks>
        ///   This field is valid for all textures: 1D, 2D, 3D and cube textures.
        /// </remarks>
        public int Width;

        /// <summary>
        ///   Texture height, in pixels.
        ///   The range is from 1 to 16384 or whichever the maximum allowed height for an 2D image is supported by the GPU and graphics API.
        ///   Also, the range is actually constrained by the feature level at which you create the rendering device.
        /// </summary>
        /// <remarks>
        ///   This field is valid only for textures: 2D, 3D and cube textures.
        /// </remarks>
        public int Height;

        /// <summary>
        ///   Texture depth, in pixels.
        ///   The range is from 1 to 2048 or whichever the maximum allowed depth for an 3D image is supported by the GPU and graphics API.
        ///   Also, the range is actually constrained by the feature level at which you create the rendering device.
        /// </summary>
        /// <remarks>
        ///   This field is only valid for 3D textures.
        /// </remarks>
        public int Depth;

        /// <summary>
        ///   Number of textures in the array.
        ///   The range is from 1 to 2048 or whichever the maximum allowed array size for an texture array is supported by the GPU and graphics API.
        ///   Also, the range is actually constrained by the feature level at which you create the rendering device.
        /// </summary>
        /// <remarks>
        ///   This field is only valid for textures: 1D, 2D and cube textures.
        /// </remarks>
        public int ArraySize;

        /// <summary>
        ///   The maximum number of mipmap levels in the texture.
        /// </summary>
        /// <remarks>
        ///   Use a value of 1 for a multisampled texture; or 0 to generate a full set of subtextures.
        /// </remarks>
        public int MipLevels;

        /// <summary>
        ///   Pixel format.
        /// </summary>
        public PixelFormat Format;


        public bool Equals(ImageDescription other)
        {
            return Dimension.Equals(other.Dimension) &&
                   Width == other.Width &&
                   Height == other.Height &&
                   Depth == other.Depth &&
                   ArraySize == other.ArraySize &&
                   MipLevels == other.MipLevels &&
                   Format.Equals(other.Format);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return obj is ImageDescription description && Equals(description);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Dimension.GetHashCode();
                hashCode = (hashCode * 397) ^ Width;
                hashCode = (hashCode * 397) ^ Height;
                hashCode = (hashCode * 397) ^ Depth;
                hashCode = (hashCode * 397) ^ ArraySize;
                hashCode = (hashCode * 397) ^ MipLevels;
                hashCode = (hashCode * 397) ^ Format.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(ImageDescription left, ImageDescription right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ImageDescription left, ImageDescription right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"Dimension: {Dimension}, Width: {Width}, Height: {Height}, Depth: {Depth}, Format: {Format}, ArraySize: {ArraySize}, MipLevels: {MipLevels}";
        }
    }
}
