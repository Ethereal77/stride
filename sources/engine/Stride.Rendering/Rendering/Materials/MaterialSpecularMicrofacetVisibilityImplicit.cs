// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Shaders;

namespace Stride.Rendering.Materials
{
    /// <summary>
    /// Implicit Geometric Shadowing.
    /// </summary>
    [DataContract("MaterialSpecularMicrofacetVisibilityImplicit")]
    [Display("Implicit")]
    public class MaterialSpecularMicrofacetVisibilityImplicit : IMaterialSpecularMicrofacetVisibilityFunction
    {
        public ShaderSource Generate(MaterialGeneratorContext context)
        {
            return new ShaderClassSource("MaterialSpecularMicrofacetVisibilityImplicit");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is MaterialSpecularMicrofacetVisibilityImplicit;
        }

        public override int GetHashCode()
        {
            return typeof(MaterialSpecularMicrofacetVisibilityImplicit).GetHashCode();
        }
    }
}
