// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Graphics;

namespace Stride.Rendering
{
    public class WireframePipelineProcessor : PipelineProcessor
    {
        public RenderStage RenderStage { get; set; }

        public override void Process(RenderNodeReference renderNodeReference, ref RenderNode renderNode, RenderObject renderObject, PipelineStateDescription pipelineState)
        {
            if (renderNode.RenderStage == RenderStage)
            {
                pipelineState.RasterizerState = RasterizerStates.Wireframe;
            }
        }
    }
}
