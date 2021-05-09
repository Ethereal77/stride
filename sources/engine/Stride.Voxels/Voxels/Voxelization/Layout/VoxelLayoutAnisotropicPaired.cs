// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core;
using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    [DataContract(DefaultMemberMode = DataMemberMode.Default)]
    [Display("Anisotropic (3 sided)")]
    public class VoxelLayoutAnisotropicPaired : VoxelLayoutBase, IVoxelLayout
    {
        protected override int LayoutCount { get; set; } = 3;

        protected override ShaderClassSource Writer { get; set; } = new ShaderClassSource("VoxelAnisotropicPairedWriter_Float4");

        protected override ShaderClassSource Sampler { get; set; } = new ShaderClassSource("VoxelAnisotropicPairedSampler");

        protected override string ApplierKey { get; set; } = "AnisotropicPaired";


        public void UpdateVoxelizationLayout(string compositionName, List<VoxelModifierEmissionOpacity> modifier)
        {
            DirectOutput = VoxelAnisotropicPairedWriter_Float4Keys.DirectOutput.ComposeWith(compositionName);
            BrightnessInvKey = VoxelAnisotropicPairedWriter_Float4Keys.maxBrightnessInv.ComposeWith(compositionName);
        }

        public void UpdateSamplingLayout(string compositionName)
        {
            BrightnessKey = VoxelAnisotropicPairedSamplerKeys.maxBrightness.ComposeWith(compositionName);
            storageTex.UpdateSamplingLayout("storage." + compositionName);
        }
    }
}
