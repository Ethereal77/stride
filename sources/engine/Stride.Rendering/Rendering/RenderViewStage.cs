// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Collections;
using Stride.Core.Threading;

namespace Stride.Rendering
{
    /// <summary>
    /// Stage-specific data for a <see cref="RenderView"/>.
    /// </summary>
    /// Mostly useful to store list of <see cref="RenderNode"/> prefiltered by a <see cref="Rendering.RenderStage"/> and a <see cref="RenderView"/>.
    public struct RenderViewStage
    {
        public readonly int Index;

        /// <summary>
        /// Invalid slot.
        /// </summary>
        public static readonly RenderViewStage Invalid = new RenderViewStage(-1);

        public RenderViewStage(int index)
        {
            Index = index;
            RenderNodes = null;
            SortedRenderNodes = null;
        }

        public RenderViewStage(RenderStage renderStage)
        {
            Index = renderStage.Index;
            RenderNodes = null;
            SortedRenderNodes = null;
        }

        /// <summary>
        /// List of render nodes. It might cover multiple RenderStage and RootRenderFeature. RenderStages contains RenderStage range information.
        /// Used mostly for sorting and rendering.
        /// </summary>
        public ConcurrentCollector<RenderNodeFeatureReference> RenderNodes;

        /// <summary>
        /// Sorted list of render nodes, that should be used during actual drawing.
        /// </summary>
        public FastList<RenderNodeFeatureReference> SortedRenderNodes;

        public static implicit operator RenderViewStage(RenderStage renderStage)
        {
            return new RenderViewStage(renderStage);
        }
    }
}
