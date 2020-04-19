// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xenko.Engine.Events
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
