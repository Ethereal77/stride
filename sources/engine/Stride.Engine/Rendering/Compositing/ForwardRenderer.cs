// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Collections;
using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Core.Storage;
using Stride.Graphics;
using Stride.Rendering.Images;
using Stride.Rendering.Lights;
using Stride.Rendering.Shadows;
using Stride.Rendering.SubsurfaceScattering;

namespace Stride.Rendering.Compositing
{
    /// <summary>
    ///   Represents a scene renderer that renders using a "Forward Rendering" technique.
    /// </summary>
    /// <remarks>
    ///   The renderer should use the current <see cref="RenderContext.RenderView"/> and <see cref="CameraComponentRendererExtensions.GetCurrentCamera"/>.
    /// </remarks>
    [Display("Forward renderer")]
    public partial class ForwardRenderer : SceneRendererBase, ISharedRenderer
    {
        // TODO: Should we use GraphicsDeviceManager.PreferredBackBufferFormat?
        public const PixelFormat DepthBufferFormat = PixelFormat.D24_UNorm_S8_UInt;

        private readonly ImageScaler Scaler = new ImageScaler();

        private IShadowMapRenderer shadowMapRenderer;
        private Texture depthStencilROCached;
        private MultisampleCount actualMultisampleCount = MultisampleCount.None;

        private readonly Logger logger = GlobalLogger.GetLogger(nameof(ForwardRenderer));

        private readonly FastList<Texture> currentRenderTargets = new FastList<Texture>();
        private readonly FastList<Texture> currentRenderTargetsNonMSAA = new FastList<Texture>();
        private Texture currentDepthStencil;
        private Texture currentDepthStencilNonMSAA;

        protected Texture viewOutputTarget;
        protected Texture viewDepthStencil;

        protected int ViewCount { get; private set; }

        protected int ViewIndex { get; private set; }

        public ClearRenderer Clear { get; set; } = new ClearRenderer();

        /// <summary>
        ///   Gets or sets a value indicating whether to enable Light Probe based indirect illumination.
        /// </summary>
        public bool LightProbes { get; set; } = true;

        /// <summary>
        ///   Gets or sets the main render stage for opaque geometry.
        /// </summary>
        public RenderStage OpaqueRenderStage { get; set; }

        /// <summary>
        ///   Gets or sets the render stage for transparent geometry.
        /// </summary>
        public RenderStage TransparentRenderStage { get; set; }

        /// <summary>
        ///   Gets the shadow map render stages for shadow casters. No shadow rendering will happen if <c>null</c>.
        /// </summary>
        [MemberCollection(NotNullItems = true)]
        public List<RenderStage> ShadowMapRenderStages { get; } = new List<RenderStage>();

        /// <summary>
        ///   Gets or sets the G-Buffer render stage where depth and possibly some other extra info are rendered to
        ///   buffers (i.e. normals).
        /// </summary>
        public RenderStage GBufferRenderStage { get; set; }

        /// <summary>
        ///   Gets or sets the post-processing effects renderer.
        /// </summary>
        public IPostProcessingEffects PostEffects { get; set; }

        /// <summary>
        ///   Gets or sets the volumetric light shafts effect renderer.
        /// </summary>
        public LightShafts LightShafts { get; set; }

        /// <summary>
        ///   Gets or sets the subsurface scattering effect renderer.
        /// </summary>
        public SubsurfaceScatteringBlur SubsurfaceScatteringBlurEffect { get; set; }

        /// <summary>
        ///   Gets or sets the level of multi-sampling anti-aliasing.
        /// </summary>
        public MultisampleCount MSAALevel { get; set; } = MultisampleCount.None;

        /// <summary>
        ///   Gets the MSAA Resolver used to resolve multi-sampled render targets into normal render targets.
        /// </summary>
        [NotNull]
        public MSAAResolver MSAAResolver { get; } = new MSAAResolver();

