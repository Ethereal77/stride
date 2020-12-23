// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core;
using Stride.Graphics;

namespace Stride.Rendering.Images
{
    /// <summary>
    ///   Represents a class that serves as a base for rendering image processing effects.
    /// </summary>
    [DataContract]
    public abstract class ImageEffect : DrawEffect, IImageEffect
    {
        private readonly Texture[] inputTextures;
        private int maxInputTextureIndex;

        private Texture outputDepthStencilView;
        private Texture outputRenderTargetView;
        private Texture[] outputRenderTargetViews;
        private Texture[] createdOutputRenderTargetViews;

        private Viewport? viewport;


        /// <summary>
        ///   Initializes a new instance of the <see cref="ImageEffect"/> class.
        /// </summary>
        /// <param name="name">The name to set for this image effect. Specify <c>null</c> to use the type name automatically.</param>
        /// <param name="supersample">A value indicating whether to use a supersampling pattern when scaling images.</param>
        protected ImageEffect(string name, bool supersample = false)
            : base(name)
        {
            inputTextures = new Texture[16];
            maxInputTextureIndex = -1;
            EnableSetRenderTargets = true;
            SamplingPattern = supersample ? SamplingPattern.Expanded : SamplingPattern.Linear;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ImageEffect"/> class.
        /// </summary>
        protected ImageEffect() : this(name: null, supersample: false) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ImageEffect"/> class.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <param name="name">The name to set for this image effect. Specify <c>null</c> to use the type name automatically.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is a <c>null</c> reference.</exception>
        protected ImageEffect(RenderContext context, string name = null) : this(name)
        {
            Initialize(context);
        }


        /// <summary>
        ///   Gets or sets a value indicating whether to set the depth and render targets from the output with
        ///   <see cref="CommandList.SetRenderTarget"/>.
        /// </summary>
        /// <value>A value indicating whether to set the depth and render targets from the output. The default value is <c>true</c>.</value>
        protected bool EnableSetRenderTargets { get; set; }

        /// <summary>
        ///   Sets an input texture.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <param name="texture">The texture.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="slot"/> must be in the range [0, 15].</exception>
        public void SetInput(int slot, Texture texture)
        {
            if (slot < 0 || slot >= inputTextures.Length)
                throw new ArgumentOutOfRangeException(nameof(slot), "slot must be in the range [0, 15]");

            inputTextures[slot] = texture;
            if (slot > maxInputTextureIndex)
            {
                maxInputTextureIndex = slot;
            }
        }

        /// <summary>
        ///   Resets the state of this effect.
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            maxInputTextureIndex = -1;
            Array.Clear(inputTextures, 0, inputTextures.Length);
            outputDepthStencilView = null;
            outputRenderTargetView = null;
            outputRenderTargetViews = null;
        }

        /// <summary>
        ///   Sets the render target output.
        /// </summary>
        /// <param name="view">The render target output view.</param>
        /// <exception cref="ArgumentNullException"><paramref name="view"/> is a <c>null</c> reference.</exception>
        public void SetOutput(Texture view)
        {
            if (view is null)
                throw new ArgumentNullException(nameof(view));

            SetOutputInternal(view);
        }

        /// <summary>
        ///   Sets the render target output.
        /// </summary>
        /// <param name="views">The render target output views.</param>
        /// <exception cref="ArgumentNullException"><paramref name="views"/> is a <c>null</c> reference.</exception>
        public void SetOutput(params Texture[] views)
        {
            if (views is null)
                throw new ArgumentNullException(nameof(views));

            SetOutputInternal(views);
        }

        /// <summary>
        ///   Sets the render target output.
        /// </summary>
        /// <param name="depthStencilView">The depth stencil output view.</param>
        /// <param name="renderTargetView">The render target output view.</param>
        public void SetDepthOutput(Texture depthStencilView, Texture renderTargetView)
        {
            outputDepthStencilView = depthStencilView;
            outputRenderTargetView = renderTargetView;
            outputRenderTargetViews = null;
        }

        /// <summary>
        ///   Sets the render target outputs.
        /// </summary>
        /// <param name="depthStencilView">The depth stencil output view.</param>
        /// <param name="renderTargetViews">The render target output views.</param>
        public void SetDepthOutput(Texture depthStencilView, params Texture[] renderTargetViews)
        {
            outputDepthStencilView = depthStencilView;
            outputRenderTargetView = null;
            outputRenderTargetViews = renderTargetViews;
        }

