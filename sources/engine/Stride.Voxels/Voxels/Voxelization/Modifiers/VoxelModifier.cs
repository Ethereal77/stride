// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;

using Stride.Core;
using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    [DataContract(DefaultMemberMode = DataMemberMode.Default)]
    public abstract class VoxelModifier
    {
        [DataMember(-20)]
        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;


        public virtual  bool RequiresColumns() => false;

        public abstract void CollectAttributes(List<AttributeStream> attributes, VoxelizationStage stage, bool output);

        public abstract void UpdateVoxelizationLayout(string compositionName);

        public abstract void ApplyVoxelizationParameters(ParameterCollection parameters);

        public abstract ShaderSource GetApplier(string layout);
    }
}
