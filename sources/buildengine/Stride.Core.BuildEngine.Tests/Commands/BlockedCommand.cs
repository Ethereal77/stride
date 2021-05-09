// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace Stride.Core.BuildEngine.Tests.Commands
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
