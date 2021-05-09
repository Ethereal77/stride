// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Shaders;

namespace Stride.Rendering.Materials
{
    /// <summary>
    /// </summary>
    [DataContract("MaterialHairDiscardFunctionTransparentPass")]
    [Display("MaterialHairDiscardFunctionTransparentPass")]
    public class MaterialHairDiscardFunctionTransparentPass : IMaterialHairDiscardFunction
    {
        public ShaderSource Generate(MaterialGeneratorContext context, ValueParameterKey<float> uniqueAlphaThresholdKey)
        {
            return new ShaderClassSource("MaterialHairDiscardFunctionTransparentPass", uniqueAlphaThresholdKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is MaterialHairDiscardFunctionTransparentPass;
        }

        public override int GetHashCode()
        {
            return typeof(MaterialHairDiscardFunctionTransparentPass).GetHashCode();
        }
    }
}
