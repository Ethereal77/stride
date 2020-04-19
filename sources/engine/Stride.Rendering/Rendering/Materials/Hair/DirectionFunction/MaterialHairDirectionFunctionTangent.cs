// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Core.Annotations;
using Xenko.Shaders;

namespace Xenko.Rendering.Materials
{
    /// <summary>
    /// Uses the tangent vector as the hair direction.
    /// </summary>
    [DataContract("MaterialHairDirectionFunctionTangent")]
    [Display("Tangent")]
    public class MaterialHairDirectionFunctionTangent : IMaterialHairDirectionFunction
    {
        public ShaderSource Generate(MaterialGeneratorContext context)
        {
            return new ShaderClassSource("MaterialHairDirectionFunctionTangent");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is MaterialHairDirectionFunctionTangent;
        }

        public override int GetHashCode()
        {
            return typeof(MaterialHairDirectionFunctionTangent).GetHashCode();
        }
    }
}
