// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Rendering;

namespace Stride.Graphics
{
    /// <summary>
    ///   Represents a primitive quad use to draw an effect on a quad (fullscreen by default).
    /// </summary>
    public class PrimitiveQuad : ComponentBase
    {
        /// <summary>
        ///   The pipeline state.
        /// </summary>
        private readonly MutablePipelineState pipelineState;

        private readonly EffectInstance simpleEffect;
        private readonly SharedData sharedData;

        private const int QuadCount = 3;

        public static readonly VertexDeclaration VertexDeclaration = VertexPositionNormalTexture.Layout;
        public static readonly PrimitiveType PrimitiveType = PrimitiveType.TriangleList;

        /// <summary>
        ///   Initializes a new instance of the <see cref="PrimitiveQuad" /> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="effect">The effect.</param>
        public PrimitiveQuad(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            sharedData = GraphicsDevice.GetOrCreateSharedData(GraphicsDeviceSharedDataType.PerDevice, "PrimitiveQuad::VertexBuffer", d => new SharedData(GraphicsDevice));

            simpleEffect = new EffectInstance(new Effect(GraphicsDevice, SpriteEffect.Bytecode));
            simpleEffect.Parameters.Set(SpriteBaseKeys.MatrixTransform, Matrix.Identity);
            simpleEffect.UpdateEffect(graphicsDevice);

            pipelineState = new MutablePipelineState(GraphicsDevice);
            pipelineState.State.SetDefaults();
            pipelineState.State.InputElements = VertexDeclaration.CreateInputElements();
            pipelineState.State.PrimitiveType = PrimitiveType;
        }

        /// <summary>
        ///   Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        ///   Gets the parameters used.
        /// </summary>
        /// <value>The parameters.</value>
        public ParameterCollection Parameters => simpleEffect.Parameters;

        /// <summary>
        ///   Draws the quad. An effect must have been applied before calling this method with a pixel shader having the
        ///   signature <c>float2 : TEXCOORD</c>.
        /// </summary>
        /// <param name="commandList">The command list to use to draw.</param>
        public void Draw(CommandList commandList)
        {
            commandList.SetVertexBuffer(0, sharedData.VertexBuffer.Buffer, sharedData.VertexBuffer.Offset, sharedData.VertexBuffer.Stride);
            commandList.Draw(QuadCount);
        }

        /// <summary>
        ///   Draws the quad with the specified effect.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use to draw the quad.</param>
        /// <param name="effectInstance">The effect instance used to draw the quad.</param>
        public void Draw(GraphicsContext graphicsContext, EffectInstance effectInstance)
        {
            effectInstance.UpdateEffect(GraphicsDevice);

            pipelineState.State.RootSignature = effectInstance.RootSignature;
            pipelineState.State.EffectBytecode = effectInstance.Effect.Bytecode;
            pipelineState.State.BlendState = BlendStates.Default;
            pipelineState.State.Output.CaptureState(graphicsContext.CommandList);
            pipelineState.Update();

            graphicsContext.CommandList.SetPipelineState(pipelineState.CurrentState);

            // Apply the effect
            effectInstance.Apply(graphicsContext);

            Draw(graphicsContext.CommandList);
        }

        /// <summary>
        ///   Draws the quad with the specified texture, using a simple pixel shader that is sampling the texture.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use to draw the texture.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="blendState">Blend state to use to draw the texture. If <c>null</c> is specified, no blending will be performed.</param>
        public void Draw(GraphicsContext graphicsContext, Texture texture, BlendStateDescription? blendState = null)
        {
            Draw(graphicsContext, texture, samplerState: null, Color.White, blendState);
        }

        /// <summary>
        ///   Draws the quad with the specified texture, using a simple pixel shader that is sampling the texture.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use to draw the texture.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="samplerState">The sampler state. If <c>null</c>, <see cref="SamplerStateFactory.LinearClamp"/> will be used by default.</param>
        /// <param name="color">The color tint. Specify <see cref="Color4.White"/> for no tinting.</param>
        /// <param name="blendState">Blend state to use to draw the texture. If <c>null</c> is specified, no blending will be performed.</param>
        public void Draw(GraphicsContext graphicsContext, Texture texture, SamplerState samplerState, Color4 color, BlendStateDescription? blendState = null)
        {
            pipelineState.State.RootSignature = simpleEffect.RootSignature;
            pipelineState.State.EffectBytecode = simpleEffect.Effect.Bytecode;
            pipelineState.State.BlendState = blendState ?? BlendStates.Default;
            pipelineState.State.Output.CaptureState(graphicsContext.CommandList);
            pipelineState.Update();
            graphicsContext.CommandList.SetPipelineState(pipelineState.CurrentState);

            // Make sure that we are using our vertex shader
            simpleEffect.Parameters.Set(SpriteEffectKeys.Color, color);
            simpleEffect.Parameters.Set(TexturingKeys.Texture0, texture);
            simpleEffect.Parameters.Set(TexturingKeys.Sampler, samplerState ?? GraphicsDevice.SamplerStates.LinearClamp);
            simpleEffect.Apply(graphicsContext);

            Draw(graphicsContext.CommandList);

            // TODO: ADD QUICK UNBIND FOR SRV
            //GraphicsDevice.Context.PixelShader.SetShaderResource(0, null);
        }

        /// <summary>
        ///   Internal structure used to store VertexBuffer and VertexInputLayout.
        /// </summary>
        private class SharedData : ComponentBase
        {
            /// <summary>
            ///   The vertex buffer
            /// </summary>
            public readonly VertexBufferBinding VertexBuffer;

            private static readonly VertexPositionNormalTexture[] QuadsVertices =
            {
                new VertexPositionNormalTexture(new Vector3(-1,  1, 0), new Vector3(0, 0, 1), new Vector2(0, 0)),
                new VertexPositionNormalTexture(new Vector3(+3,  1, 0), new Vector3(0, 0, 1), new Vector2(2, 0)),
                new VertexPositionNormalTexture(new Vector3(-1, -3, 0), new Vector3(0, 0, 1), new Vector2(0, 2)),
            };

            public SharedData(GraphicsDevice device)
            {
                var vertexBuffer = Buffer.Vertex.New(device, QuadsVertices).DisposeBy(this);

                // Register reload
                vertexBuffer.Reload = (graphicsResource) => ((Buffer)graphicsResource).Recreate(QuadsVertices);

                VertexBuffer = new VertexBufferBinding(vertexBuffer, VertexDeclaration, QuadsVertices.Length, VertexPositionNormalTexture.Size);
            }
        }
    }
}
