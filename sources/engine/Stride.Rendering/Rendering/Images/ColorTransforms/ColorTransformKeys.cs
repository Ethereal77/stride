// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering.Images
{
    /// <summary>
    /// Parameter keys used by <see cref="ColorTransformBase"/>
    /// </summary>
    internal static class ColorTransformKeys
    {
        /// <summary>
        /// A boolean indicating wheter a <see cref="ColorTransformBase"/> is active or not.
        /// </summary>
        public static readonly PermutationParameterKey<bool> Enabled = ParameterKeys.NewPermutation(true);

        /// <summary>
        /// The shader used by <see cref="ColorTransformBase"/>.
        /// </summary>
        public static readonly PermutationParameterKey<string> Shader = ParameterKeys.NewPermutation("ColorTransformShader");

        /// <summary>
        /// The shader used by <see cref="ColorTransformBase"/>.
        /// </summary>
        public static readonly PermutationParameterKey<object[]> GenericArguments = ParameterKeys.NewPermutation((object[])null);
    }
}
