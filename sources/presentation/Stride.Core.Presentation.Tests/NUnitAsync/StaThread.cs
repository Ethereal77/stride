// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NUnitAsync
{
    // Source: http://stackoverflow.com/a/18838117
    internal class StaThread
    {
        private Thread mStaThread;
        private IQueueReader<SendOrPostCallbackItem> mQueueConsumer;
        private readonly SynchronizationContext syncContext;

        private ManualResetEvent mStopEvent = new ManualResetEvent(false);


        internal StaThread(IQueueReader<SendOrPostCallbackItem> reader, SynchronizationContext syncContext)
        {
            mQueueConsumer = reader;
            this.syncContext = syncContext;
            mStaThread = new Thread(Run);
            mStaThread.Name = "STA Worker Thread";
            mStaThread.SetApartmentState(ApartmentState.STA);
        }

        internal void Start()
        {
            mStaThread.Start();
        }


        internal void Join()
        {
            mStaThread.Join();
        }

        private void Run()
        {
            SynchronizationContext.SetSynchronizationContext(syncContext);
            while (true)
            {
                bool stop = mStopEvent.WaitOne(0);
                if (stop)
                {
                    mQueueConsumer.Dispose();
                    break;
                }

                SendOrPostCallbackItem workItem = mQueueConsumer.Dequeue();
                if (workItem != null)
                    workItem.Execute();
            }
        }

        internal void Stop()
        {
            mStopEvent.Set();
            mQueueConsumer.ReleaseReader();
        }
    }
}
