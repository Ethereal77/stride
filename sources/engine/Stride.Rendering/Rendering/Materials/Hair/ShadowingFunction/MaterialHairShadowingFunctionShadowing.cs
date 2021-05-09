// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Shaders;

namespace Stride.Rendering.Materials
{
    /// <summary>
    /// Shadows the hair using traditional shadow mapping.
    /// The hair will be treated as an opaque surface.
    /// </summary>
    /// <userdoc>
    /// Shadows the hair using traditional shadow mapping.
    /// The hair will be treated as an opaque surface.
    /// </userdoc>
    [DataContract("MaterialHairShadowingFunctionShadowing")]
    [Display("Shadowing")]
    public class MaterialHairShadowingFunctionShadowing : IMaterialHairShadowingFunction
    {
        public ShaderSource Generate(MaterialGeneratorContext context)
        {
            return new ShaderClassSource("MaterialHairShadowingFunctionShadowing");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is MaterialHairShadowingFunctionShadowing;
        }

        public override int GetHashCode()
        {
            return typeof(MaterialHairShadowingFunctionShadowing).GetHashCode();
        }
    }
}
