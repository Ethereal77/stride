// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core;
using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    [DataContract(DefaultMemberMode = DataMemberMode.Default)]
    [Display("Isotropic (single)")]
    public class VoxelLayoutIsotropic : VoxelLayoutBase, IVoxelLayout
    {
        protected override int LayoutCount { get; set; } = 1;

        protected override ShaderClassSource Writer { get; set; } = new ShaderClassSource("VoxelIsotropicWriter_Float4");

        protected override ShaderClassSource Sampler { get; set; } = new ShaderClassSource("VoxelIsotropicSampler");

        protected override string ApplierKey { get; set; } = "Isotropic";


        public void UpdateVoxelizationLayout(string compositionName, List<VoxelModifierEmissionOpacity> modifiers)
        {
            DirectOutput = VoxelIsotropicWriter_Float4Keys.DirectOutput.ComposeWith(compositionName);
            BrightnessInvKey = VoxelIsotropicWriter_Float4Keys.maxBrightnessInv.ComposeWith(compositionName);
        }

        public void UpdateSamplingLayout(string compositionName)
        {
            BrightnessKey = VoxelIsotropicSamplerKeys.maxBrightness.ComposeWith(compositionName);
            storageTex.UpdateSamplingLayout("storage." + compositionName);
        }
    }
}
