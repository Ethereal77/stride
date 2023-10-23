// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
//
// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using Stride.Core;
using Stride.Rendering;
using Stride.Shaders;

namespace Stride.Graphics
{
    /// <summary>
    ///   Base class to batch a group of draw calls into one.
    /// </summary>
    /// <typeparam name="TDrawInfo">A structure containing all the required information to draw one element of the batch.</typeparam>
    public abstract class BatchBase<TDrawInfo> : ComponentBase where TDrawInfo : struct
    {
        /// <summary>
        ///   Structure containing all the information required to batch one element.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        protected struct ElementInfo
        {
            /// <summary>
            ///   The number of vertices needed to draw the element.
            /// </summary>
            public int VertexCount;

            /// <summary>
            ///   The number of indices needed to draw the element.
            /// </summary>
            public int IndexCount;

            /// <summary>
            ///   The depth of the element. Used to sort the elements.
            /// </summary>
            public float Depth;

            /// <summary>
            ///   The user draw information.
            /// </summary>
            public TDrawInfo DrawInfo;


            /// <summary>
            ///   Initializes a new <see cref="ElementInfo"/> structure.
            /// </summary>
            /// <param name="vertexCount">The number of vertices needed to draw the element.</param>
            /// <param name="indexCount">The number of indices needed to draw the element.</param>
            /// <param name="drawInfo">The user draw information.</param>
            /// <param name="depth">The depth of the element. Used to sort the elements.</param>
            public ElementInfo(int vertexCount, int indexCount, in TDrawInfo drawInfo, float depth = 0)
            {
                VertexCount = vertexCount;
                IndexCount = indexCount;
                DrawInfo = drawInfo;
                Depth = depth;
            }
        }

        protected readonly ThreadLocal<DeviceResourceContext> ResourceContextPool;
        protected DeviceResourceContext ResourceContext;

        protected MutablePipelineState mutablePipeline;
        protected GraphicsDevice graphicsDevice;
        protected BlendStateDescription? blendState;
        protected RasterizerStateDescription? rasterizerState;
        protected SamplerState samplerState;
        protected DepthStencilStateDescription? depthStencilState;
        protected int stencilReferenceValue;
        protected SpriteSortMode sortMode;
        private ObjectParameterAccessor<Texture>? textureUpdater;
        private ObjectParameterAccessor<SamplerState>? samplerUpdater;

        private int[] sortIndices;
        private ElementInfo[] sortedDraws;
        private ElementInfo[] drawsQueue;
        private int drawsQueueCount;
        private Texture[] drawTextures;

        private readonly int vertexStructSize;
        private readonly int indexStructSize;

        // Indicates if we are between a call of Begin and End.
        private bool isBeginCalled;

        /// <summary>
        ///   Gets the effect used for the current batch session.
        /// </summary>
        protected EffectInstance Effect { get; private set; }

        /// <summary>
        ///   Gets the current graphics context.
        /// </summary>
        protected GraphicsContext GraphicsContext { get; private set; }

        /// <summary>
        ///   The default effect.
        /// </summary>
        protected readonly EffectInstance DefaultEffect;
        /// <summary>
        ///   The default effect when rendering in sRGB.
        /// </summary>
        protected readonly EffectInstance DefaultEffectSRgb;

        /// <summary>
        ///   Gets or sets the comparer to use when sorting the elements by texture.
        /// </summary>
        protected TextureIdComparer TextureComparer { get; set; } = new();
        /// <summary>
        ///   Gets or sets the comparer to use when sorting the elements by depth in back-to-front order.
        /// </summary>
        protected QueueComparer<ElementInfo> BackToFrontComparer { get; set; } = new SpriteBackToFrontComparer();
        /// <summary>
        ///   Gets or sets the comparer to use when sorting the elements by depth in front-to-back order.
        /// </summary>
        protected QueueComparer<ElementInfo> FrontToBackComparer { get; set; } = new SpriteFrontToBackComparer();

        internal const float DepthBiasShiftOneUnit = 0.0001f;


