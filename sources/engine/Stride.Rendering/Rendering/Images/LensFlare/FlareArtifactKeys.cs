// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Linq;

namespace Stride.Rendering.Images
{
    /// <summary>
    /// Keys used by <see cref="FlareArtifact"/> and FlareArtifactEffect sdfx.
    /// </summary>
    internal static class FlareArtifactKeys
    {
        public static readonly PermutationParameterKey<int> Count = ParameterKeys.NewPermutation<int>();
    }
}
