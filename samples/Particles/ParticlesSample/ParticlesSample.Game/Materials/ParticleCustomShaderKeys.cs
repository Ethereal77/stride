// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.Shaders;

//namespace Stride.Rendering
namespace Stride.Rendering
{
    public partial class ParticleCustomShaderKeys
    {
        public static readonly PermutationParameterKey<ShaderSource> BaseColor = ParameterKeys.NewPermutation<ShaderSource>();
        public static readonly ObjectParameterKey<Texture> EmissiveMap = ParameterKeys.NewObject<Texture>();
        public static readonly ValueParameterKey<Color4> EmissiveValue = ParameterKeys.NewValue<Color4>();

        public static readonly PermutationParameterKey<ShaderSource> BaseIntensity = ParameterKeys.NewPermutation<ShaderSource>();
        public static readonly ObjectParameterKey<Texture> IntensityMap = ParameterKeys.NewObject<Texture>();
        public static readonly ValueParameterKey<float> IntensityValue = ParameterKeys.NewValue<float>();
    }
}