        /// <summary>
        ///   Initializes a new instance of the <see cref="BatchBase{TDrawInfo}"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="defaultEffectByteCode">The effect bytecode to use by default.</param>
        /// <param name="defaultEffectByteCodeSRgb">The effect bytecode to use by default when using sRGB.</param>
        /// <param name="resourceBufferInfo"></param>
        /// <param name="vertexDeclaration"></param>
        /// <param name="indexSize"></param>
        protected BatchBase(GraphicsDevice graphicsDevice, EffectBytecode defaultEffectByteCode, EffectBytecode defaultEffectByteCodeSRgb,
                            ResourceBufferInfo resourceBufferInfo, VertexDeclaration vertexDeclaration, int indexSize = sizeof(short))
        {
            ArgumentNullException.ThrowIfNull(defaultEffectByteCode);
            ArgumentNullException.ThrowIfNull(defaultEffectByteCodeSRgb);
            ArgumentNullException.ThrowIfNull(resourceBufferInfo);
            ArgumentNullException.ThrowIfNull(vertexDeclaration);

            this.graphicsDevice = graphicsDevice;
            mutablePipeline = new MutablePipelineState(graphicsDevice);
            // TODO GRAPHICS REFACTOR Should we initialize FX lazily?
            DefaultEffect = new EffectInstance(new Effect(graphicsDevice, defaultEffectByteCode) { Name = "BatchDefaultEffect" });
            DefaultEffectSRgb = new EffectInstance(new Effect(graphicsDevice, defaultEffectByteCodeSRgb) { Name = "BatchDefaultEffectSRgb" });

            drawsQueue = new ElementInfo[resourceBufferInfo.BatchCapacity];
            drawTextures = new Texture[resourceBufferInfo.BatchCapacity];

            // Set the vertex layout and size
            indexStructSize = indexSize;
            vertexStructSize = vertexDeclaration.CalculateSize();

            // Creates the vertex buffer (shared within a device context)
            // TODO: find a better way to do that, and check resource disposal
            ResourceContextPool = graphicsDevice.GetOrCreateSharedData(resourceBufferInfo.ResourceKey,
                device => new ThreadLocal<DeviceResourceContext>(
                    () => new DeviceResourceContext(device, vertexDeclaration, resourceBufferInfo),
                trackAllValues: true));
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            base.Destroy();
        }

        /// <summary>
        ///   Gets the parameters applied on the sprite batch effect.
        /// </summary>
        public ParameterCollection Parameters => Effect.Parameters;

        /// <summary>
        ///   Begins a sprite batch rendering using the specified sorting mode and blend state, sampler, depth stencil,
        ///   rasterizer state objects and a custom effect.
        /// </summary>
        /// <param name="graphicsContext">The graphics context to use.</param>
        /// <param name="effect">The effect to use for this batch session, or <see langword="null"/> to use the default effect.</param>
        /// <param name="sessionSortMode">The sprite drawing order used for the batch session.</param>
        /// <param name="sessionBlendState">Blending state used for the batch session, or <see langword="null"/> to use the default <see cref="BlendStates.AlphaBlend"/>.</param>
        /// <param name="sessionSamplerState">Texture sampling used for the batch session, or <see langword="null"/> to use the default <see cref="SamplerStateFactory.LinearClamp"/>.</param>
        /// <param name="sessionDepthStencilState">Depth and stencil state used for the batch session, or <see langword="null"/> to use the default <see cref="DepthStencilStates.Default"/>.</param>
        /// <param name="sessionRasterizerState">Rasterization state used for the batch session, or <see langword="null"/> to use the default <see cref="RasterizerStates.CullBack"/>.</param>
        /// <param name="stencilValue">The value of the stencil buffer to take as reference for the batch session.</param>
        /// <exception cref="InvalidOperationException">
        ///   Only one sprite batch at a time can use <see cref="SpriteSortMode.Immediate"/>.
        /// </exception>
        /// <remarks>
        ///   Passing <see langword="null"/> for any of the state objects indicates the default default state object
        ///   should be used. Passing a <see langword="null"/> <paramref name="effect"/> selects the default effect shader.
        /// </remarks>
        protected void Begin(GraphicsContext graphicsContext, EffectInstance effect, SpriteSortMode sessionSortMode,
                             BlendStateDescription? sessionBlendState, SamplerState sessionSamplerState,
                             DepthStencilStateDescription? sessionDepthStencilState, RasterizerStateDescription? sessionRasterizerState,
                             int stencilValue)
        {
            CheckEndHasBeenCalled("Begin");

            ResourceContext = ResourceContextPool.Value;

            GraphicsContext = graphicsContext;

            sortMode = sessionSortMode;
            blendState = sessionBlendState;
            samplerState = sessionSamplerState;
            depthStencilState = sessionDepthStencilState;
            rasterizerState = sessionRasterizerState;
            stencilReferenceValue = stencilValue;

            Effect = effect ?? (graphicsDevice.ColorSpace == ColorSpace.Linear ? DefaultEffectSRgb : DefaultEffect);

            // Force the effect to update
            Effect.UpdateEffect(graphicsDevice);

            textureUpdater = null;
            if (Effect.Effect.HasParameter(TexturingKeys.Texture0))
                textureUpdater = Effect.Parameters.GetAccessor(TexturingKeys.Texture0);
            if (Effect.Effect.HasParameter(TexturingKeys.TextureCube0))
                textureUpdater = Effect.Parameters.GetAccessor(TexturingKeys.TextureCube0);
            if (Effect.Effect.HasParameter(TexturingKeys.Texture3D0))
                textureUpdater = Effect.Parameters.GetAccessor(TexturingKeys.Texture3D0);

            samplerUpdater = null;
            if (Effect.Effect.HasParameter(TexturingKeys.Sampler))
                samplerUpdater = Effect.Parameters.GetAccessor(TexturingKeys.Sampler);

            // Immediate mode, then prepare for rendering here instead of End()
            if (sessionSortMode == SpriteSortMode.Immediate)
            {
                if (ResourceContext.IsInImmediateMode)
                {
                    ThrowIfImmediateMode();
                }

                PrepareForRendering();

                ResourceContext.IsInImmediateMode = true;
            }

            isBeginCalled = true;

            //
            // Helper to throw an exception when attempting to Begin() a SpriteBatch in Immediate mode while
            // a different one is currently being used also in Immediate mode.
            //
            [DoesNotReturn]
            static void ThrowIfImmediateMode()
            {
                throw new InvalidOperationException($"Only one SpriteBatch at a time can use {nameof(SpriteSortMode)}.{nameof(SpriteSortMode.Immediate)}");
            }
        }

