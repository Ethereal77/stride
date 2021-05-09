// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering.Images
{
    internal static class ToneMapKeys
    {
        public static readonly PermutationParameterKey<bool> AutoExposure = ParameterKeys.NewPermutation(false);

        public static readonly PermutationParameterKey<bool> AutoKey = ParameterKeys.NewPermutation(false);

        public static readonly PermutationParameterKey<bool> UseLocalLuminance = ParameterKeys.NewPermutation(false);

        public static readonly PermutationParameterKey<ToneMapOperator> Operator = ParameterKeys.NewPermutation<ToneMapOperator>();
    }
}
