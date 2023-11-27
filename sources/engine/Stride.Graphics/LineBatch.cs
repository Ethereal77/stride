// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Stride.Core.Mathematics;
using Stride.Rendering;

namespace Stride.Graphics
{
    /// <summary>
    ///   Renders a group of lines.
    /// </summary>
    public partial class LineBatch : BatchBase<LineBatch.LineDrawInfo>
    {
        /// <summary>
        ///   Contains the data needed to draw a line.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct LineDrawInfo
        {
            public Vector2 Source;
            public Vector2 Destination;
            public float Depth;
            public Color4 ColorSource;
            public Color4 ColorDestination;
        }


        private static ReadOnlySpan<Vector2> CornerOffsets => new Vector2[] { Vector2.Zero, Vector2.UnitX, Vector2.One, Vector2.UnitY };

        private Matrix userViewMatrix;
        private Matrix userProjectionMatrix;

        private Matrix defaultProjectionMatrix;

        public EffectInstance LineEffect { get; }

        /// <summary>
        ///   Gets or sets the default depth value used by the <see cref="LineBatch"/> when the <see cref="VirtualResolution"/> is not set.
        /// </summary>
        /// <remarks>
        ///   More precisely, this value represents the length "farPlane - nearPlane" used by the default projection matrix.
        /// </remarks>
        public float DefaultDepth { get; set; } = 200;

        /// <summary>
        ///   Gets or sets the virtual resolution used for this <see cref="LineBatch"/>
        /// </summary>
        public Vector3? VirtualResolution { get; set; }


        /// <summary>
        ///   Initializes a new instance of the <see cref="LineBatch" /> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="bufferElementCount">The maximum number of elements that can be batched in one time.</param>
        /// <param name="batchCapacity">The batch capacity default to 64.</param>
        public LineBatch(GraphicsDevice graphicsDevice, int bufferElementCount = 1024, int batchCapacity = 64)

            : base(graphicsDevice, Bytecode, BytecodeSRgb,
                   StaticQuadBufferInfo.CreateQuadBufferInfo("LineBatch.VertexIndexBuffer", cycle: true, bufferElementCount, batchCapacity),
                   VertexPositionColorTextureSwizzle.Layout)
        {
        }

        #region Projection

        /// <summary>
        ///   Calculates the default projection matrix for the provided virtual resolution.
        /// </summary>
        /// <param name="virtualResolution">The virtual resolution of the viewport to render to.</param>
        /// <returns>The default projection matrix for the provided virtual resolution.</returns>
        /// <remarks>
        ///   The line batch default projection is an orthogonal matrix such that <c>(0,0)</c> is the Top / Left corner of
        ///   the screen and <c>(VirtualResolution.X, VirtualResolution.Y)</c> is the Bottom / Right corner of the screen.
        /// </remarks>
        public static Matrix CalculateDefaultProjection(in Vector3 virtualResolution)
        {
            CalculateDefaultProjection(virtualResolution, out var matrix);

            return matrix;
        }

        /// <summary>
        ///   Calculates the default projection matrix for the provided virtual resolution.
        /// </summary>
        /// <param name="virtualResolution">The virtual resolution of the viewport to render to.</param>
        /// <param name="projection">When this method completes, contains the calculated projection matrix.</param>
        /// <remarks>
        ///   The line batch default projection is an orthogonal matrix such that <c>(0,0)</c> is the Top / Left corner of
        ///   the screen and <c>(VirtualResolution.X, VirtualResolution.Y)</c> is the Bottom / Right corner of the screen.
        /// </remarks>
        public static void CalculateDefaultProjection(in Vector3 virtualResolution, out Matrix projection)
        {
            var xRatio = 1 / virtualResolution.X;
            var yRatio = -1 / virtualResolution.Y;
            var zRatio = -1 / virtualResolution.Z;

            projection = new Matrix { M11 = 2 * xRatio, M22 = 2 * yRatio, M33 = zRatio, M44 = 1, M41 = -1, M42 = 1, M43 = 0.5f };
        }

