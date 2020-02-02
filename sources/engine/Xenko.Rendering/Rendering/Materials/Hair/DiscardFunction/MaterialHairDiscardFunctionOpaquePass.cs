// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Shaders;

namespace Xenko.Rendering.Materials
{
    /// <summary>
    /// </summary>
    [DataContract("MaterialHairDiscardFunctionOpaquePass")]
    [Display("MaterialHairDiscardFunctionOpaquePass")]
    public class MaterialHairDiscardFunctionOpaquePass : IMaterialHairDiscardFunction
    {
        public ShaderSource Generate(MaterialGeneratorContext context, ValueParameterKey<float> uniqueAlphaThresholdKey)
        {
            return new ShaderClassSource("MaterialHairDiscardFunctionOpaquePass", uniqueAlphaThresholdKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is MaterialHairDiscardFunctionOpaquePass;
        }

        public override int GetHashCode()
        {
            return typeof(MaterialHairDiscardFunctionOpaquePass).GetHashCode();
        }
    }
}
