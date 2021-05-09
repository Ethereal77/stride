// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.ComponentModel;

using Stride.Core;

namespace Stride.Rendering.Lights
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