        /// <summary>
        ///   Sets the viewport to use.
        /// </summary>
        /// <param name="viewport">The viewport. Specify <c>null</c> to not set a specific viewport.</param>
        public void SetViewport(Viewport? viewport)
        {
            // TODO: Support multiple viewport?
            this.viewport = viewport;
        }

        protected override void PreDrawCore(RenderDrawContext context)
        {
            base.PreDrawCore(context);

            if (EnableSetRenderTargets)
                SetRenderTargets(context);
        }

        /// <summary>
        ///   Set the render targets for the image effect.
        /// </summary>
        /// <param name="context">The rendering context.</param>
        protected virtual void SetRenderTargets(RenderDrawContext context)
        {
            // Transtion inputs to read sources
            for (int i = 0; i <= maxInputTextureIndex; ++i)
            {
                if (inputTextures[i] != null)
                    context.CommandList.ResourceBarrierTransition(inputTextures[i], GraphicsResourceState.PixelShaderResource);
            }

            if (outputDepthStencilView != null)
                context.CommandList.ResourceBarrierTransition(outputDepthStencilView, GraphicsResourceState.DepthWrite);

            if (outputRenderTargetView != null)
            {
                // Transition render target
                context.CommandList.ResourceBarrierTransition(outputRenderTargetView, GraphicsResourceState.RenderTarget);

                if (outputRenderTargetView.ViewDimension == TextureDimension.TextureCube)
                {
                    if (createdOutputRenderTargetViews is null)
                        createdOutputRenderTargetViews = new Texture[6];

                    for (int i = 0; i < createdOutputRenderTargetViews.Length; i++)
                        createdOutputRenderTargetViews[i] = outputRenderTargetView.ToTextureView(ViewType.Single, i, 0);

                    context.CommandList.SetRenderTargetsAndViewport(outputDepthStencilView, createdOutputRenderTargetViews);
                }
                else
                {
                    context.CommandList.SetRenderTargetAndViewport(outputDepthStencilView, outputRenderTargetView);
                }
            }
            else if (outputRenderTargetViews != null)
            {
                // Transition render targets
                foreach (var renderTarget in outputRenderTargetViews)
                    context.CommandList.ResourceBarrierTransition(renderTarget, GraphicsResourceState.RenderTarget);

                context.CommandList.SetRenderTargetsAndViewport(outputDepthStencilView, outputRenderTargetViews);
            }
            else if (outputDepthStencilView != null)
            {
                context.CommandList.SetRenderTargetsAndViewport(outputDepthStencilView, null);
            }

            if (viewport.HasValue)
            {
                context.CommandList.SetViewport(viewport.Value);
            }
        }

        protected override void PostDrawCore(RenderDrawContext context)
        {
            if (EnableSetRenderTargets)
                DisposeCreatedRenderTargetViews(context);

            base.PostDrawCore(context);
        }

        /// <summary>
        ///   Dispose the render target views that have been created.
        /// </summary>
        protected virtual void DisposeCreatedRenderTargetViews(RenderDrawContext context)
        {
            if (createdOutputRenderTargetViews is null)
                return;

            for (int i = 0; i < createdOutputRenderTargetViews.Length; i++)
            {
                createdOutputRenderTargetViews[i].Dispose();
                createdOutputRenderTargetViews[i] = null;
            }
        }

        /// <summary>
        ///   Gets the number of input textures.
        /// </summary>
        /// <value>The input count.</value>
        public int InputCount => maxInputTextureIndex + 1;

        /// <summary>
        ///   Gets an input texture by the specified index.
        /// </summary>
        /// <param name="index">The index of the input texture.</param>
        /// <returns>The texture, or <c>null</c> if there is no input texture set for <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is an invald texture input index.</exception>
        public Texture GetInput(int index)
        {
            if (index < 0 || index > maxInputTextureIndex)
                throw new ArgumentOutOfRangeException(nameof(index), $"Invald texture input index [{index}]. Max value is [{maxInputTextureIndex}].");

            return inputTextures[index];
        }

        /// <summary>
        ///   Gets a non-null input texture by the specified index.
        /// </summary>
        /// <param name="index">The index of the input texture.</param>
        /// <returns>The texture.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is an invald texture input index.</exception>
        /// <exception cref="InvalidOperationException">The input texture specified by <paramref name="index"/> is not set (is <c>null</c>).</exception>
        protected Texture GetSafeInput(int index)
        {
            var input = GetInput(index);
            if (input is null)
                throw new InvalidOperationException($"Expecting texture input on slot [{index}].");

            return input;
        }

