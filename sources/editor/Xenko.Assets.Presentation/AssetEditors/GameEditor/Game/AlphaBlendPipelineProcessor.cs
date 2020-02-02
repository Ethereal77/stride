// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Graphics;
using Xenko.Rendering;

namespace Xenko.Assets.Presentation.AssetEditors.GameEditor.Game
{
    public class AlphaBlendPipelineProcessor : PipelineProcessor
    {
        public RenderStage RenderStage { get; set; }

        public override void Process(RenderNodeReference renderNodeReference, ref RenderNode renderNode, RenderObject renderObject, PipelineStateDescription pipelineState)
        {
            if (renderNode.RenderStage == RenderStage)
            {
                pipelineState.BlendState = BlendStates.AlphaBlend;
                pipelineState.DepthStencilState = DepthStencilStates.DepthRead;
            }
        }
    }
}
