// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Graphics;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents a render feature used inside another one (i.e. <see cref="MeshRenderFeature.RenderFeatures"/>).
    /// </summary>
    public abstract class SubRenderFeature : RenderFeature
    {
        /// <summary>
        ///   Gets the root render feature.
        /// </summary>
        protected RootRenderFeature RootRenderFeature;

        /// <summary>
        ///   Attaches this <see cref="SubRenderFeature"/> to a <see cref="Rendering.RootRenderFeature"/>.
        /// </summary>
        /// <param name="rootRenderFeature">The root render feature.</param>
        internal void AttachRootRenderFeature(RootRenderFeature rootRenderFeature)
        {
            RootRenderFeature = rootRenderFeature;
            RenderSystem = rootRenderFeature.RenderSystem;
        }

        /// <summary>
        ///   Does any changes required to the pipeline state by this render feature.
        /// </summary>
        /// <param name="context">Context object providing access to information and services.</param>
        /// <param name="renderNodeReference"></param>
        /// <param name="renderNode"></param>
        /// <param name="renderObject"></param>
        /// <param name="pipelineState"></param>
        public virtual void ProcessPipelineState(RenderContext context, RenderNodeReference renderNodeReference, ref RenderNode renderNode, RenderObject renderObject, PipelineStateDescription pipelineState) { }
    }
}
