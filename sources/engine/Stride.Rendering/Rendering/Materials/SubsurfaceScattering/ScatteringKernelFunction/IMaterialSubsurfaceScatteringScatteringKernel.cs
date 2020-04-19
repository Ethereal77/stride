// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Mathematics;
using Xenko.Shaders;

namespace Xenko.Rendering.Materials
{
    /// <summary>
    /// Common interface for calculating the scattering profile applied during the forward pass using the subsurface scattering shading model.
    /// </summary>
    public interface IMaterialSubsurfaceScatteringScatteringKernel
    {
        /// <summary>
        /// Generates the scattering kernel that is fed into the SSS post-process.
        /// </summary>
        /// <returns>ShaderSource.</returns>
        Vector4[] Generate();
    }
}
