// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering
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
