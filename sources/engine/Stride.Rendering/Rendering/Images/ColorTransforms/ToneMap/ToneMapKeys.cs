// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering.Images
{
    internal static class ToneMapKeys
    {
        public static readonly PermutationParameterKey<bool> AutoExposure = ParameterKeys.NewPermutation(false);

        public static readonly PermutationParameterKey<bool> AutoKey = ParameterKeys.NewPermutation(false);

        public static readonly PermutationParameterKey<bool> UseLocalLuminance = ParameterKeys.NewPermutation(false);

        public static readonly PermutationParameterKey<ToneMapOperator> Operator = ParameterKeys.NewPermutation<ToneMapOperator>();
    }
}
