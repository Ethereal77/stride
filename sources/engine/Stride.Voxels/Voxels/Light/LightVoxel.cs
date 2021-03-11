// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;
using Stride.Rendering.Lights;

namespace Stride.Rendering.Voxels.VoxelGI
{
    /// <summary>
    ///   An environment light based on a voxel representation.
    /// </summary>
    [Display("Voxel")]
    [DataContract("LightVoxel")]
    public class LightVoxel : IEnvironmentLight
    {
        [DataMember(1)]
        public VoxelVolumeComponent Volume { get; set; }

        [DataMember(10)]
        public int AttributeIndex { get; set; } = 0;


        [DataMember(20)]
        public IVoxelMarchSet DiffuseMarcher { get; set; } = new VoxelMarchSetHemisphere6(new VoxelMarchConePerMipmap());

        [DataMember(30)]
        public IVoxelMarchMethod SpecularMarcher { get; set; } = new VoxelMarchCone(30, 0.5f, 1.0f);

        [DataMember(40)]
        public float BounceIntensityScale { get; set; }

        [DataMember(50)]
        public float SpecularIntensityScale { get; set; }


        public bool Update(RenderLight light) => true;
    }
}
