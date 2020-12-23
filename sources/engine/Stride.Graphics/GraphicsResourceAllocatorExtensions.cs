// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Graphics
{
    /// <summary>
    ///   Defines extension methods for the <see cref="GraphicsResourceAllocator"/>.
    /// </summary>
    public static class GraphicsResourceAllocatorExtensions
    {
        /// <summary>
        ///   Gets a texture for the specified description.
        /// </summary>
        /// <param name="allocator">The graphics resources allocator.</param>
        /// <param name="description">The description.</param>
        /// <returns>A new instance of texture.</returns>
        public static Texture GetTemporaryTexture2D(this GraphicsResourceAllocator allocator, TextureDescription description)
        {
            return allocator.GetTemporaryTexture(description);
        }

        /// <summary>
        ///   Gets a texture for the specified description with a single mipmap.
        /// </summary>
        /// <param name="allocator">The graphics resources allocator.</param>
        /// <param name="width">The width of the render target.</param>
        /// <param name="height">The height of the render target.</param>
        /// <param name="format">The pixel format to use.</param>
        /// <param name="flags">
        ///   The texture flags (for unordered access, render targets, etc). Default value is <see cref="TextureFlags.RenderTarget"/>
        ///   and <see cref="TextureFlags.ShaderResource"/>.
        /// </param>
        /// <param name="arraySize">Size of the texture array. Default value is 1.</param>
        /// <returns>A new instance of texture.</returns>
        public static Texture GetTemporaryTexture2D(this GraphicsResourceAllocator allocator, int width, int height, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return allocator.GetTemporaryTexture(
                TextureDescription.New2D(width, height, mipCount: 1, format, flags, arraySize));
        }

        /// <summary>
        ///   Gets a texture for the specified description.
        /// </summary>
        /// <param name="allocator">The graphics resources allocator.</param>
        /// <param name="width">The width of the render target.</param>
        /// <param name="height">The height of the render target.</param>
        /// <param name="format">The pixel format to use.</param>
        /// <param name="mipCount">
        ///   Number of mipmaps. Set to <c>true</c> to have all mipmaps, to an integer greater than 1 for a particular mipmap count.
        /// </param>
        /// <param name="flags">
        ///   The texture flags (for unordered access, render targets, etc). Default value is <see cref="TextureFlags.RenderTarget"/>
        ///   and <see cref="TextureFlags.ShaderResource"/>.
        /// </param>
        /// <param name="arraySize">Size of the texture array. Default value is 1.</param>
        /// <returns>A new instance of texture.</returns>
        public static Texture GetTemporaryTexture2D(this GraphicsResourceAllocator allocator, int width, int height, PixelFormat format, MipMapCount mipCount, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return allocator.GetTemporaryTexture(
                TextureDescription.New2D(width, height, mipCount, format, flags, arraySize));
        }
    }
}