        /// <summary>
        ///   Gets or sets a value indicating whether the depth buffer generated during <see cref="OpaqueRenderStage"/>
        ///   will be available as a shader resource named <c>DepthBase.DepthStencil</c> during <see cref="TransparentRenderStage"/>.
        /// </summary>
        /// <remarks>
        ///   This is needed by some effects such as soft-edged particles.
        ///   <para/>
        ///   On recent platforms that can bind the depth buffer as read-only (<see cref="GraphicsDeviceFeatures.HasDepthAsReadOnlyRT"/>),
        ///   the depth buffer will be used as is. Otherwise, a copy will be generated.
        /// </remarks>
        [DefaultValue(true)]
        public bool BindDepthAsResourceDuringTransparentRendering { get; set; } = true;

        protected override void InitializeCore()
        {
            base.InitializeCore();

            shadowMapRenderer = Context.RenderSystem.RenderFeatures
                .OfType<MeshRenderFeature>().FirstOrDefault()?.RenderFeatures
                .OfType<ForwardLightingRenderFeature>().FirstOrDefault()?.ShadowMapRenderer;

            if (MSAALevel != MultisampleCount.None)
            {
                actualMultisampleCount = (MultisampleCount) Math.Min((int) MSAALevel, (int) GraphicsDevice.Features[PixelFormat.R16G16B16A16_Float].MultisampleCountMax);
                actualMultisampleCount = (MultisampleCount) Math.Min((int) actualMultisampleCount, (int) GraphicsDevice.Features[DepthBufferFormat].MultisampleCountMax);

                // TODO: We cannot support MSAA on DX10 now
                //   Direct3D has MSAA support starting from version 11 because it requires multisample depth buffers as shader resource views.
                //   Therefore we force-disable MSAA on any platform that doesn't support MSAA.

                if (actualMultisampleCount != MSAALevel)
                {
                    logger.Warning($"Multisample count of {(int) MSAALevel} samples is not supported. Falling back to highest supported sample count of {(int) actualMultisampleCount} samples.");
                }
            }

            var camera = Context.GetCurrentCamera();
        }

        protected virtual void CollectStages(RenderContext context)
        {
            if (OpaqueRenderStage != null)
            {
                OpaqueRenderStage.OutputValidator.BeginCustomValidation(context.RenderOutput.DepthStencilFormat, context.RenderOutput.MultisampleCount);
                ValidateOpaqueStageOutput(OpaqueRenderStage.OutputValidator, context);
                OpaqueRenderStage.OutputValidator.EndCustomValidation();
            }

            if (TransparentRenderStage != null)
            {
                TransparentRenderStage.OutputValidator.Validate(ref context.RenderOutput);
            }

            if (GBufferRenderStage != null && LightProbes)
            {
                GBufferRenderStage.Output = new RenderOutputDescription(PixelFormat.None, context.RenderOutput.DepthStencilFormat);
            }
        }

        protected virtual void ValidateOpaqueStageOutput(RenderOutputValidator renderOutputValidator, RenderContext renderContext)
        {
            renderOutputValidator.Add<ColorTargetSemantic>(renderContext.RenderOutput.RenderTargetFormat0);

            if (PostEffects != null)
            {
                if (PostEffects.RequiresNormalBuffer)
                {
                    renderOutputValidator.Add<NormalTargetSemantic>(PixelFormat.R10G10B10A2_UNorm);
                }

                if (PostEffects.RequiresSpecularRoughnessBuffer)
                {
                    renderOutputValidator.Add<SpecularColorRoughnessTargetSemantic>(PixelFormat.R8G8B8A8_UNorm);
                }

                if (PostEffects.RequiresVelocityBuffer)
                {
                    renderOutputValidator.Add<VelocityTargetSemantic>(PixelFormat.R16G16_Float);
                }

                if (SubsurfaceScatteringBlurEffect != null)
                {
                    // TODO: This is just a workaround for now, because I can't sample an integer texture in a post process.
                    //       Should use this instead: PixelFormat.R8_UInt
                    renderOutputValidator.Add<MaterialIndexTargetSemantic>(PixelFormat.R16_Float);
                }
            }
        }

