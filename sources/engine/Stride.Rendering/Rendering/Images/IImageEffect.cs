// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Graphics;

namespace Stride.Rendering.Images
{
    /// <summary>
    ///   Defines the methods for rendering image processing effects.
    /// </summary>
    public interface IImageEffect : IGraphicsRenderer
    {
        /// <summary>
        ///   Sets an input texture.
        /// </summary>
        /// <param name="slot">The input slot on which to set the texture.</param>
        /// <param name="texture">The texture.</param>
        void SetInput(int slot, Texture texture);

        /// <summary>
        ///   Sets the viewport.
        /// </summary>
        /// <param name="viewport">The viewport. Specify <c>null</c> to not use a custom viewport.</param>
        void SetViewport(Viewport? viewport);

        /// <summary>
        ///   Sets the texture to use as output.
        /// </summary>
        /// <param name="texture">The texture to use as output.</param>
        /// <exception cref="ArgumentNullException"><paramref name="texture"/> is a <c>null</c> reference.</exception>
        void SetOutput(Texture texture);

        /// <summary>
        ///   Sets the textures to use as output.
        /// </summary>
        /// <param name="textures">The textures to use as output.</param>
        void SetOutput(params Texture[] textures);
    }
}
