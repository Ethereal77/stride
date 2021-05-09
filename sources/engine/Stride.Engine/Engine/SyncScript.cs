// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Collections;
using Stride.Core.MicroThreading;

namespace Stride.Engine
{
    /// <summary>
    /// A script whose <see cref="Update"/> will be called every frame.
    /// </summary>
    public abstract class SyncScript : StartupScript
    {
        internal PriorityQueueNode<SchedulerEntry> UpdateSchedulerNode;

        /// <summary>
        /// Called every frame.
        /// </summary>
        public abstract void Update();
    }
}
