// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Graphics
{
    /// <summary>
    /// Used by <see cref="Image"/> to provide a selector to a <see cref="PixelBuffer"/>.
    /// </summary>
    public sealed class PixelBufferArray
    {
        private readonly Image image;

        internal PixelBufferArray(Image image)
        {
            this.image = image;
        }

        /// <summary>
        /// Gets the pixel buffer.
        /// </summary>
        /// <returns>A <see cref="PixelBuffer"/>.</returns>
        public PixelBuffer this[int bufferIndex]
        {
            get
            {
                return this.image.PixelBuffers[bufferIndex];
            }
        }

        /// <summary>
        /// Gets the total number of pixel buffers.
        /// </summary>
        /// <returns>The total number of pixel buffers.</returns>
        public int Count { get { return this.image.PixelBuffers.Length; } }

        /// <summary>
        /// Gets the pixel buffer for the specified array/z slice and mipmap level.
        /// </summary>
        /// <param name="arrayOrDepthSlice">For 3D image, the parameter is the Z slice, otherwise it is an index into the texture array.</param>
        /// <param name="mipIndex">The mip map slice index.</param>
        /// <returns>A <see cref="PixelBuffer"/>.</returns>
        public PixelBuffer this[int arrayOrDepthSlice, int mipIndex]
        {
            get
            {
                return this.image.GetPixelBuffer(arrayOrDepthSlice, mipIndex);
            }
        }

        /// <summary>
        /// Gets the pixel buffer for the specified array/z slice and mipmap level.
        /// </summary>
        /// <param name="arrayIndex">Index into the texture array. Must be set to 0 for 3D images.</param>
        /// <param name="zIndex">Z index for 3D image. Must be set to 0 for all 1D/2D images.</param>
        /// <param name="mipIndex">The mip map slice index.</param>
        /// <returns>A <see cref="PixelBuffer"/>.</returns>
        public PixelBuffer this[int arrayIndex, int zIndex, int mipIndex]
        {
            get
            {
                return this.image.GetPixelBuffer(arrayIndex, zIndex, mipIndex);
            }
        }
    }
}
