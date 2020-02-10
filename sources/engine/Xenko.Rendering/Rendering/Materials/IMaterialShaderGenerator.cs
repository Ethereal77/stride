// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering.Materials
{
    /// <summary>
    /// Defines the interface to generate the shaders for a <see cref="IMaterialFeature"/>
    /// </summary>
    public interface IMaterialShaderGenerator
    {
        /// <summary>
        /// Generates the shader.
        /// </summary>
        /// <param name="context">The context.</param>
        void Visit(MaterialGeneratorContext context);
    }
}
