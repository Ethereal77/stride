// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

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
    public sealed partial class ThreadPool
    {
        /// <summary>
        ///   The default instance that the whole process shares.
        /// </summary>
        /// <remarks>Use this instance where possible instead of creating new ones to avoid wasting process memory.</remarks>
        public static readonly ThreadPool Instance = new ThreadPool();

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


        public ThreadPool(int? threadCount = null)
        {
            WorkerThreadsCount =
                threadCount ??
                (Environment.ProcessorCount == 1 ? 1 : Environment.ProcessorCount - 1);

            for (int i = 0; i < WorkerThreadsCount; i++)
                CreateWorkerThread();

            // TODO: Benchmark this on multiple computers at different work frequency
            const int SpinDuration = 140;
            semaphore = new SemaphoreW(0, SpinDuration);
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
                Name = $"{GetType().FullName} thread",
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
                while (true)
                {
                    while (TryCooperate()) { }

                    semaphore.Wait();
                }
            }
            finally
            {
                // If this thread is aborted or errored, spin up a new thread prior to stopping
                CreateWorkerThread();
            }
        }
    }
}
