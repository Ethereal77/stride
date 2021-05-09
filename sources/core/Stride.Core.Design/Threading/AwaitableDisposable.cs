// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2014 AsyncEx - StephenCleary
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Stride.Core.Threading
{
    /// <summary>
    /// An awaitable wrapper around a task whose result is disposable. The wrapper is not disposable, so this prevents usage errors like "using (MyAsync())" when the appropriate usage should be "using (await MyAsync())".
    /// </summary>
    /// <typeparam name="T">The type of the result of the underlying task.</typeparam>
    public struct AwaitableDisposable<T> where T : IDisposable
    {
        /// <summary>
        /// The underlying task.
        /// </summary>
        private readonly Task<T> task;

        /// <summary>
        /// Initializes a new awaitable wrapper around the specified task.
        /// </summary>
        /// <param name="task">The underlying task to wrap.</param>
        public AwaitableDisposable(Task<T> task)
        {
            this.task = task;
        }

        /// <summary>
        /// Returns the underlying task.
        /// </summary>
        public Task<T> AsTask()
        {
            return task;
        }

        /// <summary>
        /// Implicit conversion to the underlying task.
        /// </summary>
        /// <param name="source">The awaitable wrapper.</param>
        public static implicit operator Task<T>(AwaitableDisposable<T> source)
        {
            return source.AsTask();
        }

        /// <summary>
        /// Infrastructure. Returns the task awaiter for the underlying task.
        /// </summary>
        public TaskAwaiter<T> GetAwaiter()
        {
            return task.GetAwaiter();
        }

        /// <summary>
        /// Infrastructure. Returns a configured task awaiter for the underlying task.
        /// </summary>
        /// <param name="continueOnCapturedContext">Whether to attempt to marshal the continuation back to the captured context.</param>
        public ConfiguredTaskAwaitable<T> ConfigureAwait(bool continueOnCapturedContext)
        {
            return task.ConfigureAwait(continueOnCapturedContext);
        }
    }
}
