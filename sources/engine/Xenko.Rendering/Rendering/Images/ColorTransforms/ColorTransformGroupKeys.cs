// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

namespace Xenko.Rendering.Images
{
    /// <summary>
    /// Keys used by <see cref="ToneMap"/> and ToneMapEffect xkfx
    /// </summary>
    internal static class ColorTransformGroupKeys
    {
        public static readonly PermutationParameterKey<List<ColorTransform>> Transforms = ParameterKeys.NewPermutation<List<ColorTransform>>();
    }
}
