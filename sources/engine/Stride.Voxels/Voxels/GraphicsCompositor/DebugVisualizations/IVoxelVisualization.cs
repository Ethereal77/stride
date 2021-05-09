// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using Stride.Rendering.Images;

namespace Stride.Rendering.Voxels.Debug
{
    public interface IVoxelVisualization
    {
        ImageEffectShader GetShader(RenderDrawContext context, VoxelAttribute attr);
    }
}
