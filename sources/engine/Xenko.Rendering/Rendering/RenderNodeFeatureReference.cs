﻿// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering
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