        /// <summary>
        ///   Prepares the effects and shaders, their resources and parameters, and the state of the pipeline prior
        ///   to rendering the batch.
        /// </summary>
        protected virtual unsafe void PrepareForRendering()
        {
            // If no sampler state specified, use LinearClamp
            var localSamplerState = samplerState ?? graphicsDevice.SamplerStates.LinearClamp;

            // Sets the sampler state of the effect
            if (samplerUpdater.HasValue)
                Parameters.Set(samplerUpdater.Value, localSamplerState);

            Effect.UpdateEffect(graphicsDevice);

            // Setup states (Blend, DepthStencil, Rasterizer)
            mutablePipeline.State.SetDefaults();
            mutablePipeline.State.RootSignature = Effect.RootSignature;
            mutablePipeline.State.EffectBytecode = Effect.Effect.Bytecode;
            mutablePipeline.State.BlendState = blendState ?? BlendStates.AlphaBlend;
            mutablePipeline.State.DepthStencilState = depthStencilState ?? DepthStencilStates.Default;
            mutablePipeline.State.RasterizerState = rasterizerState ?? RasterizerStates.CullBack;
            mutablePipeline.State.InputElements = ResourceContext.InputElements;
            mutablePipeline.State.PrimitiveType = PrimitiveType.TriangleList;
            mutablePipeline.State.Output.CaptureState(GraphicsContext.CommandList);
            mutablePipeline.Update();

            // Bind pipeline
            if (mutablePipeline.State.DepthStencilState.StencilEnable)
                GraphicsContext.CommandList.SetStencilReference(stencilReferenceValue);
            GraphicsContext.CommandList.SetPipelineState(mutablePipeline.CurrentState);

            // Bind VB/IB
            if (ResourceContext.VertexBuffer != null)
                GraphicsContext.CommandList.SetVertexBuffer(index: 0, ResourceContext.VertexBuffer, offset: 0, vertexStructSize);
            if (ResourceContext.IndexBuffer != null)
                GraphicsContext.CommandList.SetIndexBuffer(ResourceContext.IndexBuffer, offset: 0, is32bits: indexStructSize == sizeof(int));
        }

        /// <summary>
        ///   Throws an exception if <c>Begin</c> has not been called.
        /// </summary>
        /// <param name="functionName">The name of the method doind the check.</param>
        /// <exception cref="InvalidOperationException">
        ///   <c>Begin</c> had to be called before calling <paramref name="functionName"/>.
        /// </exception>
        protected void CheckBeginHasBeenCalled(string functionName)
        {
            if (!isBeginCalled)
                throw new InvalidOperationException("Begin must be called before " + functionName);
        }

        /// <summary>
        ///   Throws an exception if <c>End</c> has not been called.
        /// </summary>
        /// <param name="functionName">The name of the method doind the check.</param>
        /// <exception cref="InvalidOperationException">
        ///   <c>End</c> had to be called before calling <paramref name="functionName"/>.
        /// </exception>
        protected void CheckEndHasBeenCalled(string functionName)
        {
            if (isBeginCalled)
                throw new InvalidOperationException("End must be called before " + functionName);
        }

