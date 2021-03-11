// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;
using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    [DataContract(DefaultMemberMode = DataMemberMode.Default)]
    [Display("Float16")]
    public class VoxelFragmentPackFloat16 : IVoxelFragmentPacker
    {
        ShaderSource source = new ShaderClassSource("VoxelFragmentPackFloat16");

        public ShaderSource GetShader() => source;

        public int GetBits(int channels) => channels * 16;
    }
}
