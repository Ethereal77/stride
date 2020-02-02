// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ComponentModel;

using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Shaders;

namespace Xenko.Rendering.Materials
{
    /// <summary>
    /// Calculates the scattering profile for skin, which is applied during
    /// the forward pass using the subsurface scattering shading model.
    /// It also calculates a scattering kernel based on the "Falloff" and "Strength" parameters.
    /// 
    /// </summary>
    [Display("Skin")]
    [DataContract("MaterialSubsurfaceScatteringScatteringProfileSkin")]
    public class MaterialSubsurfaceScatteringScatteringProfileSkin : IMaterialSubsurfaceScatteringScatteringProfile
    {
        public ShaderSource Generate(MaterialGeneratorContext context)
        {
            return new ShaderClassSource("MaterialSubsurfaceScatteringScatteringProfileSkin");
        }
    }
}
