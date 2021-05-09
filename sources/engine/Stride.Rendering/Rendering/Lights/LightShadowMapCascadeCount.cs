// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Rendering.Lights
{
    /// <summary>
    /// Number of cascades used for a shadow map.
    /// </summary>
    [DataContract("LightShadowMapCascadeCount")]
    public enum LightShadowMapCascadeCount
    {
        /// <summary>
        /// A shadow map with one cascade.
        /// </summary>
        [Display("One Cascade")]
        OneCascade = 1,

        /// <summary>
        /// A shadow map with two cascades.
        /// </summary>
        [Display("Two Cascades")]
        TwoCascades = 2,

        /// <summary>
        /// A shadow map with four cascades.
        /// </summary>
        [Display("Four Cascades")]
        FourCascades = 4,
    }
}