        /// <summary>
        ///   Returns the current resolution (width, height, depth) that is used for calculating the projection.
        /// </summary>
        /// <param name="commandList">The command list from which to get the viewport.</param>
        /// <returns>
        ///   Returns the current set <see cref="VirtualResolution"/>. If not specified, returns the resolution of the
        ///   current <see cref="Viewport"/> set in the <see cref="CommandList"/>.
        /// </returns>
        private Vector3 GetCurrentResolution(CommandList commandList)
        {
            return VirtualResolution ?? new Vector3(commandList.Viewport.Width, commandList.Viewport.Height, DefaultDepth);
        }

        /// <summary>
        ///   Recalculates the projection matrix using the current viewport or the configured <see cref="VirtualResolution"/>.
        /// </summary>
        /// <param name="commandList">The command list from which to get the viewport.</param>
        private void UpdateDefaultProjectionMatrix(CommandList commandList)
        {
            var resolution = GetCurrentResolution(commandList);
            CalculateDefaultProjection(resolution, out defaultProjectionMatrix);
        }

        #endregion

        #region Begin

        /// <summary>
        ///   Begins a line batch operation using deferred sort and default state objects.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use.</param>
        /// <param name="sortMode">The sprite drawing order to use for the batch session.</param>
        /// <param name="effect">The effect to use for the batch session.</param>
        /// <remarks>
        ///    The default render state objects are:
        ///    <list type="bullet">
        ///      <item><see cref="BlendStates.AlphaBlend"/></item>
        ///      <item><see cref="SamplerStateFactory.LinearClamp"/></item>
        ///      <item><see cref="DepthStencilStates.Default"/></item>
        ///      <item><see cref="RasterizerStates.CullBack"/></item>
        ///    </list>
        /// </remarks>
        public void Begin(GraphicsContext graphicsContext, SpriteSortMode sortMode, EffectInstance effect)
        {
            UpdateDefaultProjectionMatrix(graphicsContext.CommandList);

            Begin(graphicsContext, viewMatrix: Matrix.Identity, defaultProjectionMatrix, sortMode,
                  blendState: null, samplerState: null, depthStencilState: null, rasterizerState: null, effect);
        }

        /// <summary>
        ///   Begins a line batch rendering using the specified sorting mode and blend state, sampler, depth stencil,
        ///   and rasterizer state objects, plus a custom effect.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use.</param>
        /// <param name="sortMode">The sprite drawing order to use for the batch session.</param>
        /// <param name="blendState">The blending state to use for the batch session, or <see langword="null"/> to use the default <see cref="BlendStates.AlphaBlend"/>.</param>
        /// <param name="samplerState">The sampling state to use for the batch session, or <see langword="null"/> to use the default <see cref="SamplerStateFactory.LinearClamp"/>.</param>
        /// <param name="depthStencilState">The depth stencil state to use for the batch session, or <see langword="null"/> to use the default <see cref="DepthStencilStates.Default"/>.</param>
        /// <param name="rasterizerState">The rasterizer state to use for the batch session, or <see langword="null"/> to use the default <see cref="RasterizerStates.CullBack"/>.</param>
        /// <param name="effect">The effect to use for the batch session, or <see langword="null"/> to use the default LineBatch Class shader.</param>
        /// <param name="stencilValue">The value of the stencil buffer to take as reference for the batch session.</param>
        /// <remarks>
        ///   Passing <see langword="null"/> for any of the state objects indicates the default default state object
        ///   should be used. Passing a <see langword="null"/> <paramref name="effect"/> selects the default LineBatch Class shader.
        /// </remarks>
        public void Begin(GraphicsContext graphicsContext, SpriteSortMode sortMode = SpriteSortMode.Deferred,
                          BlendStateDescription? blendState = null, SamplerState samplerState = null,
                          DepthStencilStateDescription? depthStencilState = null, RasterizerStateDescription? rasterizerState = null,
                          EffectInstance effect = null, int stencilValue = 0)
        {
            UpdateDefaultProjectionMatrix(graphicsContext.CommandList);

            Begin(graphicsContext, viewMatrix: Matrix.Identity, defaultProjectionMatrix, sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, stencilValue);
        }

