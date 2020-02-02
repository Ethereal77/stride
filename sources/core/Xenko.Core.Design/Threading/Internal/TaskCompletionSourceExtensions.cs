// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2014 AsyncEx - StephenCleary
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xenko.Core.Annotations;

namespace Xenko.Core.Threading
{
    /// <summary>
    /// Provides extension methods for <see cref="TaskCompletionSource{TResult}"/>.
    /// </summary>
    internal static class TaskCompletionSourceExtensions
    {
        /// <summary>
        /// Attempts to complete a <see cref="TaskCompletionSource{TResult}"/> with the specified value, forcing all continuations onto a threadpool thread even if they specified <c>ExecuteSynchronously</c>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the asynchronous operation.</typeparam>
        /// <param name="this">The task completion source. May not be <c>null</c>.</param>
        /// <param name="result">The result of the asynchronous operation.</param>
        public static void TrySetResultWithBackgroundContinuations<TResult>([NotNull] this TaskCompletionSource<TResult> @this, TResult result)
        {
            // Set the result on a threadpool thread, so any synchronous continuations will execute in the background.
            Task.Run(() => @this.TrySetResult(result));

            // Wait for the TCS task to complete; note that the continuations may not be complete.
            @this.Task.Wait();
        }

        /// <summary>
        /// Attempts to complete a <see cref="TaskCompletionSource{TResult}"/> as canceled, forcing all continuations onto a threadpool thread even if they specified <c>ExecuteSynchronously</c>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the asynchronous operation.</typeparam>
        /// <param name="this">The task completion source. May not be <c>null</c>.</param>
        public static void TrySetCanceledWithBackgroundContinuations<TResult>([NotNull] this TaskCompletionSource<TResult> @this)
        {
            // Complete on a threadpool thread, so any synchronous continuations will execute in the background.
            Task.Run(() => @this.TrySetCanceled());

            // Wait for the TCS task to complete; note that the continuations may not be complete.
            try
            {
                @this.Task.Wait();
            }
            catch (AggregateException)
            {
            }
        }
    }
}
