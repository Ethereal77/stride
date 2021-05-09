// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Data;

namespace Stride.Audio
{
    [DataContract]
    [Display("Audio")]
    public class AudioEngineSettings : Configuration
    {
        /// <summary>
        /// Enables HRTF audio. Note that only audio emitters with HRTF enabled produce HRTF audio
        /// </summary>
        /// <userdoc>
        /// Enables HRTF audio. Note that only audio emitters with HRTF enabled produce HRTF audio
        /// </userdoc>
        [DataMember(10)]
        [Display("HRTF (if available)")]
        public bool HrtfSupport { get; set; }
    }
}
