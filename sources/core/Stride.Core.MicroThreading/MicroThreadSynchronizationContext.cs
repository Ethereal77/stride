// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Threading;

namespace Stride.Core.MicroThreading
{
    public class MicroThreadSynchronizationContext : SynchronizationContext, IMicroThreadSynchronizationContext
    {
        private readonly MicroThread microThread;

        MicroThread IMicroThreadSynchronizationContext.MicroThread => microThread;


        public MicroThreadSynchronizationContext(MicroThread microThread)
        {
            this.microThread = microThread;
        }


        public override SynchronizationContext CreateCopy() => this;

        public override void Post(SendOrPostCallback callback, object state)
        {
            // There is two cases:
            //
            //   1) We are either in normal MicroThread inside Scheduler.Step() (CurrentThread test),
            //      in which case we will directly execute the callback to avoid further processing from scheduler.
            //      Also, note that Wait() sends us event that are supposed to come back into scheduler.
            //      NOTE: As it will end up on the callstack, it might be better to Schedule it instead (to avoid overflow)?
            //
            //   2) Otherwise, we just received an external task continuation (i.e. Task.Sleep()), or a microthread triggering
            //      another, so schedule it so that it comes back in our regular scheduler.

            if (microThread.Scheduler.RunningMicroThread == microThread)
            {
                callback(state);
            }
            else if (microThread.State == MicroThreadState.Completed)
            {
                throw new InvalidOperationException("MicroThread is already completed but still posting continuations.");
            }
            else
            {
                microThread.ScheduleContinuation(microThread.ScheduleMode, callback, state);
            }
        }
    }
}
