// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Shaders;

namespace Xenko.Rendering.Materials
{
    /// <summary>
    /// Neumann Geometric Shadowing.
    /// </summary>
    [DataContract("MaterialSpecularMicrofacetVisibilityNeumann")]
    [Display("Neumann")]
    public class MaterialSpecularMicrofacetVisibilityNeumann : IMaterialSpecularMicrofacetVisibilityFunction
    {
        public ShaderSource Generate(MaterialGeneratorContext context)
        {
            return new ShaderClassSource("MaterialSpecularMicrofacetVisibilityNeumann");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is MaterialSpecularMicrofacetVisibilityNeumann;
        }

        public override int GetHashCode()
        {
            return typeof(MaterialSpecularMicrofacetVisibilityNeumann).GetHashCode();
        }
    }
}
