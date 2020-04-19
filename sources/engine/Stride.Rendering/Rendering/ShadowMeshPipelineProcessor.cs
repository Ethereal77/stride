// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ComponentModel;

using Xenko.Graphics;

namespace Xenko.Rendering
{
    /// <summary>
    /// Pipline processor for <see cref="RenderMesh"/> that cast shadows, to properly disable culling and depth clip.
    /// </summary>
    public class ShadowMeshPipelineProcessor : PipelineProcessor
    {
        public RenderStage ShadowMapRenderStage { get; set; }

        [DefaultValue(false)]
        public bool DepthClipping { get; set; } = false;

        public override void Process(RenderNodeReference renderNodeReference, ref RenderNode renderNode, RenderObject renderObject, PipelineStateDescription pipelineState)
        {
            // Objects in the shadow map render stage disable culling and depth clip
            if (renderNode.RenderStage == ShadowMapRenderStage)
            {
                pipelineState.RasterizerState = new RasterizerStateDescription(CullMode.None) { DepthClipEnable = DepthClipping };
            }
        }
    }
}
