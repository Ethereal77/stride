// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2014 AsyncEx - StephenCleary
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Stride.Core.Annotations;

namespace Stride.Core.Threading
{
    /// <summary>
    /// The default wait queue implementation, which uses a double-ended queue.
    /// </summary>
    /// <typeparam name="T">The type of the results. If this isn't needed, use <see cref="Object"/>.</typeparam>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [DebuggerTypeProxy(typeof(DefaultAsyncWaitQueue<>.DebugView))]
    internal sealed class DefaultAsyncWaitQueue<T> : IAsyncWaitQueue<T>
    {
        private readonly Deque<TaskCompletionSource<T>> queue = new Deque<TaskCompletionSource<T>>();

        private int Count
        {
            get { lock (queue) { return queue.Count; } }
        }

        bool IAsyncWaitQueue<T>.IsEmpty => Count == 0;

        Task<T> IAsyncWaitQueue<T>.Enqueue()
        {
            var tcs = new TaskCompletionSource<T>();
            lock (queue)
                queue.AddToBack(tcs);
            return tcs.Task;
        }

        [NotNull]
        IDisposable IAsyncWaitQueue<T>.Dequeue(T result)
        {
            TaskCompletionSource<T> tcs;
            lock (queue)
                tcs = queue.RemoveFromFront();
            return new CompleteDisposable(result, tcs);
        }

        [NotNull]
        IDisposable IAsyncWaitQueue<T>.DequeueAll(T result)
        {
            TaskCompletionSource<T>[] taskCompletionSources;
            lock (queue)
            {
                taskCompletionSources = queue.ToArray();
                queue.Clear();
            }
            return new CompleteDisposable(result, taskCompletionSources);
        }

        [NotNull]
        IDisposable IAsyncWaitQueue<T>.TryCancel(Task task)
        {
            TaskCompletionSource<T> tcs = null;
            lock (queue)
            {
                for (int i = 0; i != queue.Count; ++i)
                {
                    if (queue[i].Task == task)
                    {
                        tcs = queue[i];
                        queue.RemoveAt(i);
                        break;
                    }
                }
            }
            if (tcs == null)
                return new CancelDisposable();
            return new CancelDisposable(tcs);
        }

        [NotNull]
        IDisposable IAsyncWaitQueue<T>.CancelAll()
        {
            TaskCompletionSource<T>[] taskCompletionSources;
            lock (queue)
            {
                taskCompletionSources = queue.ToArray();
                queue.Clear();
            }
            return new CancelDisposable(taskCompletionSources);
        }

        private sealed class CancelDisposable : IDisposable
        {
            private readonly TaskCompletionSource<T>[] taskCompletionSources;

            public CancelDisposable(params TaskCompletionSource<T>[] taskCompletionSources)
            {
                this.taskCompletionSources = taskCompletionSources;
            }

            public void Dispose()
            {
                foreach (var cts in taskCompletionSources)
                    cts.TrySetCanceledWithBackgroundContinuations();
            }
        }

        private sealed class CompleteDisposable : IDisposable
        {
            private readonly TaskCompletionSource<T>[] taskCompletionSources;
            private readonly T result;

            public CompleteDisposable(T result, params TaskCompletionSource<T>[] taskCompletionSources)
            {
                this.result = result;
                this.taskCompletionSources = taskCompletionSources;
            }

            public void Dispose()
            {
                foreach (var cts in taskCompletionSources)
                    cts.TrySetResultWithBackgroundContinuations(result);
            }
        }

        [DebuggerNonUserCode]
        internal sealed class DebugView
        {
            private readonly DefaultAsyncWaitQueue<T> queue;

            public DebugView(DefaultAsyncWaitQueue<T> queue)
            {
                this.queue = queue;
            }

            [NotNull]
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public Task<T>[] Tasks
            {
                get { return queue.queue.Select(x => x.Task).ToArray(); }
            }
        }
    }
}
