// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    public class VoxelizationPass
    {
        public RenderView view;
        public IVoxelStorer storer;
        public IVoxelizationMethod method;

        // Stage 1
        public List<VoxelAttribute> AttributesTemp = new List<VoxelAttribute>();
        public List<VoxelAttribute> AttributesDirect = new List<VoxelAttribute>();
        public List<VoxelAttribute> AttributesIndirect = new List<VoxelAttribute>();

        public ShaderSource source;

        public bool requireShadows = false;
        public RenderStage renderStage = null;


        public void Add(VoxelAttribute attr, VoxelizationStage stage, bool output, bool shadows)
        {
            if (stage == VoxelizationStage.Initial)
            {
                if (output)
                    AttributesDirect.Add(attr);
                else
                    AttributesTemp.Add(attr);
            }
            else if (stage == VoxelizationStage.Post)
            {
                AttributesIndirect.Add(attr);
            }

            requireShadows |= shadows;
        }
    }
}
