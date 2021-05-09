// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stride.Engine.Events
{
    /// <summary>
    /// Simple passthru scheduler to avoid the default dataflow TaskScheduler.Default usage
    /// This also makes sure we fire events at proper required order/timing
    /// </summary>
    internal class EventTaskScheduler : TaskScheduler
    {
        public static readonly EventTaskScheduler Scheduler = new EventTaskScheduler();

        protected override void QueueTask(Task task)
        {
            TryExecuteTask(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return null;
        }
    }
}
