// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering.Compositing
{
    /// <summary>
    /// Renders a single stage with the current <see cref="RenderView"/>.
    /// </summary>
    public partial class SingleStageRenderer : SceneRendererBase
    {
        public RenderStage RenderStage { get; set; }

        protected override void CollectCore(RenderContext context)
        {
            if (RenderStage == null)
                return;

            // Collect with current RenderView
            RenderStage.OutputValidator.Validate(ref context.RenderOutput);
            context.RenderView.RenderStages.Add(RenderStage);
        }

        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            if (RenderStage == null)
                return;

            // Draw with current RenderView
            drawContext.RenderContext.RenderSystem.Draw(drawContext, drawContext.RenderContext.RenderView, RenderStage);
        }
    }
}
