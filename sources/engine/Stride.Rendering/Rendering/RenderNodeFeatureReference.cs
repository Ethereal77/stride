// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering
{
    public struct RenderNodeFeatureReference
    {
        public readonly RootRenderFeature RootRenderFeature;
        public readonly RenderNodeReference RenderNode;
        public readonly RenderObject RenderObject;

        public RenderNodeFeatureReference(RootRenderFeature rootRenderFeature, RenderNodeReference renderNode, RenderObject renderObject)
        {
            RootRenderFeature = rootRenderFeature;
            RenderNode = renderNode;
            RenderObject = renderObject;
        }
    }
}