        protected virtual void CollectView(RenderContext context)
        {
            // Fill RenderStage formats and register render stages to main view
            if (OpaqueRenderStage != null)
            {
                context.RenderView.RenderStages.Add(OpaqueRenderStage);
            }

            if (TransparentRenderStage != null)
            {
                context.RenderView.RenderStages.Add(TransparentRenderStage);
            }

            if (GBufferRenderStage != null && LightProbes)
            {
                context.RenderView.RenderStages.Add(GBufferRenderStage);
            }
        }

        protected override unsafe void CollectCore(RenderContext context)
        {
            var camera = context.GetCurrentCamera();

            if (context.RenderView is null)
                throw new NullReferenceException(nameof(context.RenderView) + " is null. Please make sure you have your camera correctly set.");

            // Setup pixel formats for RenderStage
            using (context.SaveRenderOutputAndRestore())
            {
                // Mark this view as requiring shadows
                shadowMapRenderer?.RenderViewsWithShadows.Add(context.RenderView);

                context.RenderOutput = new RenderOutputDescription(PostEffects != null ? PixelFormat.R16G16B16A16_Float : context.RenderOutput.RenderTargetFormat0, DepthBufferFormat, MSAALevel);

                CollectStages(context);

                // Write params to view
                SceneCameraRenderer.UpdateCameraToRenderView(context, context.RenderView, camera);

                CollectView(context);

                LightShafts?.Collect(context);

                PostEffects?.Collect(context);

                // Set depth format for shadow map render stages
                foreach (var shadowMapRenderStage in ShadowMapRenderStages)
                {
                    // TODO: This format should be acquired from the ShadowMapRenderer instead of being fixed here
                    if (shadowMapRenderStage != null)
                        shadowMapRenderStage.Output = new RenderOutputDescription(PixelFormat.None, PixelFormat.D32_Float);
                }
            }

            PostEffects?.Collect(context);
        }

        protected static PixelFormat ComputeNonMSAADepthFormat(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R16_Float:
                case PixelFormat.R16_Typeless:
                case PixelFormat.D16_UNorm:
                    return PixelFormat.R16_Float;

                case PixelFormat.R32_Float:
                case PixelFormat.R32_Typeless:
                case PixelFormat.D32_Float:
                    return PixelFormat.R32_Float;

                // NOTE: For these formats we lose stencil buffer information during MSAA -> non-MSAA conversion
                case PixelFormat.R24G8_Typeless:
                case PixelFormat.D24_UNorm_S8_UInt:
                case PixelFormat.R24_UNorm_X8_Typeless:
                    return PixelFormat.R32_Float;

                case PixelFormat.R32G8X24_Typeless:
                case PixelFormat.D32_Float_S8X24_UInt:
                case PixelFormat.R32_Float_X8X24_Typeless:
                    return PixelFormat.R32_Float;

                default:
                    throw new NotSupportedException($"Unsupported depth format [{format}].");
            }
        }

        /// <summary>
        ///   Resolves the MSAA textures. Converts MSAA currentRenderTargets and currentDepthStencil into
        ///   currentRenderTargetsNonMSAA and currentDepthStencilNonMSAA.
        /// </summary>
        /// <param name="drawContext">The draw context.</param>
        private void ResolveMSAA(RenderDrawContext drawContext)
        {
            // Resolve render targets
            currentRenderTargetsNonMSAA.Resize(currentRenderTargets.Count, false);
            for (int index = 0; index < currentRenderTargets.Count; index++)
            {
                var input = currentRenderTargets[index];

                var outputDescription = TextureDescription.New2D(input.ViewWidth, input.ViewHeight, 1, input.Format, TextureFlags.ShaderResource | TextureFlags.RenderTarget);
                var output = PushScopedResource(drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(outputDescription));

                currentRenderTargetsNonMSAA[index] = output;
                MSAAResolver.Resolve(drawContext, input, output);
            }

            // Resolve depth buffer
            currentDepthStencilNonMSAA = viewDepthStencil;
            MSAAResolver.Resolve(drawContext, currentDepthStencil, currentDepthStencilNonMSAA);
        }

