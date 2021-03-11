// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

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
