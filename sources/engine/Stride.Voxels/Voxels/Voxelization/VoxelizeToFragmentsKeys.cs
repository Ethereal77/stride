// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

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
