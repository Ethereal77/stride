// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Rendering.Colors;

namespace Xenko.Rendering.Lights
{
    /// <summary>
    /// Base interface for a light with a color
    /// </summary>
    public interface IColorLight : ILight
    {
        /// <summary>
        /// Gets or sets the light color.
        /// </summary>
        /// <value>The color.</value>
        IColorProvider Color { get; set; }

        /// <summary>
        /// Computes the color to the proper <see cref="ColorSpace"/> with the specified intensity.
        /// </summary>
        /// <param name="colorSpace"></param>
        /// <param name="intensity">The intensity.</param>
        /// <returns>Color3.</returns>
        Color3 ComputeColor(ColorSpace colorSpace, float intensity);
    }
}
