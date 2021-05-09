// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    [DataContract(DefaultMemberMode = DataMemberMode.Default)]
    [Display("Hemisphere (6)")]
    public class VoxelMarchSetHemisphere6 : VoxelMarchSetBase, IVoxelMarchSet
    {
        public VoxelMarchSetHemisphere6() { }

        public VoxelMarchSetHemisphere6(IVoxelMarchMethod marcher)
        {
            Marcher = marcher;
        }


        public ShaderSource GetMarchingShader(int attrID)
        {
            var mixin = new ShaderMixinSource();
            mixin.Mixins.Add(new ShaderClassSource("VoxelMarchSetHemisphere6"));
            mixin.AddComposition("Marcher", Marcher.GetMarchingShader(attrID));

            return mixin;
        }

        public override void UpdateMarchingLayout(string compositionName)
        {
            base.UpdateMarchingLayout(compositionName);
            OffsetKey = VoxelMarchSetHemisphere6Keys.offset.ComposeWith(compositionName);
        }
    }
}
