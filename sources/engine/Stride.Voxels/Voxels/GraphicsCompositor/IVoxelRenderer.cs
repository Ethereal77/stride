// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Rendering.Shadows;

namespace Stride.Rendering.Voxels
{
    public interface IVoxelRenderer
    {
        void Collect(RenderContext Context, IShadowMapRenderer ShadowMapRenderer);


        void Draw(RenderDrawContext drawContext, IShadowMapRenderer ShadowMapRenderer);

        Dictionary<VoxelVolumeComponent, ProcessedVoxelVolume> GetProcessedVolumes();
    }
}
