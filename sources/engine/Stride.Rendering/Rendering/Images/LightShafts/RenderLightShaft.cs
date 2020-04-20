// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Stride.Core.Mathematics;
using Stride.Rendering.Lights;

namespace Stride.Rendering.Images
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
