// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Graphics
{
    public enum SpriteFontType
    {
        /// <summary>
        /// Static font which has fixed font size and is pre-compiled
        /// </summary>
        /// <userdoc>
        /// Offline rasterized sprite font which has a fixed size in-game
        /// </userdoc>
        [Display("Offline Rasterized")]
        Static,

        /// <summary>
        /// Font which can change its font size dynamically and is compiled at runtime
        /// </summary>
        /// <userdoc>
        /// Runtime (in-game) rasterized sprite font which is also resizable
        /// </userdoc>
        [Display("Runtime Rasterized")]
        Dynamic,

        /// <summary>
        /// Signed Distance Field font which is pre-compiled but can still be scaled at runtime
        /// </summary>
        /// <userdoc>
        /// Offline rasterized sprite font which is resizable in-game
        /// </userdoc>
        [Display("Signed Distance Field")]
        SDF,
    }
}
