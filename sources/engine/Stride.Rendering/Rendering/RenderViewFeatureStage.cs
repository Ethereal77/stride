// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering
{
    /// <summary>
    /// Describes a specific <see cref="RenderView"/>, <see cref="RootRenderFeature"/> and <see cref="RenderStage"/> combination.
    /// </summary>
    public struct RenderViewFeatureStage
    {
        public RenderStage RenderStage;

        public int RenderNodeStart;
        public int RenderNodeEnd;

        public RenderViewFeatureStage(RenderStage renderStage, int renderNodeStart, int renderNodeEnd)
        {
            RenderStage = renderStage;
            RenderNodeStart = renderNodeStart;
            RenderNodeEnd = renderNodeEnd;
        }
    }
}
