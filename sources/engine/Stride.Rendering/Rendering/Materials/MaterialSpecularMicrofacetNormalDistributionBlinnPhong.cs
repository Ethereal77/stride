// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Shaders;

namespace Xenko.Rendering.Materials
{
    /// <summary>
    /// The Blinn-Phong Normal Distribution.
    /// </summary>
    [DataContract("MaterialSpecularMicrofacetNormalDistributionBlinnPhong")]
    [Display("Blinn-Phong")]
    public class MaterialSpecularMicrofacetNormalDistributionBlinnPhong : IMaterialSpecularMicrofacetNormalDistributionFunction
    {
        public ShaderSource Generate(MaterialGeneratorContext context)
        {
            return new ShaderClassSource("MaterialSpecularMicrofacetNormalDistributionBlinnPhong");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is MaterialSpecularMicrofacetNormalDistributionBlinnPhong;
        }

        public override int GetHashCode()
        {
            return typeof(MaterialSpecularMicrofacetNormalDistributionBlinnPhong).GetHashCode();
        }
    }
}
