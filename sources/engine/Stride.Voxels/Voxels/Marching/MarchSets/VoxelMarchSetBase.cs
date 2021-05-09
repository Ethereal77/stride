// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Rendering.Voxels
{
    [DataContract(DefaultMemberMode = DataMemberMode.Default)]
    public class VoxelMarchSetBase
    {
        public IVoxelMarchMethod Marcher { set; get; } = new VoxelMarchConePerMipmap();

        public float Offset { set; get; } = 1.0f;


        public VoxelMarchSetBase() { }

        public VoxelMarchSetBase(IVoxelMarchMethod marcher)
        {
            Marcher = marcher;
        }


        protected ValueParameterKey<float> OffsetKey;

        public virtual void UpdateMarchingLayout(string compositionName)
        {
            Marcher.UpdateMarchingLayout("Marcher." + compositionName);
        }

        public virtual void ApplyMarchingParameters(ParameterCollection parameters)
        {
            Marcher.ApplyMarchingParameters(parameters);
            parameters.Set(OffsetKey, Offset);
        }
    }
}
