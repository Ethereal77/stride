// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering.SubsurfaceScattering
{
    internal class SubsurfaceScatteringKeys
    {
        public static readonly PermutationParameterKey<int> MaxMaterialCount = ParameterKeys.NewPermutation<int>();
        public static readonly PermutationParameterKey<bool> KernelSizeJittering = ParameterKeys.NewPermutation<bool>();
        public static readonly PermutationParameterKey<bool> BlurHorizontally = ParameterKeys.NewPermutation<bool>();
        public static readonly PermutationParameterKey<int> FollowSurface = ParameterKeys.NewPermutation<int>(1);
        public static readonly PermutationParameterKey<bool> OrthographicProjection = ParameterKeys.NewPermutation<bool>();
        public static readonly PermutationParameterKey<int> KernelLength = ParameterKeys.NewPermutation<int>();
        public static readonly PermutationParameterKey<int> RenderMode = ParameterKeys.NewPermutation<int>();
    }
}
