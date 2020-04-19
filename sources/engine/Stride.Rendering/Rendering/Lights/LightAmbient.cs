// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Engine;

namespace Xenko.Rendering.Lights
{
    /// <summary>
    /// An ambient light.
    /// </summary>
    [DataContract("LightAmbient")]
    [Display("Ambient")]
    public class LightAmbient : ColorLightBase
    {
        public override bool Update(RenderLight light)
        {
            return true;
        }
    }
}