        /// <summary>
        ///   Flushes the sprite batch and restores the device state to how it was before <c>Begin</c> was called.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///   Cannot end one sprite batch while another is using <see cref="SpriteSortMode.Immediate"/>.
        /// </exception>
        public void End()
        {
            CheckBeginHasBeenCalled("End");

            if (sortMode == SpriteSortMode.Immediate)
            {
                ResourceContext.IsInImmediateMode = false;
            }
            else if (drawsQueueCount > 0)
            {
                // Draw the queued sprites now
                if (ResourceContext.IsInImmediateMode)
                {
                    ThrowIfImmediateMode();
                }

                // If not immediate, then setup and render all sprites
                PrepareForRendering();
                FlushBatch();
            }

            ResourceContext = null;

            // We are inside the Begin / End pair
            isBeginCalled = false;

            //
            // Helper to throw an exception when attempting to End() a SpriteBatch while a different one is currently
            // being used in Immediate mode.
            //
            [DoesNotReturn]
            static void ThrowIfImmediateMode()
            {
                throw new InvalidOperationException($"Cannot end one SpriteBatch while another is using {nameof(SpriteSortMode)}.{nameof(SpriteSortMode.Immediate)}");
            }
        }

        /// <summary>
        ///   Sorts the sprites according to the specified <see cref="SpriteSortMode"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">The specified <see cref="SpriteSortMode"/> is not supported.</exception>
        private void SortSprites()
        {
            IComparer<int> comparer;

            switch (sortMode)
            {
                case SpriteSortMode.Texture:
                    TextureComparer.SpriteTextures = drawTextures;
                    comparer = TextureComparer;
                    break;

                case SpriteSortMode.BackToFront:
                    BackToFrontComparer.ImageInfos = drawsQueue;
                    comparer = BackToFrontComparer;
                    break;

                case SpriteSortMode.FrontToBack:
                    FrontToBackComparer.ImageInfos = drawsQueue;
                    comparer = FrontToBackComparer;
                    break;

                default:
                    ThrowUnsupportedSortMode();
                    break;
            }

            if ((sortIndices == null) || (sortIndices.Length < drawsQueueCount))
            {
                sortIndices = new int[drawsQueueCount];
                sortedDraws = new ElementInfo[drawsQueueCount];
            }

            // Reset all indices to the original order
            for (int i = 0; i < drawsQueueCount; i++)
            {
                sortIndices[i] = i;
            }

            Array.Sort(sortIndices, index: 0, drawsQueueCount, comparer);

            //
            // Helper for throwning an exception when an invalid SpriteSortMode is used.
            //
            [DoesNotReturn]
            static void ThrowUnsupportedSortMode() => throw new NotSupportedException();
        }

        /// <summary>
        ///   Draws all the queued elements in the batch.
        /// </summary>
        private void FlushBatch()
        {
            ElementInfo[] spriteQueueForBatch;

            // If Deferred, then sprites are displayed in the same order they arrived
            if (sortMode == SpriteSortMode.Deferred)
            {
                spriteQueueForBatch = drawsQueue;
            }
            else
            {
                // Else Sort all sprites according to their sprite order mode
                SortSprites();
                spriteQueueForBatch = sortedDraws;
            }

            // Iterate on all sprites and group batch per texture.
            int offset = 0;
            Texture previousTexture = null;
            for (int i = 0; i < drawsQueueCount; i++)
            {
                Texture texture;

                if (sortMode == SpriteSortMode.Deferred)
                {
                    texture = drawTextures[i];
                }
                else
                {
                    // Copy ordered sprites to the queue to batch
                    int index = sortIndices[i];
                    spriteQueueForBatch[i] = drawsQueue[index];

                    // Get the texture indirectly
                    texture = drawTextures[index];
                }

                if (texture != previousTexture)
                {
                    if (i > offset)
                    {
                        DrawBatchPerTexture(previousTexture, spriteQueueForBatch, offset, i - offset);
                    }

                    offset = i;
                    previousTexture = texture;
                }
            }

            // Draw the last batch
            DrawBatchPerTexture(previousTexture, spriteQueueForBatch, offset, drawsQueueCount - offset);

            // Reset the queue
            Array.Clear(drawTextures, 0, drawsQueueCount);
            drawsQueueCount = 0;

            // When sorting is disabled, we persist `sortedDraws` data from one batch to the next, to avoid
            // unnecessary work in GrowSortedSprites. But we never reuse these when sorting, because re-sorting
            // previously sorted items gives unstable ordering if some sprites have identical sort keys.
            if (sortMode != SpriteSortMode.Deferred)
            {
                Array.Clear(sortedDraws, 0, sortedDraws.Length);
            }
        }

        /// <summary>
        ///   Draws the elements of the batch. Sets the texture in the effect parameters, applies the effect, and
        ///   configures the pipeline.
        /// </summary>
        /// <param name="sprites">The elements to draw.</param>
        /// <param name="offset">The offset in <paramref name="sprites"/> to start from.</param>
        /// <param name="count">The number of elements to draw.</param>
        private void DrawBatchPerTexture(Texture texture, ElementInfo[] sprites, int offset, int count)
        {
            // Sets the texture for this sprite effect.
            //   Use an optimized version in order to avoid reapplying the sprite effect here just to change texture.
            //   We are calling the PixelShaderStage directly. We assume that the texture is on slot 0 as it is
            //   setup in the original BasicEffect shader.

            if (textureUpdater.HasValue)
                Effect.Parameters.Set(textureUpdater.Value, texture);

            Effect.Apply(GraphicsContext);

            // Draw the batch of sprites
            DrawBatchPerTextureAndPass(sprites, offset, count);
        }