        /// <summary>
        ///   Begins a line batch rendering using the specified sorting mode and blend state, sampler, depth stencil,
        ///   rasterizer state objects, plus a custom effect and a 2D transformation matrix.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use.</param>
        /// <param name="viewMatrix">The view matrix to use for the batch session.</param>
        /// <param name="sortMode">The sprite drawing order to use for the batch session.</param>
        /// <param name="blendState">The blending state to use for the batch session, or <see langword="null"/> to use the default <see cref="BlendStates.AlphaBlend"/>.</param>
        /// <param name="samplerState">The sampling state to use for the batch session, or <see langword="null"/> to use the default <see cref="SamplerStateFactory.LinearClamp"/>.</param>
        /// <param name="depthStencilState">The depth stencil state to use for the batch session, or <see langword="null"/> to use the default <see cref="DepthStencilStates.Default"/>.</param>
        /// <param name="rasterizerState">The rasterizer state to use for the batch session, or <see langword="null"/> to use the default <see cref="RasterizerStates.CullBack"/>.</param>
        /// <param name="effect">The effect to use for the batch session, or <see langword="null"/> to use the default LineBatch Class shader.</param>
        /// <param name="stencilValue">The value of the stencil buffer to take as reference for the batch session.</param>
        /// <remarks>
        ///   Passing <see langword="null"/> for any of the state objects indicates the default default state object
        ///   should be used. Passing a <see langword="null"/> <paramref name="effect"/> selects the default LineBatch Class shader.
        /// </remarks>
        public void Begin(GraphicsContext graphicsContext, in Matrix viewMatrix, SpriteSortMode sortMode = SpriteSortMode.Deferred,
                          BlendStateDescription? blendState = null, SamplerState samplerState = null,
                          DepthStencilStateDescription? depthStencilState = null, RasterizerStateDescription? rasterizerState = null,
                          EffectInstance effect = null, int stencilValue = 0)
        {
            UpdateDefaultProjectionMatrix(graphicsContext.CommandList);

            Begin(graphicsContext, viewMatrix, defaultProjectionMatrix, sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, stencilValue);
        }

        /// <summary>
        ///   Begins a line batch rendering using the specified sorting mode and blend state, sampler, depth stencil,
        ///   rasterizer state objects, plus a custom effect and a 2D transformation matrix.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use.</param>
        /// <param name="viewMatrix">The view matrix to use for the batch session.</param>
        /// <param name="projectionMatrix">The projection matrix to use for the batch session.</param>
        /// <param name="sortMode">The sprite drawing order to use for the batch session.</param>
        /// <param name="blendState">The blending state to use for the batch session, or <see langword="null"/> to use the default <see cref="BlendStates.AlphaBlend"/>.</param>
        /// <param name="samplerState">The sampling state to use for the batch session, or <see langword="null"/> to use the default <see cref="SamplerStateFactory.LinearClamp"/>.</param>
        /// <param name="depthStencilState">The depth stencil state to use for the batch session, or <see langword="null"/> to use the default <see cref="DepthStencilStates.Default"/>.</param>
        /// <param name="rasterizerState">The rasterizer state to use for the batch session, or <see langword="null"/> to use the default <see cref="RasterizerStates.CullBack"/>.</param>
        /// <param name="effect">The effect to use for the batch session, or <see langword="null"/> to use the default LineBatch Class shader.</param>
        /// <param name="stencilValue">The value of the stencil buffer to take as reference for the batch session.</param>
        /// <remarks>
        ///   Passing <see langword="null"/> for any of the state objects indicates the default default state object
        ///   should be used. Passing a <see langword="null"/> <paramref name="effect"/> selects the default LineBatch Class shader.
        /// </remarks>
        public void Begin(GraphicsContext graphicsContext, in Matrix viewMatrix, in Matrix projectionMatrix,
                          SpriteSortMode sortMode = SpriteSortMode.Deferred,
                          BlendStateDescription? blendState = null, SamplerState samplerState = null,
                          DepthStencilStateDescription? depthStencilState = null, RasterizerStateDescription? rasterizerState = null,
                          EffectInstance effect = null, int stencilValue = 0)
        {
            CheckEndHasBeenCalled("Begin");

            userViewMatrix = viewMatrix;
            userProjectionMatrix = projectionMatrix;

            Begin(graphicsContext, effect, sortMode, blendState, samplerState, depthStencilState, rasterizerState, stencilValue);
        }