        /// <summary>
        ///   Gets the output depth stencil texture.
        /// </summary>
        /// <value>The depth stencil output.</value>
        protected Texture DepthStencil => outputDepthStencilView;

        /// <summary>
        ///   Gets a value indicating whether this effect has depth stencil output texture binded.
        /// </summary>
        /// <value><c>true</c> if this instance has depth stencil output; otherwise, <c>false</c>.</value>
        protected bool HasDepthStencilOutput => outputDepthStencilView != null;

        /// <summary>
        ///   Gets the number of output render targets.
        /// </summary>
        /// <value>The output count.</value>
        public int OutputCount =>
            outputRenderTargetView != null ? 1 :
            outputRenderTargetViews != null ? outputRenderTargetViews.Length : 0;

        /// <summary>
        ///   Gets an output render target for the specified index.
        /// </summary>
        /// <param name="index">The index of the output texture.</param>
        /// <returns>The texture, or <c>null</c> if there is no output texture set for <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is an invald texture output index.</exception>
        public Texture GetOutput(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), $"Invald texture output index [{index}] cannot be negative for effect [{Name}].");

            return outputRenderTargetView ??
                   (outputRenderTargetViews != null ? outputRenderTargetViews[index] : null);
        }

        /// <summary>
        ///   Gets an non-null output render target for the specified index.
        /// </summary>
        /// <param name="index">The index of the output texture.</param>
        /// <returns>The texture.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is an invald texture output index.</exception>
        /// <exception cref="InvalidOperationException">The output texture specified by <paramref name="index"/> is not set (is <c>null</c>).</exception>
        protected Texture GetSafeOutput(int index)
        {
            var output = GetOutput(index);
            if (output is null)
                throw new InvalidOperationException($"Expecting texture output on slot [{index}].");

            return output;
        }

        /// <summary>
        ///   Gets a render target for the specified description, scoped for the duration of the <see cref="RendererBase.DrawCore"/>.
        /// </summary>
        /// <returns>A new instance of texture.</returns>
        protected Texture NewScopedRenderTarget2D(TextureDescription description)
        {
            CheckIsInDrawCore();

            // TODO: Check if we should introduce an enum for the kind of scope (per DrawCore, per Frame...etc.)
            return PushScopedResource(Context.Allocator.GetTemporaryTexture2D(description));
        }

        /// <summary>
        ///   Gets a render target for the specified description with a single mipmap, scoped for the duration of the <see cref="RendererBase.DrawCore"/>.
        /// </summary>
        /// <param name="width">The width of the render target.</param>
        /// <param name="height">The height of the render target.</param>
        /// <param name="format">The pixel format to use.</param>
        /// <param name="flags">
        ///   The texture flags (for unordered access, render targets, etc). Default value is <see cref="TextureFlags.RenderTarget"/>
        ///   and <see cref="TextureFlags.ShaderResource"/>.
        /// </param>
        /// <param name="arraySize">Size of the texture array. Default value is 1.</param>
        /// <returns>A new instance of texture.</returns>
        protected Texture NewScopedRenderTarget2D(int width, int height, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            CheckIsInDrawCore();

            return PushScopedResource(Context.Allocator.GetTemporaryTexture2D(width, height, format, flags, arraySize));
        }

        /// <summary>
        ///   Gets a render target for the specified description, scoped for the duration of the <see cref="RendererBase.DrawCore"/>.
        /// </summary>
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
        protected Texture NewScopedRenderTarget2D(int width, int height, PixelFormat format, MipMapCount mipCount, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            CheckIsInDrawCore();

            return PushScopedResource(Context.Allocator.GetTemporaryTexture2D(width, height, format, mipCount, flags, arraySize));
        }

        private void SetOutputInternal(Texture view)
        {
            // TODO: Do we want to handle the output the same way we handle the input textures?
            outputDepthStencilView = null;
            outputRenderTargetView = view;
            outputRenderTargetViews = null;
        }

        private void SetOutputInternal(params Texture[] views)
        {
            // TODO: Do we want to handle the output the same way we handle the input textures?
            outputDepthStencilView = null;
            outputRenderTargetView = null;
            outputRenderTargetViews = views;
        }
    }
}