        /// <summary>
        ///   Draws the elements of the batch. Assumes a texture is already set in the effect parameters, and the
        ///   effect is applied, and the pipeline configured.
        /// </summary>
        /// <param name="sprites">The elements to draw.</param>
        /// <param name="offset">The offset in <paramref name="sprites"/> to start from.</param>
        /// <param name="count">The number of elements to draw.</param>
        private void DrawBatchPerTextureAndPass(ElementInfo[] sprites, int offset, int count)
        {
            while (count > 0)
            {
                // How many indices / vertices do we want to draw?
                var indexCount = 0;
                var vertexCount = 0;
                var batchSize = 0;

                while (batchSize < count)
                {
                    var spriteIndex = offset + batchSize;
                    ref var spriteElementInfo = ref sprites[spriteIndex];

                    // How many sprites does the D3D vertex buffer have room for?
                    var remainingVertexSpace = ResourceContext.VertexCount - ResourceContext.VertexBufferPosition - vertexCount;
                    var remainingIndexSpace = ResourceContext.IndexCount - ResourceContext.IndexBufferPosition - indexCount;

                    // If there is not enough space left for either the indices or vertices of the current element,
                    if (spriteElementInfo.IndexCount > remainingIndexSpace ||
                        spriteElementInfo.VertexCount > remainingVertexSpace)
                    {
                        // If we haven't started the current batch yet, we restart at the beginning of the buffers
                        if (batchSize == 0)
                        {
                            ResourceContext.VertexBufferPosition = 0;
                            ResourceContext.IndexBufferPosition = 0;
                            continue;
                        }

                        // Else we perform the draw call and batch remaining elements in the next draw call
                        break;
                    }

                    ++batchSize;
                    vertexCount += spriteElementInfo.VertexCount;
                    indexCount += spriteElementInfo.IndexCount;
                }

                // Sets the data directly to the buffer in memory
                var offsetVertexInBytes = ResourceContext.VertexBufferPosition * vertexStructSize;
                var offsetIndexInBytes = ResourceContext.IndexBufferPosition * indexStructSize;

                if (ResourceContext.VertexBufferPosition == 0)
                {
                    if (ResourceContext.VertexBuffer != null)
                        GraphicsContext.Allocator.ReleaseReference(ResourceContext.VertexBuffer);

                    var tempBufferDesc = new BufferDescription(ResourceContext.VertexCount * vertexStructSize, BufferFlags.VertexBuffer, GraphicsResourceUsage.Dynamic);
                    ResourceContext.VertexBuffer = GraphicsContext.Allocator.GetTemporaryBuffer(tempBufferDesc);
                    GraphicsContext.CommandList.SetVertexBuffer(index: 0, ResourceContext.VertexBuffer, offset: 0, vertexStructSize);
                }

                if (ResourceContext.IsIndexBufferDynamic && ResourceContext.IndexBufferPosition == 0)
                {
                    if (ResourceContext.IndexBuffer != null)
                        GraphicsContext.Allocator.ReleaseReference(ResourceContext.IndexBuffer);

                    ResourceContext.IndexBuffer = GraphicsContext.Allocator.GetTemporaryBuffer(new BufferDescription(ResourceContext.IndexCount * indexStructSize, BufferFlags.IndexBuffer, GraphicsResourceUsage.Dynamic));
                    GraphicsContext.CommandList.SetIndexBuffer(ResourceContext.IndexBuffer, offset: 0, is32bits: indexStructSize == sizeof(int));
                }

                // ------------------------------------------------------------------------------------------------------------
                // CAUTION: Performance problem under x64 resolved by this special codepath:
                // For some unknown reasons, It seems that writing directly to the pointer returned by the MapSubresource is
                // extremely inefficient using x64 but using a temporary buffer and performing a mempcy to the locked region
                // seems to be running at the same speed than x86
                // ------------------------------------------------------------------------------------------------------------
                // TODO Check again why we need this code
                //if (IntPtr.Size == 8)
                //{
                //    if (x64TempBuffer == null)
                //    {
                //        x64TempBuffer = ToDispose(new DataBuffer(Utilities.SizeOf<VertexPositionColorTexture>() * MaxBatchSize * VerticesPerSprite));
                //    }

                //    // Perform the update of all vertices on a temporary buffer
                //    var texturePtr = (VertexPositionColorTexture*)x64TempBuffer.DataPointer;
                //    for (int i = 0; i < batchSize; i++)
                //    {
                //        UpdateBufferValuesFromElementInfo(ref sprites[offset + i], ref texturePtr, deltaX, deltaY);
                //    }

                //    // Then copy this buffer in one shot
                //    resourceContext.VertexBuffer.SetData(GraphicsDevice, new DataPointer(x64TempBuffer.DataPointer, batchSize * VerticesPerSprite * Utilities.SizeOf<VertexPositionColorTexture>()), offsetInBytes, noOverwrite);
                //}
                //else
                {
                    var mappedIndices = new MappedResource();

                    var mappedVertices = GraphicsContext.CommandList.MapSubresource(ResourceContext.VertexBuffer, subResourceIndex: 0, MapMode.WriteNoOverwrite, doNotWait: false, offsetVertexInBytes, vertexCount * vertexStructSize);

                    if (ResourceContext.IsIndexBufferDynamic)
                        mappedIndices = GraphicsContext.CommandList.MapSubresource(ResourceContext.IndexBuffer, subResourceIndex: 0, MapMode.WriteNoOverwrite, doNotWait: false, offsetIndexInBytes, indexCount * indexStructSize);

                    var vertexPointer = mappedVertices.DataBox.DataPointer;
                    var indexPointer = mappedIndices.DataBox.DataPointer;

                    for (var i = 0; i < batchSize; i++)
                    {
                        var spriteIndex = offset + i;
                        ref var spriteElementInfo = ref sprites[spriteIndex];

                        UpdateBufferValuesFromElementInfo(spriteElementInfo, vertexPointer, indexPointer, ResourceContext.VertexBufferPosition);

                        ResourceContext.VertexBufferPosition += spriteElementInfo.VertexCount;
                        vertexPointer += vertexStructSize * spriteElementInfo.VertexCount;
                        indexPointer += indexStructSize * spriteElementInfo.IndexCount;
                    }

                    GraphicsContext.CommandList.UnmapSubresource(mappedVertices);

                    if (ResourceContext.IsIndexBufferDynamic)
                        GraphicsContext.CommandList.UnmapSubresource(mappedIndices);
                }

                // Draw from the specified index
                GraphicsContext.CommandList.DrawIndexed(indexCount, ResourceContext.IndexBufferPosition);

                // Update position, offset and remaining count
                ResourceContext.IndexBufferPosition += indexCount;
                offset += batchSize;
                count -= batchSize;
            }
        }