        #endregion

        #region Draw sprites

        /// <summary>
        ///   Adds a line to a batch of lines for rendering using the specified texture, destination rectangle, and color.
        /// </summary>
        /// <param name="texture">The line texture.</param>
        /// <param name="destinationRectangle">A rectangle that specifies the destination for drawing the sprite, in screen coordinates.</param>
        /// <param name="color">The color to tint the sprite. Specify <see cref="Color.White"/> for full color with no tinting.</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void Draw(Texture texture, ref RectangleF destinationRectangle, Color4 color, Color4 colorAdd = default)
        {
            DrawSprite(texture, ref destinationRectangle, scaleDestination: false, sourceRectangle: default, color, colorAdd, rotation: 0, Vector2.Zero, SpriteEffects.None, ImageOrientation.AsIs, depth: 0);
        }

        /// <summary>
        ///   Adds a line to a batch of lines for rendering using the specified texture, position and color.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void Draw(Texture texture, Vector2 position)
        {
            Draw(texture, position, Color.White);
        }

        /// <summary>
        ///   Adds a line to a batch of lines for rendering using the specified texture, position and color.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void Draw(Texture texture, Vector2 position, Color color, Color4 colorAdd = default)
        {
            var destination = new RectangleF(position.X, position.Y, 1, 1);

            DrawSprite(texture, ref destination, scaleDestination: true, sourceRectangle: default, color, colorAdd, rotation: 0, Vector2.Zero, SpriteEffects.None, ImageOrientation.AsIs, depth: 0);
        }

        /// <summary>
        ///   Adds a line to a batch of lines for rendering using the specified texture, destination rectangle, source rectangle, color, rotation, origin, effects and layer.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="destinationRectangle">A rectangle that specifies (in screen coordinates) the destination for drawing the sprite. If this rectangle is not the same size as the source rectangle, the sprite will be scaled to fit.</param>
        /// <param name="sourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin in the texture in pixels (dependent of image orientation). Default value is (0,0) which represents the upper-left corner.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="orientation">The source image orientation</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void Draw(Texture texture, ref RectangleF destinationRectangle, in RectangleF? sourceRectangle,
                         Color4 color, float rotation, in Vector2 origin,
                         SpriteEffects effects = SpriteEffects.None, ImageOrientation orientation = ImageOrientation.AsIs,
                         float layerDepth = 0, Color4 colorAdd = default, SwizzleMode swizzle = SwizzleMode.None)
        {
            DrawSprite(texture, ref destinationRectangle, scaleDestination: false, sourceRectangle, color, colorAdd, rotation, origin, effects, orientation, layerDepth, swizzle);
        }

