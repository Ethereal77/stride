// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Linq;

using Stride.Core;
using Stride.Rendering.Lights;
using Stride.Rendering.Shadows;
using Stride.Rendering.Compositing;
using Stride.Rendering.Voxels.Debug;

namespace Stride.Rendering.Voxels
{
    /// <summary>
    ///   Forward renderer specialization for voxel-based global illumination.
    /// </summary>
    [Display("Forward & Voxel renderer")]
    public class ForwardRendererVoxels : ForwardRenderer
    {
        public IVoxelRenderer VoxelRenderer { get; set; }

        protected IShadowMapRenderer ShadowMapRenderer_notPrivate;

        public VoxelDebug VoxelVisualization { get; set; }

        protected override void InitializeCore()
        {
            ShadowMapRenderer_notPrivate =
                Context.RenderSystem.RenderFeatures
                    .OfType<MeshRenderFeature>()
                    .FirstOrDefault()?.RenderFeatures
                        .OfType<ForwardLightingRenderFeature>()
                        .FirstOrDefault()?.ShadowMapRenderer;

            base.InitializeCore();
        }

        protected override void CollectCore(RenderContext context)
        {
            VoxelRenderer?.Collect(Context, ShadowMapRenderer_notPrivate);

            base.CollectCore(context);
        }

        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            using (drawContext.PushRenderTargetsAndRestore())
            {
                VoxelRenderer?.Draw(drawContext, ShadowMapRenderer_notPrivate);
            }

            base.DrawCore(context, drawContext);
        }

        protected override void DrawView(RenderContext context, RenderDrawContext drawContext)
        {
            base.DrawView(context, drawContext);

            // Voxel Debug if enabled
            if (VoxelVisualization != null)
            {
                VoxelVisualization.VoxelRenderer = VoxelRenderer;
                VoxelVisualization.Draw(drawContext, viewOutputTarget);
            }
        }
    }
}
