// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core.Mathematics;
using Xenko.Rendering.Lights;

namespace Xenko.Rendering.Images
{
    public struct RenderLightShaft
    {
        public RenderLight Light;
        public IDirectLight Light2;
        public int SampleCount;
        public float DensityFactor;
        public IReadOnlyList<RenderLightShaftBoundingVolume> BoundingVolumes;
        public bool SeparateBoundingVolumes;
    }

    public struct RenderLightShaftBoundingVolume
    {
        public Matrix World;
        public Model Model;
    }
}