        /// <summary>
        ///   Adds an element to draw with a texture to the batch.
        /// </summary>
        /// <param name="texture">The element's texture.</param>
        /// <param name="elementInfo">The information about the element to draw.</param>
        protected void Draw(Texture texture, in ElementInfo elementInfo)
        {
            // Make sure that Begin was called
            CheckBeginHasBeenCalled("Draw");

            // Resize the buffer of SpriteInfo
            if (drawsQueueCount >= drawsQueue.Length)
            {
                Array.Resize(ref drawsQueue, drawsQueue.Length * 2);
            }

            // set the info required to draw the image
            drawsQueue[drawsQueueCount] = elementInfo;

            // If we are in immediate mode, render the sprite directly
            if (sortMode == SpriteSortMode.Immediate)
            {
                DrawBatchPerTexture(texture, drawsQueue, offset: 0, count: 1);
            }
            else
            {
                if (drawTextures.Length < drawsQueue.Length)
                {
                    Array.Resize(ref drawTextures, drawsQueue.Length);
                }
                drawTextures[drawsQueueCount] = texture;
                drawsQueueCount++;
            }
        }

        /// <summary>
        ///   Updates the mapped vertex and index buffer values using the provided element info.
        /// </summary>
        /// <param name="elementInfo">The structure containing the information about the element to draw.</param>
        /// <param name="vertexPointer">A pointer to the vertex array buffer to update.</param>
        /// <param name="indexPointer">A pointer to the index array buffer to update. This value is 0 if the index buffer used is static.</param>
        /// <param name="vexterStartOffset">The offset in the vertex buffer where the vertex of the element starts.</param>
        protected abstract void UpdateBufferValuesFromElementInfo(in ElementInfo elementInfo, IntPtr vertexPointer, IntPtr indexPointer, int vexterStartOffset);

        #region Nested types

        /// <summary>
        ///   Describes how to build the batch vertex and index buffer.
        /// </summary>
        protected class ResourceBufferInfo
        {
            /// <summary>
            ///   Gets the key used to identify the GPU resource.
            /// </summary>
            public string ResourceKey { get; }

            /// <summary>
            ///   Gets or sets the initial number of draw calls that can be batched at one time.
            /// </summary>
            /// <remarks>
            ///   The batch will adjust the size of its data structures when needed if capacity is not sufficient.
            /// </remarks>
            public int BatchCapacity { get; set; }

