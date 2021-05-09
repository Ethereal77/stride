// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    public partial class BufferToTextureKeys
    {
        public static readonly PermutationParameterKey<ShaderSourceCollection> AttributesIndirect = ParameterKeys.NewPermutation<ShaderSourceCollection>();
        public static readonly PermutationParameterKey<ShaderSourceCollection> AttributesTemp = ParameterKeys.NewPermutation<ShaderSourceCollection>();
        public static readonly PermutationParameterKey<ShaderSourceCollection> AttributeLocalSamples = ParameterKeys.NewPermutation<ShaderSourceCollection>();
        public static readonly PermutationParameterKey<string> IndirectReadAndStoreMacro = ParameterKeys.NewPermutation<string>();
        public static readonly PermutationParameterKey<string> IndirectStoreMacro = ParameterKeys.NewPermutation<string>();
    }
}
