// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    public static partial class Voxel2x2x2MipmapKeys
    {
        public static readonly PermutationParameterKey<ShaderSource> mipmapper = ParameterKeys.NewPermutation<ShaderSource>();
    }
}
