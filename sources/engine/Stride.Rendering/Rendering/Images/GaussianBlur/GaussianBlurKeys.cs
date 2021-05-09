// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Linq;

namespace Stride.Rendering.Images
{
    /// <summary>
    /// Keys used by <see cref="GaussianBlur"/> and GaussianBlurEffect sdfx
    /// </summary>
    internal static class GaussianBlurKeys
    {
        public static readonly PermutationParameterKey<int> Count = ParameterKeys.NewPermutation<int>();

        public static readonly PermutationParameterKey<bool> VerticalBlur = ParameterKeys.NewPermutation<bool>();
    }
}