        protected virtual void DrawView(RenderContext context, RenderDrawContext drawContext)
        {
            var renderSystem = context.RenderSystem;

            // Z-Prepass
            var lightProbes = LightProbes && GBufferRenderStage != null;
            if (lightProbes)
            {
                // NOTE: Baking light probes before GBuffer prepass because we are updating some cbuffer parameters needed by Opaque pass that GBuffer pass might upload early
                PrepareLightprobeConstantBuffer(context);

                // TODO: Temporarily using ShadowMap shader
                using (drawContext.QueryManager.BeginProfile(Color.Green, CompositingProfilingKeys.GBuffer))
                using (drawContext.PushRenderTargetsAndRestore())
                {
                    drawContext.CommandList.Clear(drawContext.CommandList.DepthStencilBuffer, DepthStencilClearOptions.DepthBuffer);
                    drawContext.CommandList.SetRenderTarget(drawContext.CommandList.DepthStencilBuffer, null);

                    // Draw [Main view | Z-Prepass stage]
                    renderSystem.Draw(drawContext, context.RenderView, GBufferRenderStage);
                }

                // Bake lightprobes against Z-buffer
                BakeLightProbes(context, drawContext);
            }

            using (drawContext.PushRenderTargetsAndRestore())
            {
                // Draw [Main view | Main stage]
                if (OpaqueRenderStage != null)
                {
                    using (drawContext.QueryManager.BeginProfile(Color.Green, CompositingProfilingKeys.Opaque))
                    {
                        renderSystem.Draw(drawContext, context.RenderView, OpaqueRenderStage);
                    }
                }

                Texture depthStencilSRV = null;

                // Draw [Main view | SubSurface Scattering Post-process]
                if (SubsurfaceScatteringBlurEffect != null)
                {
                    var materialIndex = OpaqueRenderStage?.OutputValidator.Find<MaterialIndexTargetSemantic>() ?? -1;
                    if (materialIndex != -1)
                    {
                        using (drawContext.PushRenderTargetsAndRestore())
                        {
                            depthStencilSRV = ResolveDepthAsSRV(drawContext);

                            var renderTarget = drawContext.CommandList.RenderTargets[0];
                            var materialIndexRenderTarget = drawContext.CommandList.RenderTargets[materialIndex];

                            SubsurfaceScatteringBlurEffect.Draw(drawContext, renderTarget, materialIndexRenderTarget, depthStencilSRV, renderTarget);
                        }
                    }
                }

                // Draw [Main view | Transparent stage]
                if (TransparentRenderStage != null)
                {
                    // Some transparent shaders will require the depth as a shader resource - resolve it only once and set it here
                    using (drawContext.QueryManager.BeginProfile(Color.Green, CompositingProfilingKeys.Transparent))
                    using (drawContext.PushRenderTargetsAndRestore())
                    {
                        if (depthStencilSRV is null)
                            depthStencilSRV = ResolveDepthAsSRV(drawContext);

                        renderSystem.Draw(drawContext, context.RenderView, TransparentRenderStage);
                    }
                }

                var colorTargetIndex = OpaqueRenderStage?.OutputValidator.Find(typeof(ColorTargetSemantic)) ?? -1;
                if (colorTargetIndex == -1)
                    return;

                // Resolve MSAA targets
                var renderTargets = currentRenderTargets;
                var depthStencil = currentDepthStencil;
                if (actualMultisampleCount != MultisampleCount.None)
                {
                    using (drawContext.QueryManager.BeginProfile(Color.Green, CompositingProfilingKeys.MsaaResolve))
                    {
                        ResolveMSAA(drawContext);
                    }

                    renderTargets = currentRenderTargetsNonMSAA;
                    depthStencil = currentDepthStencilNonMSAA;
                }

                // Draw [Main view | Light Shafts]
                if (LightShafts != null)
                {
                    using (drawContext.QueryManager.BeginProfile(Color.Green, CompositingProfilingKeys.LightShafts))
                    {
                        LightShafts.Draw(drawContext, depthStencil, renderTargets[colorTargetIndex]);
                    }
                }

                // Draw Post-Processing effects
                if (PostEffects != null)
                {
                    // NOTE: OpaqueRenderStage can't be null otherwise colorTargetIndex would be -1
                    PostEffects.Draw(drawContext, OpaqueRenderStage.OutputValidator, renderTargets.Items, depthStencil, viewOutputTarget);
                }
                else
                {
                    if (actualMultisampleCount != MultisampleCount.None)
                    {
                        using (drawContext.QueryManager.BeginProfile(Color.Green, CompositingProfilingKeys.MsaaResolve))
                        {
                            drawContext.CommandList.Copy(renderTargets[colorTargetIndex], viewOutputTarget);
                        }
                    }
                }

                // Free the depth texture since we won't need it anymore
                if (depthStencilSRV != null)
                {
                    drawContext.Resolver.ReleaseDepthStenctilAsShaderResource(depthStencilSRV);
                }
            }
        }

        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            var viewport = drawContext.CommandList.Viewport;

