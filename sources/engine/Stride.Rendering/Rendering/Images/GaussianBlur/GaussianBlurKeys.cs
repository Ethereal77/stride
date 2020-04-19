// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Linq;

namespace Xenko.Rendering.Images
{
    /// <summary>
    /// Keys used by <see cref="GaussianBlur"/> and GaussianBlurEffect xkfx
    /// </summary>
    internal static class GaussianBlurKeys
    {
        public static readonly PermutationParameterKey<int> Count = ParameterKeys.NewPermutation<int>();

        public static readonly PermutationParameterKey<bool> VerticalBlur = ParameterKeys.NewPermutation<bool>();
    }
}