            /// <summary>
            ///   Gets the number of vertices of the vertex buffer.
            /// </summary>
            public int VertexCount { get; protected set; }

            /// <summary>
            ///   Gets the number of indices of the index buffer.
            /// </summary>
            public int IndexCount { get; private set; }

            /// <summary>
            ///   Gets or sets the static indices to use for the index buffer.
            /// </summary>
            public short[] StaticIndices { get; set; }

            /// <summary>
            ///   Gets a value indicating whether the index buffer is static or dynamic.
            /// </summary>
            public bool IsIndexBufferDynamic => StaticIndices is null;


            /// <summary>
            ///   Creates the buffer resource information for a batch having both a dynamic index buffer and vertex buffer.
            /// </summary>
            /// <param name="resourceKey">The name of key to use to identify the resource.</param>
            /// <param name="indexCount">The number of indices contained by the index buffer.</param>
            /// <param name="vertexCount">The number of vertices contained by the vertex buffer.</param>
            public static ResourceBufferInfo CreateDynamicIndexBufferInfo(string resourceKey, int indexCount, int vertexCount)
            {
                return new ResourceBufferInfo(resourceKey, staticIndices: null, indexCount, vertexCount);
            }

            /// <summary>
            ///   Creates the buffer resource information for a batch having a dynamic vertex buffer but a static index buffer.
            /// </summary>
            /// <param name="resourceKey">The name of key to use to identify the resource.</param>
            /// <param name="staticIndices">The value of the indices to upload into the index buffer.</param>
            /// <param name="vertexCount">The number of vertices contained by the vertex buffer.</param>
            public static ResourceBufferInfo CreateStaticIndexBufferInfo(string resourceKey, short[] staticIndices, int vertexCount)
            {
                return new ResourceBufferInfo(resourceKey, staticIndices, indexCount: 0, vertexCount);
            }

            /// <summary>
            ///   Initializes a new instance of the <see cref="ResourceBufferInfo"/> class.
            /// </summary>
            /// <param name="resourceKey">The name of key to use to identify the resource.</param>
            /// <param name="staticIndices">The value of the indices to upload into the index buffer.</param>
            /// <param name="vertexCount">The number of vertices contained by the vertex buffer.</param>
            /// <param name="indexCount">The number of indices contained by the index buffer.</param>
            protected ResourceBufferInfo(string resourceKey, short[] staticIndices, int indexCount, int vertexCount)
            {
                if (staticIndices != null)
                    indexCount = staticIndices.Length;

                BatchCapacity = 64;
                ResourceKey = resourceKey;
                StaticIndices = staticIndices;
                IndexCount = indexCount;
                VertexCount = vertexCount;
            }
        }

        /// <summary>
        ///   Contains the information required to build a vertex and index buffer for simple quad-based batching.
        /// </summary>
        /// <remarks>
        ///   The index buffer is used in static mode and contains six indices for each quad.
        ///   The vertex buffer contains 4 vertices for each quad.
        ///   <para/>
        ///   The rectangle is composed of two triangles as follow:
        ///   <code>
        ///                     v0 - - - v1                       v0 - - - v1
        ///                     |  \   t1 |                       | t1   /  |
        ///  If cycle == true:  |    \    |   If cycle == false:  |    /    |
        ///                     | t2   \  |                       |  /   t2 |
        ///                     v3 - - - v2                       v2 - - - v3
        ///   </code>
        /// </remarks>
        protected class StaticQuadBufferInfo : ResourceBufferInfo
        {
            public const int IndicesByElement = 6;
            public const int VerticesByElement = 4;

            private StaticQuadBufferInfo(string resourceKey, short[] staticIndices, int vertexCount)
                : base(resourceKey, staticIndices, indexCount: 0, vertexCount)
            {
            }

            /// <summary>
            ///   Creates the buffer resource information for a batch having a dynamic vertex buffer and
            ///   a static index buffer prepared to batch quads composed of 2 triangles and 6 vertices.
            /// </summary>
            /// <param name="resourceKey">The name of key to use to identify the resource.</param>
            /// <param name="cycle">A value indicating the orientation of the triangles. See remarks for <see cref="StaticQuadBufferInfo"/>.</param>
            /// <param name="maxQuadNumber">The maximun number of quads that can be drawn in one batch.</param>
            /// <param name="batchCapacity">The initial number of draw calls that can be batched at one time.</param>
            public static StaticQuadBufferInfo CreateQuadBufferInfo(string resourceKey, bool cycle, int maxQuadNumber, int batchCapacity = 64)
            {
                var indices = new short[maxQuadNumber * IndicesByElement];
                var k = 0;
                for (var i = 0; i < indices.Length; k += VerticesByElement)
                {
                    indices[i++] = (short)(k + 0);
                    indices[i++] = (short)(k + 1);
                    indices[i++] = (short)(k + 2);
                    if (cycle)
                    {
                        indices[i++] = (short)(k + 0);
                        indices[i++] = (short)(k + 2);
                        indices[i++] = (short)(k + 3);
                    }
                    else
                    {
                        indices[i++] = (short)(k + 1);
                        indices[i++] = (short)(k + 3);
                        indices[i++] = (short)(k + 2);
                    }
                }

                return new StaticQuadBufferInfo(resourceKey, indices, VerticesByElement * maxQuadNumber) { BatchCapacity = batchCapacity };
            }
        }

