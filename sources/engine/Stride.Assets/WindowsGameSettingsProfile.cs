// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core;
using Stride.Graphics;

namespace Stride.Assets
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
