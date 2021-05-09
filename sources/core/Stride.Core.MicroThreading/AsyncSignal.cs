// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

namespace Stride.Core.MicroThreading
{
    public class AsyncSignal
    {
        private TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        public Task WaitAsync()
        {
            lock (this)
            {
                tcs = new TaskCompletionSource<bool>();
                var result = tcs.Task;
                return result;
            }
        }

        public void Set()
        {
            lock (this)
            {
                tcs.TrySetResult(true);
            }
        }
    }
}
