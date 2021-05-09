// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Shaders;

namespace Stride.Rendering.Materials
{
    /// <summary>
    /// Common interface for discarding pixels for the hair shading model.
    /// </summary>
    public interface IMaterialHairDiscardFunction
    {
        /// <summary>
        /// Generates the shader class source used for the shader composition.
        /// </summary>
        /// <returns>ShaderSource.</returns>
        ShaderSource Generate(MaterialGeneratorContext context, ValueParameterKey<float> uniqueAlphaThresholdKey);
    }
}
