// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

namespace Xenko.Core.BuildEngine.Tests.Commands
{
    public class BlockedCommand : TestCommand
    {
        private readonly Semaphore sem = new Semaphore(0, 1);

        protected override async Task<ResultStatus> DoCommandOverride(ICommandContext commandContext)
        {
            sem.WaitOne();
            return await Task.FromResult(CancellationToken.IsCancellationRequested ? ResultStatus.Cancelled : ResultStatus.Successful);
        }

        public override void Cancel()
        {
            sem.Release();
        }
    }
}
