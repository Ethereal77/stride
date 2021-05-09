// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2014 AsyncEx - StephenCleary
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stride.Core.Threading
{
    /// <summary>
    /// A collection of cancelable <see cref="TaskCompletionSource{T}"/> instances. Implementations must be threadsafe <b>and</b> must work correctly if the caller is holding a lock.
    /// </summary>
    /// <typeparam name="T">The type of the results. If this isn't needed, use <see cref="Object"/>.</typeparam>
    public interface IAsyncWaitQueue<T>
    {
        /// <summary>
        /// Gets whether the queue is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Creates a new entry and queues it to this wait queue. The returned task must support both synchronous and asynchronous waits.
        /// </summary>
        /// <returns>The queued task.</returns>
        Task<T> Enqueue();

        /// <summary>
        /// Removes a single entry in the wait queue. Returns a disposable that completes that entry.
        /// </summary>
        /// <param name="result">The result used to complete the wait queue entry. If this isn't needed, use <c>default(T)</c>.</param>
        IDisposable Dequeue(T result = default(T));

        /// <summary>
        /// Removes all entries in the wait queue. Returns a disposable that completes all entries.
        /// </summary>
        /// <param name="result">The result used to complete the wait queue entries. If this isn't needed, use <c>default(T)</c>.</param>
        IDisposable DequeueAll(T result = default(T));

        /// <summary>
        /// Attempts to remove an entry from the wait queue. Returns a disposable that cancels the entry.
        /// </summary>
        /// <param name="task">The task to cancel.</param>
        /// <returns>A value indicating whether the entry was found and canceled.</returns>
        IDisposable TryCancel(Task task);

        /// <summary>
        /// Removes all entries from the wait queue. Returns a disposable that cancels all entries.
        /// </summary>
        IDisposable CancelAll();
    }

    /// <summary>
    /// Provides extension methods for wait queues.
    /// </summary>
    public static class AsyncWaitQueueExtensions
    {
        /// <summary>
        /// Creates a new entry and queues it to this wait queue. If the cancellation token is already canceled, this method immediately returns a canceled task without modifying the wait queue.
        /// </summary>
        /// <param name="this">The wait queue.</param>
        /// <param name="syncObject">A synchronization object taken while cancelling the entry.</param>
        /// <param name="token">The token used to cancel the wait.</param>
        /// <returns>The queued task.</returns>
        public static Task<T> Enqueue<T>(this IAsyncWaitQueue<T> @this, object syncObject, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return Task.FromCanceled<T>(token);

            var ret = @this.Enqueue();
            if (!token.CanBeCanceled)
                return ret;

            var registration = token.Register(() =>
            {
                IDisposable finish;
                lock (syncObject)
                    finish = @this.TryCancel(ret);
                finish.Dispose();
            }, useSynchronizationContext: false);
            ret.ContinueWith(_ => registration.Dispose(), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            return ret;
        }
    }
}
