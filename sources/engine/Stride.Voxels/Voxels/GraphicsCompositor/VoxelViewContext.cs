// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Rendering.Voxels
{
    public struct VoxelViewContext
    {
        public bool IsVoxelView;

        public VoxelViewContext(VoxelizationPassList passes, int viewIndex)
        {
            IsVoxelView = false;

            foreach (var pass in passes.passes)
            {
                if (pass.view.Index == viewIndex)
                {
                    IsVoxelView = true;
                    break;
                }
            }
        }
        public VoxelViewContext(bool voxelView)
        {
            IsVoxelView = voxelView;
        }
    }
}
