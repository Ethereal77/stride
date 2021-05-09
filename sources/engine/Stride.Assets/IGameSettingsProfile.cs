// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Graphics;

namespace Stride.Assets
{
    /// <summary>
    /// Base interface for game settings for a particular profile
    /// </summary>
    public interface IGameSettingsProfile
    {
        /// <summary>
        /// Gets the GraphicsPlatform used by this profile.
        /// </summary>
        GraphicsPlatform GraphicsPlatform { get; }

        /// <summary>
        /// Gets the <see cref="GraphicsPlatform"/> list supported by this profile.
        /// </summary>
        /// <returns></returns>
        IEnumerable<GraphicsPlatform> GetSupportedGraphicsPlatforms();
    }
}
