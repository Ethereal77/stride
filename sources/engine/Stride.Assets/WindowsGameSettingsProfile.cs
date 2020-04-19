// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core;
using Xenko.Graphics;

namespace Xenko.Assets
{
    /// <summary>
    /// Base settings for Windows profile.
    /// </summary>
    [DataContract("WindowsGameSettingsProfile")]
    public class WindowsGameSettingsProfile : GameSettingsProfileBase
    {
        public WindowsGameSettingsProfile()
        {
            GraphicsPlatform = GraphicsPlatform.Direct3D11;
        }

        public override IEnumerable<GraphicsPlatform> GetSupportedGraphicsPlatforms()
        {
            return new[] { GraphicsPlatform.Direct3D11 };
        }
    }
}
