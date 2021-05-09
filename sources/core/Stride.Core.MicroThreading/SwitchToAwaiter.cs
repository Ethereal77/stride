// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Stride.Core.MicroThreading
{
    public class SwitchToAwaiter : INotifyCompletion
    {
        private readonly Scheduler scheduler;

        private MicroThread microThread;

        public bool IsCompleted => false;


        public SwitchToAwaiter(Scheduler scheduler)
        {
            this.scheduler = scheduler;
            this.microThread = null;
        }


        public void OnCompleted(Action continuation)
        {
            microThread = scheduler.Add(() =>
            {
                continuation();
                return Task.FromResult(true);
            });
        }

        public IDisposable GetResult() => new SwitchMicroThread(microThread);

        public SwitchToAwaiter GetAwaiter() => this;

        private struct SwitchMicroThread : IDisposable
        {
            private readonly MicroThread microThread;

            public SwitchMicroThread(MicroThread microThread)
            {
                this.microThread = microThread;
                //microThread.SynchronizationContext.IncrementTaskCount();
            }

            public void Dispose()
            {
                //microThread.SynchronizationContext.DecrementTaskCount();
            }
        }
    }
}