        /// <summary>
        ///   Adds a line to a batch of lines for rendering using the specified texture, position, source rectangle, color, rotation, origin, scale, effects, and layer.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin in the texture in pixels (dependent of image orientation). Default value is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="orientation">The source image orientation</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void Draw(Texture texture, Vector2 position, Color4 color, float rotation, Vector2 origin, float scale = 1.0f,
                         SpriteEffects effects = SpriteEffects.None, ImageOrientation orientation = ImageOrientation.AsIs,
                         float layerDepth = 0)
        {
            Draw(texture, position, sourceRectangle: null, color, rotation, origin, scale, effects, orientation, layerDepth);
        }

        /// <summary>
        ///   Adds a line to a batch of lines for rendering using the specified texture, position, source rectangle, color, rotation, origin, scale, effects, and layer.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin in the texture in pixels (dependent of image orientation). Default value is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="orientation">The source image orientation</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void Draw(Texture texture, Vector2 position, Color4 color, float rotation, Vector2 origin, Vector2 scale,
                         SpriteEffects effects = SpriteEffects.None, ImageOrientation orientation = ImageOrientation.AsIs,
                         float layerDepth = 0)
        {
            Draw(texture, position, sourceRectangle: null, color, rotation, origin, scale, effects, orientation, layerDepth);
        }

        /// <summary>
        ///   Adds a line to a batch of lines for rendering using the specified texture, position, source rectangle, and color.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="sourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void Draw(Texture texture, Vector2 position, in RectangleF? sourceRectangle, Color4 color, Color4 colorAdd = default)
        {
            var destination = new RectangleF(position.X, position.Y, 1, 1);

            DrawSprite(texture, ref destination, scaleDestination: true, sourceRectangle, color, colorAdd, rotation: 0, Vector2.Zero, SpriteEffects.None, ImageOrientation.AsIs, depth: 0);
        }

        /// <summary>
        ///   Adds a line to a batch of lines for rendering using the specified texture, position, source rectangle, color, rotation, origin, scale, effects, and layer.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="sourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
        /// <param name="color">The color to tint a sprite. Use Color4.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin in the texture in pixels (dependent of image orientation). Default value is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="orientation">The source image orientation</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void Draw(Texture texture, Vector2 position, RectangleF? sourceRectangle, Color4 color, float rotation,
                         Vector2 origin, float scale = 1, SpriteEffects effects = SpriteEffects.None, ImageOrientation orientation = ImageOrientation.AsIs,
                         float layerDepth = 0, Color4 colorAdd = default, SwizzleMode swizzle = SwizzleMode.None)
        {
            var destination = new RectangleF(position.X, position.Y, scale, scale);

            DrawSprite(texture, ref destination, scaleDestination: true, sourceRectangle, color, colorAdd, rotation, origin, effects, orientation, layerDepth, swizzle);
        }

        /// <summary>
        ///   Adds a line to a batch of lines for rendering using the specified texture, position, source rectangle, color, rotation, origin, scale, effects, and layer.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="sourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin in the texture in pixels (dependent of image orientation). Default value is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="orientation">The source image orientation</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void Draw(Texture texture, Vector2 position, in RectangleF? sourceRectangle, in Color4 color, float rotation,
                         Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, ImageOrientation orientation = ImageOrientation.AsIs,
                         float layerDepth = 0, in Color4 colorAdd = default)
        {
            var destination = new RectangleF(position.X, position.Y, scale.X, scale.Y);

            DrawSprite(texture, ref destination, scaleDestination: true, sourceRectangle, color, colorAdd, rotation, origin, effects, orientation, layerDepth);
        }

        internal unsafe void DrawSprite(Texture texture, ref RectangleF destination, bool scaleDestination, in RectangleF? sourceRectangle, in Color4 color, in Color4 colorAdd,
                                        float rotation, Vector2 origin, SpriteEffects effects, ImageOrientation orientation, float depth, SwizzleMode swizzle = SwizzleMode.None)
        {
            ArgumentNullException.ThrowIfNull(texture);

            // Put values in next ElementInfo
            var elementInfo = new ElementInfo();
            ref var spriteInfo = ref elementInfo.DrawInfo;

            float width;
            float height;

            if (sourceRectangle.HasValue)
            {
                // If the source rectangle has a value, then use it
                var rectangle = sourceRectangle.Value;
                spriteInfo.Source.X = rectangle.X;
                spriteInfo.Source.Y = rectangle.Y;
                width = rectangle.Width;
                height = rectangle.Height;
            }
            else
            {
                // Else, use directly the size of the texture
                spriteInfo.Source.X = 0;
                spriteInfo.Source.Y = 0;
                width = texture.ViewWidth;
                height = texture.ViewHeight;
            }

            // Sets the width and height
            spriteInfo.Source.Width = width;
            spriteInfo.Source.Height = height;

            // Scale the destination box
            if (scaleDestination)
            {
                if (orientation == ImageOrientation.Rotated90)
                {
                    destination.Width *= height;
                    destination.Height *= width;
                }
                else
                {
                    destination.Width *= width;
                    destination.Height *= height;
                }
            }

            // Sets the destination
            spriteInfo.Destination = destination;

            // Copy all other values
            spriteInfo.Origin = origin;
            spriteInfo.Rotation = rotation;
            spriteInfo.Depth = depth;
            spriteInfo.SpriteEffects = effects;
            spriteInfo.ColorScale = color;
            spriteInfo.ColorAdd = colorAdd;
            spriteInfo.Swizzle = swizzle;
            spriteInfo.TextureSize.X = texture.ViewWidth;
            spriteInfo.TextureSize.Y = texture.ViewHeight;
            spriteInfo.Orientation = orientation;

            elementInfo.VertexCount = StaticQuadBufferInfo.VerticesByElement;
            elementInfo.IndexCount = StaticQuadBufferInfo.IndicesByElement;
            elementInfo.Depth = depth;

            Draw(texture, in elementInfo);
        }

        #endregion

        #region Draw text

        /// <summary>
        /// Measure the size of the given text in virtual pixels depending on the target size.
        /// </summary>
        /// <param name="spriteFont">The font used to draw the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="targetSize">The size of the target to render in. If null, the size of the window back buffer is used.</param>
        /// <returns>The size of the text in virtual pixels.</returns>
        /// <exception cref="ArgumentNullException">The provided sprite font is null.</exception>
        public Vector2 MeasureString(SpriteFont spriteFont, string text, Vector2? targetSize = null)
        {
            ArgumentNullException.ThrowIfNull(spriteFont);

            return MeasureString(spriteFont, text, spriteFont.Size, targetSize);
        }

        /// <summary>
        /// Measure the size of the given text in virtual pixels depending on the target size.
        /// </summary>
        /// <param name="spriteFont">The font used to draw the text.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="fontSize">The font size (in pixels) used to draw the text.</param>
        /// <param name="targetSize">The size of the target to render in. If null, the size of the window back buffer is used.</param>
        /// <returns>The size of the text in virtual pixels.</returns>
        /// <exception cref="ArgumentNullException">The provided sprite font is null.</exception>
        public Vector2 MeasureString(SpriteFont spriteFont, string text, float fontSize, Vector2? targetSize = null)
        {
            ArgumentNullException.ThrowIfNull(spriteFont);

            if (string.IsNullOrEmpty(text))
                return Vector2.Zero;

            var targetSizeValue = targetSize ?? new Vector2(graphicsDevice.Presenter.BackBuffer.Width, graphicsDevice.Presenter.BackBuffer.Height);

            // Calculate the size of the text that will be used to draw
            var virtualResolution = VirtualResolution ?? new Vector3(targetSizeValue, DefaultDepth);
            var ratio = new Vector2(targetSizeValue.X / virtualResolution.X, targetSizeValue.Y / virtualResolution.Y);

            var realSize = spriteFont.MeasureString(text, fontSize * ratio);

            // convert pixel size into virtual pixel size (if needed)
            var virtualSize = realSize;
            virtualSize.X /= ratio.X;
            virtualSize.Y /= ratio.Y;

            return virtualSize;
        }

        /// <summary>
        ///   Adds a string to a batch of lines for rendering using the specified font, text, position, and color.
        /// </summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="alignment">Describes how to align the text to draw</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, in Color4 color, TextAlignment alignment = TextAlignment.Left)
        {
            var proxy = new SpriteFont.StringProxy(text);

            DrawString(spriteFont, proxy, fontSize: -1, position, color, rotation: 0, Vector2.Zero, Vector2.One, SpriteEffects.None, layerDepth: 0, alignment);
        }

        /// <summary>
        ///   Adds a string to a batch of lines for rendering using the specified font, text, position, and color.
        /// </summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">Text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="alignment">Describes how to align the text to draw</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, in Color4 color, TextAlignment alignment = TextAlignment.Left)
        {
            var proxy = new SpriteFont.StringProxy(text);

            DrawString(spriteFont, proxy, fontSize: -1, position, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, layerDepth: 0, alignment);
        }

        /// <summary>
        ///   Adds a string to a batch of lines for rendering using the specified font, text, position, and color.
        /// </summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="fontSize">The font size in pixels (ignored in the case of static fonts)</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="alignment">Describes how to align the text to draw</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void DrawString(SpriteFont spriteFont, string text, float fontSize, Vector2 position, in Color4 color, TextAlignment alignment = TextAlignment.Left)
        {
            var proxy = new SpriteFont.StringProxy(text);

            DrawString(spriteFont, proxy, fontSize, position, color, rotation: 0, Vector2.Zero, Vector2.One, SpriteEffects.None, layerDepth: 0, alignment);
        }

        /// <summary>
        ///   Adds a string to a batch of lines for rendering using the specified font, text, position, and color.
        /// </summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">Text string.</param>
        /// <param name="fontSize">The font size in pixels (ignored in the case of static fonts)</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="alignment">Describes how to align the text to draw</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void DrawString(SpriteFont spriteFont, StringBuilder text, float fontSize, Vector2 position, in Color4 color, TextAlignment alignment = TextAlignment.Left)
        {
            var proxy = new SpriteFont.StringProxy(text);

            DrawString(spriteFont, proxy, fontSize, position, color, rotation: 0, Vector2.Zero, Vector2.One, SpriteEffects.None, layerDepth: 0, alignment);
        }

        /// <summary>
        ///   Adds a string to a batch of lines for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer.
        /// </summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin in virtual pixels; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        /// <param name="alignment">Describes how to align the text to draw</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, in Color4 color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, TextAlignment alignment)
        {
            var proxy = new SpriteFont.StringProxy(text);

            DrawString(spriteFont, proxy, fontSize: -1, position, color, rotation, origin, scale, effects, layerDepth, alignment);
        }

        /// <summary>
        ///   Adds a string to a batch of lines for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer.
        /// </summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">Text string.</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin in virtual pixels; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        /// <param name="alignment">Describes how to align the text to draw</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, in Color4 color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, TextAlignment alignment)
        {
            var proxy = new SpriteFont.StringProxy(text);

            DrawString(spriteFont, proxy, fontSize: -1, position, color, rotation, origin, scale, effects, layerDepth, alignment);
        }

        /// <summary>
        ///   Adds a string to a batch of lines for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer.
        /// </summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">A text string.</param>
        /// <param name="fontSize">The font size in pixels (ignored in the case of static fonts)</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin in virtual pixels; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        /// <param name="alignment">Describes how to align the text to draw</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void DrawString(SpriteFont spriteFont, string text, float fontSize, Vector2 position, in Color4 color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, TextAlignment alignment)
        {
            var proxy = new SpriteFont.StringProxy(text);

            DrawString(spriteFont, proxy, fontSize, position, color, rotation, origin, scale, effects, layerDepth, alignment);
        }

        /// <summary>
        ///   Adds a string to a batch of lines for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer.
        /// </summary>
        /// <param name="spriteFont">A font for displaying text.</param>
        /// <param name="text">Text string.</param>
        /// <param name="fontSize">The font size in pixels (ignored in the case of static fonts)</param>
        /// <param name="position">The location (in screen coordinates) to draw the sprite.</param>
        /// <param name="color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
        /// <param name="origin">The sprite origin in virtual pixels; the default is (0,0) which represents the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effects">Effects to apply.</param>
        /// <param name="layerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
        /// <param name="alignment">Describes how to align the text to draw</param>
        /// <remarks>
        ///   Before making any calls to <c>Draw</c>, you must call <c>Begin</c>.
        ///   Once all calls to <c>Draw</c> are complete, call <c>End</c>.
        /// </remarks>
        public void DrawString(SpriteFont spriteFont, StringBuilder text, float fontSize, Vector2 position, in Color4 color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, TextAlignment alignment)
        {
            var proxy = new SpriteFont.StringProxy(text);

            DrawString(spriteFont, proxy, fontSize, position, color, rotation, origin, scale, effects, layerDepth, alignment);
        }

        private void DrawString(SpriteFont spriteFont, in SpriteFont.StringProxy text, float fontSize, Vector2 position, in Color4 color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, TextAlignment alignment)
        {
            ArgumentNullException.ThrowIfNull(spriteFont);

            ThrowIfNullText(text);

            if (fontSize < 0)
                fontSize = spriteFont.Size;

            // Calculate the resolution ratio between the screen real size and the virtual resolution
            var commandList = GraphicsContext.CommandList;
            var viewportSize = commandList.Viewport;
            var virtualResolution = GetCurrentResolution(commandList);
            var resolutionRatio = new Vector2(viewportSize.Width / virtualResolution.X, viewportSize.Height / virtualResolution.Y);
            scale.X /= resolutionRatio.X;
            scale.Y /= resolutionRatio.Y;

            var fontSize2 = fontSize * ((spriteFont.FontType == SpriteFontType.Dynamic) ? resolutionRatio : Vector2.One);
            var drawCommand = new SpriteFont.InternalDrawCommand(this, in fontSize2, in position, in color, rotation, in origin, in scale, effects, layerDepth);

            // Snap the position the closest 'real' pixel
            Vector2.Modulate(ref drawCommand.Position, ref resolutionRatio, out drawCommand.Position);
            drawCommand.Position.X = MathF.Round(drawCommand.Position.X);
            drawCommand.Position.Y = MathF.Round(drawCommand.Position.Y);
            drawCommand.Position.X /= resolutionRatio.X;
            drawCommand.Position.Y /= resolutionRatio.Y;

            spriteFont.InternalDraw(commandList, ref text, ref drawCommand, alignment);

            //
            // Helper to throw an exception when the text is null.
            //
            static void ThrowIfNullText(in SpriteFont.StringProxy text)
            {
                if (text.IsNull)
                    throw new ArgumentNullException(nameof(text));
            }
        }

        #endregion

        /// <inheritdoc/>
        protected override unsafe void UpdateBufferValuesFromElementInfo(in ElementInfo elementInfo, IntPtr vertexPtr, IntPtr indexPtr, int vertexOffset)
        {
            var vertex = (VertexPositionColorTextureSwizzle*) vertexPtr;

            ref LineDrawInfo drawInfo = ref Unsafe.AsRef(elementInfo.DrawInfo);

            float deltaX = 1 / drawInfo.TextureSize.X;
            float deltaY = 1 / drawInfo.TextureSize.Y;

            origin.X /= Math.Max(float.Epsilon, drawInfo.Source.Width);
            origin.Y /= Math.Max(float.Epsilon, drawInfo.Source.Height);

            for (int j = 0; j < 4; j++)
            {
                Vector2 corner = CornerOffsets[j];
                Vector2 position = new()
                {
                    X = (corner.X - origin.X) * drawInfo.Destination.Width,
                    Y = (corner.Y - origin.Y) * drawInfo.Destination.Height
                };

                vertex->Position.X = drawInfo.Destination.X + (position.X * rotation.X) - (position.Y * rotation.Y);
                vertex->Position.Y = drawInfo.Destination.Y + (position.X * rotation.Y) + (position.Y * rotation.X);
                vertex->Position.Z = drawInfo.Depth;
                vertex->Position.W = 1.0f;

                vertex->ColorScale = drawInfo.ColorScale;
                vertex->ColorAdd = drawInfo.ColorAdd;

                corner = CornerOffsets[((j ^ (int) drawInfo.SpriteEffects) + (int) drawInfo.Orientation) % 4];
                vertex->TextureCoordinate.X = (drawInfo.Source.X + corner.X * drawInfo.Source.Width) * deltaX;
                vertex->TextureCoordinate.Y = (drawInfo.Source.Y + corner.Y * drawInfo.Source.Height) * deltaY;

                vertex->Swizzle = (int) drawInfo.Swizzle;

                vertex++;
            }
        }

        /// <inheritdoc/>
        protected override void PrepareForRendering()
        {
            Matrix.Multiply(ref userViewMatrix, ref userProjectionMatrix, out var viewProjection);

            // Setup effect parameters: MatrixTransform
            Parameters.Set(SpriteBaseKeys.MatrixTransform, ref viewProjection);

            base.PrepareForRendering();
        }
    }
}
