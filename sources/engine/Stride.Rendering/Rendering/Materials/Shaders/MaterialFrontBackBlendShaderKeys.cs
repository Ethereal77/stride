// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Rendering
{
    public partial class MaterialFrontBackBlendShaderKeys
    {
        public static readonly PermutationParameterKey<bool> UseNormalBackFace = ParameterKeys.NewPermutation<bool>(true);
    }
}
