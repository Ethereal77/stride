// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core;

namespace Stride.Graphics
{
    public partial class Texture 
    {
        /// <summary>
        /// Creates a new 2D <see cref="Texture" /> with a single mipmap.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of 2D <see cref="Texture" /> class.</returns>
        public static Texture New2D(GraphicsDevice device, int width, int height, PixelFormat format, TextureFlags textureFlags = TextureFlags.ShaderResource, int arraySize = 1, GraphicsResourceUsage usage = GraphicsResourceUsage.Default, TextureOptions options = TextureOptions.None)
        {
            return New2D(device, width, height, false, format, textureFlags, arraySize, usage, options);
        }

        /// <summary>
        /// Creates a new 2D <see cref="Texture" />.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of 2D <see cref="Texture" /> class.</returns>
        public static Texture New2D(GraphicsDevice device, int width, int height, MipMapCount mipCount, PixelFormat format, TextureFlags textureFlags = TextureFlags.ShaderResource, int arraySize = 1, GraphicsResourceUsage usage = GraphicsResourceUsage.Default, TextureOptions options = TextureOptions.None)
        {
            return new Texture(device).InitializeFrom(TextureDescription.New2D(width, height, mipCount, format, textureFlags, arraySize, usage, MultisampleCount.None, options));
        }

        /// <summary>
        /// Creates a new 2D <see cref="Texture" /> with a single level of mipmap.
        /// </summary>
        /// <typeparam name="T">Type of the pixel data to upload to the texture.</typeparam>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureData">The texture data for a single mipmap and a single array slice. See remarks</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>A new instance of 2D <see cref="Texture" /> class.</returns>
        /// <remarks>
        /// Each value in textureData is a pixel in the destination texture.
        /// </remarks>
        public static unsafe Texture New2D<T>(GraphicsDevice device, int width, int height, PixelFormat format, T[] textureData, TextureFlags textureFlags = TextureFlags.ShaderResource, GraphicsResourceUsage usage = GraphicsResourceUsage.Immutable, TextureOptions options = TextureOptions.None) where T : struct
        {
            return New2D(device, width, height, 1, format, new[] { GetDataBox(format, width, height, 1, textureData, (IntPtr)Interop.Fixed(textureData)) }, textureFlags, 1, usage, MultisampleCount.None, options);
        }

        /// <summary>
        /// Creates a new 2D <see cref="Texture" />.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice" />.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int &gt;=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureData">Texture datas through an array of <see cref="DataBox" /></param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="multisampleCount">The multisample count.</param>
        /// <param name="options">The options, e.g. sharing</param>
        /// <returns>
        /// A new instance of 2D <see cref="Texture" /> class.
        /// </returns>
        public static Texture New2D(
            GraphicsDevice device,
            int width,
            int height,
            MipMapCount mipCount,
            PixelFormat format,
            DataBox[] textureData,
            TextureFlags textureFlags = TextureFlags.ShaderResource,
            int arraySize = 1,
            GraphicsResourceUsage usage = GraphicsResourceUsage.Default,
            MultisampleCount multisampleCount = MultisampleCount.None,
            TextureOptions options = TextureOptions.None)
        {
            return new Texture(device).InitializeFrom(TextureDescription.New2D(width, height, mipCount, format, textureFlags, arraySize, usage, multisampleCount, options), textureData);
        }
    }
}
