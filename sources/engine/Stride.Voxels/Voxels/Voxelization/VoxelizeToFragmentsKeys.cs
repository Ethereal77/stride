// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    public static partial class VoxelizeToFragmentsKeys
    {
        public static readonly PermutationParameterKey<ShaderSource> Storage = ParameterKeys.NewPermutation<ShaderSource>();
        public static readonly PermutationParameterKey<bool> RequireGeometryShader = ParameterKeys.NewPermutation<bool>();
        public static readonly PermutationParameterKey<int> GeometryShaderMaxVertexCount = ParameterKeys.NewPermutation<int>();
    }
}
