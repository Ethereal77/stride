// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Data;

namespace Stride.Streaming
{
    [DataContract]
    [Display("Streaming")]
    public sealed class StreamingSettings : Configuration
    {
        /// <summary>
        /// Gets or sets a value indicating whether resource streaming should be disabled.
        /// </summary>
        /// <userdoc>
        /// Enable streaming
        /// </userdoc>
        /// <seealso cref="Games.GameSystemBase.Enabled"/>
        [DataMember]
        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        /// <inheritdoc cref="StreamingManager.ManagerUpdatesInterval"/>
        /// <userdoc>
        /// How frequently Stride updates the streaming. Smaller intervals mean the streaming system reacts faster, but use more CPU and cause more memory fluctuations.
        /// </userdoc>
        /// <seealso cref="StreamingManager.ManagerUpdatesInterval"/>
        [DataMember]
        [Display("Update interval")]
        [DataMemberRange(0.001, 3)]
        public TimeSpan ManagerUpdatesInterval { get; set; } = TimeSpan.FromMilliseconds(33);

        /// <inheritdoc cref="StreamingManager.MaxResourcesPerUpdate"/>
        /// <userdoc>
        /// The maximum number of textures loaded or unloaded per streaming update. Higher numbers reduce pop-in but might slow down the framerate.
        /// </userdoc>
        /// <seealso cref="StreamingManager.MaxResourcesPerUpdate"/>
        [DataMember]
        [Display("Max resources per update")]
        [DataMemberRange(1, 0)]
        [DefaultValue(8)]
        public int MaxResourcesPerUpdate { get; set; } = 8;

        /// <inheritdoc cref="StreamingManager.ResourceLiveTimeout"/>
        /// <userdoc>
        /// How long resources stay loaded after they're no longer used (when the memory budget is exceeded)
        /// </userdoc>
        /// <seealso cref="StreamingManager.ResourceLiveTimeout"/>
        [DataMember]
        [Display("Resource timeout (ms)")]
        [DataMemberRange(0, 3)]
        public TimeSpan ResourceLiveTimeout { get; set; } = TimeSpan.FromSeconds(8);

        /// <inheritdoc cref="StreamingManager.TargetedMemoryBudget"/>
        /// <userdoc>
        /// When the memory used by streaming exceeds this budget, Stride unloads unused textures. You can increase this to keep more textures loaded when you have memory to spare, and vice versa.
        /// </userdoc>
        /// <seealso cref="StreamingManager.TargetedMemoryBudget"/>
        [DataMember]
        [Display("Memory budget (in MB)")]
        [DataMemberRange(0, 0)]
        [DefaultValue(512)]
        public int TargetedMemoryBudget { get; set; } = 512;
    }
}
