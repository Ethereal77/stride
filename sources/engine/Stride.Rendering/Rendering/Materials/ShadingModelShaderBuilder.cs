// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Shaders;

namespace Stride.Rendering.Materials
{
    /// <summary>
    /// Helper class to build the <see cref="ShaderSource"/> for the shading model of a <see cref="IMaterialShadingModelFeature"/>.
    /// </summary>
    public class ShadingModelShaderBuilder
    {
        public List<ShaderSource> ShaderSources { get; } = new List<ShaderSource>();

        /// <summary>
        /// Shaders that needs to be mixed on top of MaterialSurfaceLightingAndShading.
        /// </summary>
        public List<ShaderClassSource> LightDependentExtraModels { get; } = new List<ShaderClassSource>();

        public ShaderSource LightDependentSurface { get; set; }
    }
}