            using (drawContext.PushRenderTargetsAndRestore())
            {
                // Render Shadow maps
                shadowMapRenderer?.Draw(drawContext);

                PrepareRenderTargets(drawContext, new Size2((int) viewport.Width, (int) viewport.Height));

                //var sssMaterialIndexRenderTarget = GenerateSSSMaterialIndexRenderTarget(context, viewport);

                using (drawContext.PushRenderTargetsAndRestore())
                {
                    drawContext.CommandList.SetRenderTargets(currentDepthStencil, currentRenderTargets.Count, currentRenderTargets.Items);

                    // Clear render target and depth stencil
                    Clear?.Draw(drawContext);

                    DrawView(context, drawContext);
                }
            }

            // Clear intermediate results
            currentRenderTargets.Clear();
            currentRenderTargetsNonMSAA.Clear();
            currentDepthStencil = null;
            currentDepthStencilNonMSAA = null;
        }

        private void CopyOrScaleTexture(RenderDrawContext drawContext, Texture input, Texture output)
        {
            if (input.Size != output.Size)
            {
                Scaler.SetInput(0, input);
                Scaler.SetOutput(output);
                Scaler.Draw(drawContext);
            }
            else
            {
                drawContext.CommandList.Copy(input, output);
            }
        }

        private Texture ResolveDepthAsSRV(RenderDrawContext context)
        {
            if (!BindDepthAsResourceDuringTransparentRendering)
                return null;

            var depthStencil = context.CommandList.DepthStencilBuffer;
            var depthStencilSRV = context.Resolver.ResolveDepthStencil(context.CommandList.DepthStencilBuffer);

            var renderView = context.RenderContext.RenderView;

            foreach (var renderFeature in context.RenderContext.RenderSystem.RenderFeatures)
            {
                if (!(renderFeature is RootEffectRenderFeature))
                    continue;

                var depthLogicalKey = ((RootEffectRenderFeature)renderFeature).CreateViewLogicalGroup("Depth");
                var viewFeature = renderView.Features[renderFeature.Index];

                // Copy ViewProjection to PerFrame cbuffer
                foreach (var viewLayout in viewFeature.Layouts)
                {
                    var resourceGroup = viewLayout.Entries[renderView.Index].Resources;

                    var depthLogicalGroup = viewLayout.GetLogicalGroup(depthLogicalKey);
                    if (depthLogicalGroup.Hash == ObjectId.Empty)
                        continue;

                    // Might want to use ProcessLogicalGroup if more than one Recource
                    resourceGroup.DescriptorSet.SetShaderResourceView(depthLogicalGroup.DescriptorSlotStart, depthStencilSRV);
                }
            }

            context.CommandList.SetRenderTargets(null, context.CommandList.RenderTargetCount, context.CommandList.RenderTargets);

            var depthStencilROCached = context.Resolver.GetDepthStencilAsRenderTarget(depthStencil, this.depthStencilROCached);
            if (depthStencilROCached != this.depthStencilROCached)
            {
                // Dispose cached view
                this.depthStencilROCached?.Dispose();
                this.depthStencilROCached = depthStencilROCached;
            }
            context.CommandList.SetRenderTargets(depthStencilROCached, context.CommandList.RenderTargetCount, context.CommandList.RenderTargets);

            return depthStencilSRV;
        }

        private void PrepareRenderTargets(RenderDrawContext drawContext, Texture outputRenderTarget, Texture outputDepthStencil)
        {
            if (OpaqueRenderStage is null)
                return;

            var renderTargets = OpaqueRenderStage.OutputValidator.RenderTargets;

            currentRenderTargets.Resize(renderTargets.Count, false);

            for (int index = 0; index < renderTargets.Count; index++)
            {
                if (renderTargets[index].Semantic is ColorTargetSemantic && PostEffects is null && actualMultisampleCount == MultisampleCount.None)
                {
                    currentRenderTargets[index] = outputRenderTarget;
                }
                else
                {
                    var description = renderTargets[index];
                    var textureDescription = TextureDescription.New2D(outputRenderTarget.Width, outputRenderTarget.Height, 1, description.Format, TextureFlags.RenderTarget | TextureFlags.ShaderResource, 1, GraphicsResourceUsage.Default, actualMultisampleCount);
                    currentRenderTargets[index] = PushScopedResource(drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(textureDescription));
                }

                drawContext.CommandList.ResourceBarrierTransition(currentRenderTargets[index], GraphicsResourceState.RenderTarget);
            }

            // Prepare depth buffer
            if (actualMultisampleCount == MultisampleCount.None)
            {
                currentDepthStencil = outputDepthStencil;
            }
            else
            {
                var description = outputDepthStencil.Description;
                var textureDescription = TextureDescription.New2D(description.Width, description.Height, 1, description.Format, TextureFlags.DepthStencil | TextureFlags.ShaderResource, 1, GraphicsResourceUsage.Default, actualMultisampleCount);
                currentDepthStencil = PushScopedResource(drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(textureDescription));
            }
            drawContext.CommandList.ResourceBarrierTransition(currentDepthStencil, GraphicsResourceState.DepthWrite);
        }

        /// <summary>
        ///   Prepares the render targets per frame, caching and handling MSAA, etc.
        /// </summary>
        /// <param name="drawContext">The current draw context</param>
        /// <param name="renderTargetsSize">The render target size</param>
        protected virtual void PrepareRenderTargets(RenderDrawContext drawContext, Size2 renderTargetsSize)
        {
            viewOutputTarget = drawContext.CommandList.RenderTarget;
            if (drawContext.CommandList.RenderTargetCount == 0)
                viewOutputTarget = null;
            viewDepthStencil = drawContext.CommandList.DepthStencilBuffer;

            // Create output if needed
            if (viewOutputTarget is null || viewOutputTarget.MultisampleCount != MultisampleCount.None)
            {
                viewOutputTarget = PushScopedResource(drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(
                    TextureDescription.New2D(renderTargetsSize.Width, renderTargetsSize.Height, 1, PixelFormat.R8G8B8A8_UNorm_SRgb,
                        TextureFlags.ShaderResource | TextureFlags.RenderTarget)));
            }

            // Create depth if needed
            if (viewDepthStencil is null || viewDepthStencil.MultisampleCount != MultisampleCount.None)
            {
                viewDepthStencil = PushScopedResource(drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(
                    TextureDescription.New2D(renderTargetsSize.Width, renderTargetsSize.Height, 1, DepthBufferFormat,
                        TextureFlags.ShaderResource | TextureFlags.DepthStencil)));
            }

            PrepareRenderTargets(drawContext, viewOutputTarget, viewDepthStencil);
        }

        protected override void Destroy()
        {
            PostEffects?.Dispose();
            depthStencilROCached?.Dispose();
        }
    }
}
