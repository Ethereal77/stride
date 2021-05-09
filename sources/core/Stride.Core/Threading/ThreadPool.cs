// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Threading;

using Stride.Core.Annotations;

namespace Stride.Core.Threading
{
    /// <summary>
    ///   Represents a pool of threads tuned for scheduling sub-millisecond actions.
    /// </summary>
    /// <remarks>
    ///   The implementation of <see cref="ThreadPool"/> is oriented towards low latency and very short
    ///   jobs (sub-millisecond). Try not to schedule long-running tasks in this pool.
    ///   <para/>
    ///   Also, this class can be instantiated. It generates less garbage than <see cref="System.Threading.ThreadPool"/>.
    /// </remarks>
    public sealed partial class ThreadPool : IDisposable
    {
        /// <summary>
        ///   The default instance that the whole process shares.
        /// </summary>
        /// <remarks>Use this instance where possible instead of creating new ones to avoid wasting process memory.</remarks>
        public static ThreadPool Instance = new ThreadPool();

        private static readonly bool isSingleCore = Environment.ProcessorCount < 2;

        /// <summary>
        ///   Gets a value that indicates if the current <see cref="Thread"/> is a worker thread of this <see cref="ThreadPool"/>.
        /// </summary>
        public static bool IsWorkedThread => isWorkedThread;
        [ThreadStatic]
        private static bool isWorkedThread;

        private readonly ConcurrentQueue<Action> workItems = new ConcurrentQueue<Action>();
        private readonly SemaphoreW semaphore;

        private long completionCounter;
        private int workScheduled, threadsBusy;
        private int disposing;
        private int leftToDispose;

        /// <summary>
        ///   Number of <see cref="Thread"/>s managed by this <see cref="ThreadPool"/>.
        /// </summary>
        public readonly int WorkerThreadsCount;

        /// <summary>
        ///   Gets the amount of work waiting to be processed by this <see cref="ThreadPool"/>.
        /// </summary>
        public int WorkScheduled => Volatile.Read(ref workScheduled);

        /// <summary>
        ///   Gets the amount of work completed by this <see cref="ThreadPool"/>.
        /// </summary>
        public ulong CompletedWork => (ulong) Volatile.Read(ref completionCounter);

        /// <summary>
        ///   Gets the amount of <see cref="Thread"/>s currently executing work items.
        /// </summary>
        public int ThreadsBusy => Volatile.Read(ref threadsBusy);


        /// <summary>
        ///   Initializes a new instance of the <see cref="ThreadPool"/> class.
        /// </summary>
        /// <param name="threadCount">
        ///   The number of worker threads to spawn. Specify <c>null</c> to decide the count based on the number of processors in the system.
        /// </param>
        public ThreadPool(int? threadCount = null)
        {
            semaphore = new SemaphoreW(spinCountParam: 70);

            WorkerThreadsCount =
                threadCount ??
                (isSingleCore ? 1 : Environment.ProcessorCount - 1);

            for (int i = 0; i < WorkerThreadsCount; i++)
                CreateWorkerThread();
        }


        /// <summary>
        ///   Adds an action to the work queue to run on one of the available threads.
        /// </summary>
        /// <param name="workItem">The action that represents the work item to execute.</param>
        /// <param name="amount">Specifies how many times to execute the <paramref name="workItem"/>.</param>
        /// <remarks>
        ///   It is strongly recommended that the action takes less than a millisecond.
        /// </remarks>
        public void QueueWorkItem([NotNull, Pooled] Action workItem, int amount = 1)
        {
            // Throw right here to help debugging
            if (workItem is null)
                throw new NullReferenceException(nameof(workItem));
            if (amount < 1)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (disposing > 0)
                throw new ObjectDisposedException(ToString());

            Interlocked.Add(ref workScheduled, amount);
            for (int i = 0; i < amount; i++)
            {
                PooledDelegateHelper.AddReference(workItem);
                workItems.Enqueue(workItem);
            }

            semaphore.Release(amount);
        }

        /// <summary>
        ///   Attempts to steal work from the pool to execute it from the current <see cref="Thread"/>.
        /// </summary>
        /// <remarks>
        ///   When doing a blocking wait inside one of this pool's <see cref="Thread"/>s, it is
        ///   recommended to wait doing a busy loop by repeatedly calling this method. It will
        ///   help with the performance of the system.
        /// </remarks>
        public bool TryCooperate()
        {
            if (workItems.TryDequeue(out var workItem))
            {
                Interlocked.Increment(ref threadsBusy);
                Interlocked.Decrement(ref workScheduled);

                try
                {
                    workItem.Invoke();
                }
                finally
                {
                    PooledDelegateHelper.Release(workItem);

                    Interlocked.Decrement(ref threadsBusy);
                    Interlocked.Increment(ref completionCounter);
                }

                return true;
            }

            return false;
        }

        private void CreateWorkerThread()
        {
            var thread = new Thread(WorkerThreadLoop)
            {
                Name = $"{GetType().FullName} worker thread",
                IsBackground = true,
                Priority = ThreadPriority.Highest,
            };

            thread.Start();
        }

        private void WorkerThreadLoop()
        {
            // Mark the ThreadLocal to tell this is a worker
            isWorkedThread = true;

            try
            {
                do
                {
                    while (TryCooperate()) { }

                    semaphore.Wait();

                    if (disposing > 0)
                        return;

                } while (true);
            }
            finally
            {
                if (disposing == 0)
                {
                    // If this thread is aborted or errored, spin up a new thread prior to stopping
                    CreateWorkerThread();
                }
                else
                {
                    Interlocked.Decrement(ref leftToDispose);
                }
            }
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref disposing, 1, 0) == 1)
                // Already disposing
                return;

            semaphore.Release(WorkerThreadsCount);

            while (Volatile.Read(ref leftToDispose) != 0)
            {
                if (semaphore.SignalCount == 0)
                    semaphore.Release(1);

                Thread.Yield();
            }

            // Finish any work left
            while (TryCooperate()) { }
        }
    }
}
