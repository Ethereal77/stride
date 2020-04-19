// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Threading.Tasks;

namespace Xenko.Core.BuildEngine.Tests.Commands
{
    public class DummyAwaitingCommand : TestCommand
    {
        public int Delay = 0;

        protected override async Task<ResultStatus> DoCommandOverride(ICommandContext commandContext)
        {
            // Simulating awaiting result
            try
            {
                await Task.Delay(Delay, CancellationToken);

            }
            catch (TaskCanceledException) {}

            return CancellationToken.IsCancellationRequested ? ResultStatus.Cancelled : ResultStatus.Successful;
        }
    }
}
