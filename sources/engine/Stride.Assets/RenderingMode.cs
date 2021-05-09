// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Assets
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
