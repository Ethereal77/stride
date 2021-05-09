// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Mathematics;
using Stride.Rendering;

namespace Stride.Graphics
{
    /// <summary>
    ///   Provides extensions for drawing with a <see cref="GraphicsDevice"/>
    /// </summary>
    public static class GraphicsDeviceExtensions
    {
        /// <summary>
        ///   Draws a fullscreen quad with the specified effect and parameters.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectInstance">The effect instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="effectInstance"/> is a <c>null</c> reference.</exception>
        public static void DrawQuad(this GraphicsContext graphicsContext, EffectInstance effectInstance)
        {
            if (effectInstance is null)
                throw new ArgumentNullException("effectInstance");

            // Draw a full screen quad
            graphicsContext.CommandList.GraphicsDevice.PrimitiveQuad.Draw(graphicsContext, effectInstance);
        }

        #region DrawQuad / DrawTexture Helpers

        /// <summary>
        ///   Draws a full screen quad. An <see cref="Effect"/> must be applied before calling this method.
        /// </summary>
        public static void DrawQuad(this CommandList commandList)
        {
            commandList.GraphicsDevice.PrimitiveQuad.Draw(commandList);
        }

        /// <summary>
        ///   Draws a fullscreen texture using a <see cref="SamplerStateFactory.LinearClamp"/> sampler.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use for drawing the texture.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="blendState">Blend state to use to draw the texture. If <c>null</c> is specified, no blending will be performed.</param>
        public static void DrawTexture(this GraphicsContext graphicsContext, Texture texture, BlendStateDescription? blendState = null)
        {
            graphicsContext.DrawTexture(texture, null, Color4.White, blendState);
        }

        /// <summary>
        ///   Draws a fullscreen texture using the specified sampler.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use for drawing the texture.</param>
        /// <param name="texture">The texture to draw..</param>
        /// <param name="sampler">The sampler state. If <c>null</c>, <see cref="SamplerStateFactory.LinearClamp"/> will be used by default.</param>
        /// <param name="blendState">Blend state to use to draw the texture. If <c>null</c> is specified, no blending will be performed.</param>
        public static void DrawTexture(this GraphicsContext graphicsContext, Texture texture, SamplerState sampler, BlendStateDescription? blendState = null)
        {
            graphicsContext.DrawTexture(texture, sampler, Color4.White, blendState);
        }

        /// <summary>
        ///   Draws a fullscreen texture using a <see cref="SamplerStateFactory.LinearClamp"/> sampler
        ///   and the texture color multiplied by a custom color.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use for drawing the texture.</param>
        /// <param name="texture">The texture. Expecting an instance of <see cref="Texture"/>.</param>
        /// <param name="color">The color tint. Specify <see cref="Color4.White"/> for no tinting.</param>
        /// <param name="blendState">Blend state to use to draw the texture. If <c>null</c> is specified, no blending will be performed.</param>
        public static void DrawTexture(this GraphicsContext graphicsContext, Texture texture, Color4 color, BlendStateDescription? blendState = null)
        {
            graphicsContext.DrawTexture(texture, null, color, blendState);
        }

        /// <summary>
        /// Draws a fullscreen texture using the specified sampler
        /// and the texture color multiplied by a custom color. See <see cref="Draw+a+texture"/> to learn how to use it.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use for drawing the texture.</param>
        /// <param name="texture">The texture. Expecting an instance of <see cref="Texture"/>.</param>
        /// <param name="sampler">The sampler state. If <c>null</c>, <see cref="SamplerStateFactory.LinearClamp"/> will be used by default.</param>
        /// <param name="color">The color tint. Specify <see cref="Color4.White"/> for no tinting.</param>
        /// <param name="blendState">Blend state to use to draw the texture. If <c>null</c> is specified, no blending will be performed.</param>
        public static void DrawTexture(this GraphicsContext graphicsContext, Texture texture, SamplerState sampler, Color4 color, BlendStateDescription? blendState = null)
        {
            graphicsContext.CommandList.GraphicsDevice.PrimitiveQuad.Draw(graphicsContext, texture, sampler, color, blendState);
        }

        #endregion

        #region Shared white texture

        public static Texture GetSharedWhiteTexture(this GraphicsDevice device)
        {
            return device.GetOrCreateSharedData("WhiteTexture", CreateWhiteTexture);
        }

        private static Texture CreateWhiteTexture(GraphicsDevice device)
        {
            const int Size = 2;
            var whiteData = new Color[Size * Size];
            for (int i = 0; i < Size * Size; i++)
                whiteData[i] = Color.White;

            return Texture.New2D(device, Size, Size, PixelFormat.R8G8B8A8_UNorm, whiteData);
        }

        #endregion
    }
}
