// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Runtime.InteropServices;

using Xenko.Core;

namespace Xenko.Graphics
{
    /// <summary>
    /// A description for <see cref="Image"/>.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageDescription : IEquatable<ImageDescription>
    {
        /// <summary>
        /// The dimension of a texture.
        /// </summary>
        public TextureDimension Dimension;

        /// <summary>
        /// <dd> <p>Texture width (in texels). The  range is from 1 to <see cref="SharpDX.Direct3D11.Resource.MaximumTexture1DSize"/> (16384). However, the range is actually constrained by the feature level at which you create the rendering device. For more information about restrictions, see Remarks.</p> </dd>
        /// </summary>
        /// <remarks>
        /// This field is valid for all textures: <see cref="Texture1D"/>, <see cref="Texture2D"/>, <see cref="Texture3D"/> and <see cref="TextureCube"/>.
        /// </remarks>
        public int Width;

        /// <summary>
        /// <dd> <p>Texture height (in texels). The  range is from 1 to <see cref="SharpDX.Direct3D11.Resource.MaximumTexture3DSize"/> (2048). However, the range is actually constrained by the feature level at which you create the rendering device. For more information about restrictions, see Remarks.</p> </dd>
        /// </summary>
        /// <remarks>
        /// This field is only valid for <see cref="Texture2D"/>, <see cref="Texture3D"/> and <see cref="TextureCube"/>.
        /// </remarks>
        public int Height;

        /// <summary>
        /// <dd> <p>Texture depth (in texels). The  range is from 1 to <see cref="SharpDX.Direct3D11.Resource.MaximumTexture3DSize"/> (2048). However, the range is actually constrained by the feature level at which you create the rendering device. For more information about restrictions, see Remarks.</p> </dd>
        /// </summary>
        /// <remarks>
        /// This field is only valid for <see cref="Texture3D"/>.
        /// </remarks>
        public int Depth;

        /// <summary>
        /// <dd> <p>Number of textures in the array. The  range is from 1 to <see cref="SharpDX.Direct3D11.Resource.MaximumTexture1DArraySize"/> (2048). However, the range is actually constrained by the feature level at which you create the rendering device. For more information about restrictions, see Remarks.</p> </dd>
        /// </summary>
        /// <remarks>
        /// This field is only valid for <see cref="Texture1D"/>, <see cref="Texture2D"/> and <see cref="TextureCube"/>
        /// </remarks>
        /// <remarks>
        /// This field is only valid for textures: <see cref="Texture1D"/>, <see cref="Texture2D"/> and <see cref="TextureCube"/>.
        /// </remarks>
        public int ArraySize;

        /// <summary>
        /// <dd> <p>The maximum number of mipmap levels in the texture. See the remarks in <strong><see cref="SharpDX.Direct3D11.ShaderResourceViewDescription.Texture1DResource"/></strong>. Use 1 for a multisampled texture; or 0 to generate a full set of subtextures.</p> </dd>
        /// </summary>
        public int MipLevels;

        /// <summary>
        /// <dd> <p>Texture format (see <strong><see cref="SharpDX.DXGI.Format"/></strong>).</p> </dd>
        /// </summary>
        public PixelFormat Format;

        public bool Equals(ImageDescription other)
        {
            return Dimension.Equals(other.Dimension) && Width == other.Width && Height == other.Height && Depth == other.Depth && ArraySize == other.ArraySize && MipLevels == other.MipLevels && Format.Equals(other.Format);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ImageDescription && Equals((ImageDescription)obj);
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
            return string.Format("Dimension: {0}, Width: {1}, Height: {2}, Depth: {3}, Format: {4}, ArraySize: {5}, MipLevels: {6}", Dimension, Width, Height, Depth, Format, ArraySize, MipLevels);
        }
    }
}
