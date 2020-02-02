// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Diagnostics;

namespace Xenko.Core.MicroThreading
{
    /// <summary>
    /// Either a microthread or an action with priority.
    /// </summary>
    internal struct SchedulerEntry : IComparable<SchedulerEntry>
    {
        public Action Action;
        public MicroThread MicroThread;
        public long Priority;
        public long SchedulerCounter;
        public object Token;
        public ProfilingKey ProfilingKey;

        public SchedulerEntry(MicroThread microThread) : this()
        {
            MicroThread = microThread;
        }

        public SchedulerEntry(Action action, long priority) : this()
        {
            Action = action;
            Priority = priority;
        }

        public int CompareTo(SchedulerEntry other)
        {
            var priorityDiff = Priority.CompareTo(other.Priority);
            if (priorityDiff != 0)
                return priorityDiff;

            return SchedulerCounter.CompareTo(other.SchedulerCounter);
        }
    }
}
