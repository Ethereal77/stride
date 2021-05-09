// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Rendering.ComputeEffect
{
    public class ComputeEffectShaderKeys
    {
        public static readonly PermutationParameterKey<string> ComputeShaderName = ParameterKeys.NewPermutation<string>();
        public static readonly PermutationParameterKey<Int3> ThreadNumbers = ParameterKeys.NewPermutation<Int3>();
    }
}
