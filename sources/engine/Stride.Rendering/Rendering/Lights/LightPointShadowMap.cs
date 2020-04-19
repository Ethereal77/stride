// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ComponentModel;

using Xenko.Core;

namespace Xenko.Rendering.Lights
{
    /// <summary>
    /// A standard shadow map.
    /// </summary>
    [DataContract("LightPointShadowMap")]
    [Display("Point ShadowMap")]
    public sealed class LightPointShadowMap : LightShadowMap
    {
        /// <summary>
        /// The type of shadow mapping technique to use for this point light
        /// </summary>
        [DefaultValue(LightPointShadowMapType.CubeMap)]
        public LightPointShadowMapType Type { get; set; } = LightPointShadowMapType.CubeMap;
    }
}
