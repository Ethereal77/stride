// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Graphics;

namespace Stride.Rendering
{
    /// <summary>
    /// Represents an single render operation of a <see cref="RenderObject"/> from a specific view with a specific effect, with attached properties.
    /// </summary>
    public struct RenderNode
    {
        /// <summary>
        /// Underlying render object.
        /// </summary>
        public readonly RenderObject RenderObject;

        /// <summary>
        /// View used when rendering. This is usually a frustum and some camera parameters.
        /// </summary>
        public readonly RenderView RenderView;

        /// <summary>
        /// Contains parameters specific to this object in the current view.
        /// </summary>
        public readonly ViewObjectNodeReference ViewObjectNode;

        /// <summary>
        /// Contains parameters specific to this object with the current effect.
        /// </summary>
        public EffectObjectNodeReference EffectObjectNode;

        /// <summary>
        /// The "PerDraw" resources.
        /// </summary>
        public ResourceGroup Resources;

        /// <summary>
        /// The render stage.
        /// </summary>
        public RenderStage RenderStage;

        /// <summary>
        /// The render effect.
        /// </summary>
        public RenderEffect RenderEffect;

        public RenderNode(RenderObject renderObject, RenderView renderView, ViewObjectNodeReference viewObjectNode, RenderStage renderStage)
        {
            RenderObject = renderObject;
            RenderView = renderView;
            ViewObjectNode = viewObjectNode;
            EffectObjectNode = EffectObjectNodeReference.Invalid;
            RenderStage = renderStage;
            RenderEffect = null;
            Resources = null;
        }
    }
}
