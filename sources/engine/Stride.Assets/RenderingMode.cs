// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Assets
{
    /// <summary>
    /// A rendering mode of Preview and Thumbnail for a game.
    /// </summary>
    [DataContract("RenderingMode")]
    public enum RenderingMode
    {
        /// <summary>
        /// The preview and thumbnail will use a low dynamic range settings when displaying assets.
        /// </summary>
        /// <userdoc>The preview and thumbnail will use a low dynamic range settings when displaying assets.</userdoc>
        [Display("Low Dynamic Range")]
        LDR,

        /// <summary>
        /// The preview and thumbnail will use a high dynamic range settings when displaying assets.
        /// </summary>
        /// <userdoc>The preview and thumbnail will use a high dynamic range settings when displaying assets.</userdoc>
        [Display("High Dynamic Range")]
        HDR,
    }
}