        /// <summary>
        ///   A comparer that determines equality of <see cref="Texture"/>s by reference equality.
        /// </summary>
        protected class TextureIdComparer : IComparer<int>
        {
            public Texture[] SpriteTextures;

            public int Compare(int left, int right)
            {
                return ReferenceEquals(SpriteTextures[left], SpriteTextures[right]) ? 0 : 1;
            }
        }

        /// <summary>
        ///   A comparer that determines the sprite order by depth, back-to-front.
        /// </summary>
        private class SpriteBackToFrontComparer : QueueComparer<ElementInfo>
        {
            public override int Compare(int left, int right)
            {
                return ImageInfos[left].Depth.CompareTo(ImageInfos[right].Depth);
            }
        }

        /// <summary>
        ///   A comparer that determines the sprite order by depth, front-to-back.
        /// </summary>
        private class SpriteFrontToBackComparer : QueueComparer<ElementInfo>
        {
            public override int Compare(int left, int right)
            {
                return ImageInfos[right].Depth.CompareTo(ImageInfos[left].Depth);
            }
        }

        /// <summary>
        ///   Base class for comparers that reference the elements to compare by their indices in a queue.
        /// </summary>
        /// <typeparam name="TInfo">The type of information to compare.</typeparam>
        protected abstract class QueueComparer<TInfo> : IComparer<int>
        {
            public TInfo[] ImageInfos;

            public abstract int Compare(int x, int y);
        }

        /// <summary>
        ///   Represents a resource context unique per <see cref="GraphicsDevice"/> (DeviceContext).
        /// </summary>
        protected class DeviceResourceContext : ComponentBase
        {
            /// <summary>
            ///   The number of vertices.
            /// </summary>
            public readonly int VertexCount;

            /// <summary>
            ///   The vertex buffer of the batch.
            /// </summary>
            public Buffer VertexBuffer;

            /// <summary>
            ///   The number of indices.
            /// </summary>
            public readonly int IndexCount;

            /// <summary>
            ///   The index buffer of the batch.
            /// </summary>
            public Buffer IndexBuffer;

            /// <summary>
            ///   Indicates if the index buffer is dynamic (populated on the fly), or static (pre-populated and reused).
            /// </summary>
            public readonly bool IsIndexBufferDynamic;

            /// <summary>
            ///   The vertex declaration, describing the vertex elements formats and layout.
            /// </summary>
            public readonly VertexDeclaration VertexDeclaration;
            /// <summary>
            ///   The vertex elements.
            /// </summary>
            public readonly InputElementDescription[] InputElements;

            /// <summary>
            ///   The index of the current vertex in the vertex buffer.
            /// </summary>
            public int VertexBufferPosition;

            /// <summary>
            ///   The index of the current index in the index buffer.
            /// </summary>
            public int IndexBufferPosition;

            /// <summary>
            ///   Indicates if the batch system is drawing in immediate mode for this buffer.
            /// </summary>
            public bool IsInImmediateMode;


            /// <summary>
            ///   Initializes a new instance of the <see cref="DeviceResourceContext"/> class.
            /// </summary>
            /// <param name="graphicsDevice">The graphics device.</param>
            /// <param name="vertexDeclaration">The vertex declaration.</param>
            /// <param name="resourceBufferInfo">A <see cref="ResourceBufferInfo"/> describing the buffers.</param>
            public DeviceResourceContext(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, ResourceBufferInfo resourceBufferInfo)
            {
                VertexDeclaration = vertexDeclaration;
                VertexCount = resourceBufferInfo.VertexCount;
                IndexCount = resourceBufferInfo.IndexCount;
                IsIndexBufferDynamic = resourceBufferInfo.IsIndexBufferDynamic;

                if (!IsIndexBufferDynamic)
                {
                    IndexBuffer = Buffer.Index.New(graphicsDevice, resourceBufferInfo.StaticIndices).DisposeBy(this);
                    IndexBuffer.Reload = (graphicsResource, _) => ((Buffer) graphicsResource).Recreate(resourceBufferInfo.StaticIndices);
                }

                InputElements = vertexDeclaration.CreateInputElements();
            }
        }

        #endregion
    }
}
