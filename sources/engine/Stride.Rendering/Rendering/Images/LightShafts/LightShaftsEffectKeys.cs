// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Shaders;

namespace Xenko.Rendering.Images
{
    public static class LightShaftsEffectKeys
    {
        public static readonly PermutationParameterKey<ShaderSource> LightGroup = ParameterKeys.NewPermutation<ShaderSource>();
        public static readonly PermutationParameterKey<int> SampleCount = ParameterKeys.NewPermutation<int>();
    }
}
