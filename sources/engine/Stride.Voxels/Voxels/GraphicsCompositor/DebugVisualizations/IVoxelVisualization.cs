// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Rendering.Images;

namespace Stride.Rendering.Voxels.Debug
{
    public interface IVoxelVisualization
    {
        ImageEffectShader GetShader(RenderDrawContext context, VoxelAttribute attr);
    }
}
