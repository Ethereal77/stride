// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

namespace Stride.Core.BuildEngine.Tests.Commands
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
