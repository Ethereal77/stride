// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Rendering.Shadows;

namespace Stride.Rendering.Lights
{
    /// <summary>
    /// Interface for the shadow of a light.
    /// </summary>
    public interface ILightShadow
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ILightShadow"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool Enabled { get; set; }
    }
}
