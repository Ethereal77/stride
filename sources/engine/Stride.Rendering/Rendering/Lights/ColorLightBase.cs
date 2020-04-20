// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering.Colors;

namespace Stride.Rendering.Lights
{
    /// <summary>
    /// Base implementation of <see cref="IColorLight"/>
    /// </summary>
    [DataContract(Inherited = true)]
    public abstract class ColorLightBase : IColorLight
    {
        protected ColorLightBase()
        {
            Color = new ColorRgbProvider();
        }

        /// <summary>
        /// Gets or sets the light color.
        /// </summary>
        /// <value>The color.</value>
        /// <userdoc>The color emitted by the light.</userdoc>
        [DataMember(-10)]
        [NotNull]
        public IColorProvider Color { get; set; }

        /// <summary>
        /// Computes the color with intensity, result is in linear space.
        /// </summary>
        /// <returns>Gets the color of this light in linear space.</returns>
        public Color3 ComputeColor(ColorSpace colorSpace, float intensity)
        {
            var color = (Color != null ? Color.ComputeColor() : new Color3(1.0f));
            color = color.ToColorSpace(colorSpace) * intensity;
            return color;
        }

        public abstract bool Update(RenderLight light);
    }
}
