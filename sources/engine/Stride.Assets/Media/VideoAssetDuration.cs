// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;
using System.ComponentModel;

namespace Xenko.Assets.Media
{
    /// <summary>
    /// Enable video trimming
    /// </summary>
    /// <userdoc>
    /// Trim the video by specifying the start and end times.
    /// </userdoc>
    [DataContract("VideoAssetDuration")]
    [Display("Trimming")]
    public struct VideoAssetDuration
    {
        [DefaultValue(false)]
        [DataMember(-5)]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the start time the video.
        /// </summary>
        /// <userdoc>
        /// Frames of the video before this time will be removed.
        /// </userdoc>
        [DataMember(2)]
        [Display("Start time")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time the video.
        /// </summary>
        /// <userdoc>
        /// Frames of the video after this time will be removed.
        /// </userdoc>
        [DataMember(4)]
        [Display("End time")]
        public TimeSpan EndTime { get; set; }
    }
}
