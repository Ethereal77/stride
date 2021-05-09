// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Shaders;

namespace Stride.Rendering
{
    public static class StrideEffectBaseKeys
    {
        public static readonly PermutationParameterKey<ShaderSource> ExtensionPostVertexStageShader = ParameterKeys.NewPermutation<ShaderSource>();
        public static readonly PermutationParameterKey<ShaderSource> ComputeVelocityShader = ParameterKeys.NewPermutation<ShaderSource>();
        public static readonly PermutationParameterKey<ShaderSource> RenderTargetExtensions = ParameterKeys.NewPermutation<ShaderSource>();

        public static readonly PermutationParameterKey<bool> HasInstancing = ParameterKeys.NewPermutation<bool>();
        public static readonly PermutationParameterKey<int> ModelTransformUsage = ParameterKeys.NewPermutation<int>();
    }
}
