// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Threading.Tasks;

using ServiceWire.NamedPipes;

using Stride.Core.Diagnostics;

namespace Stride.Debugger.Target
{
    public class GameDebuggerHost : IGameDebuggerHost
    {
        private readonly TaskCompletionSource<IGameDebuggerTarget> target = new TaskCompletionSource<IGameDebuggerTarget>();

        private NpClient<IGameDebuggerTarget> callbackChannel;
        public LoggerResult Log { get; private set; }

        public Task<IGameDebuggerTarget> Target => target.Task;

        public event Action GameExited;


        public GameDebuggerHost(LoggerResult logger)
        {
            Log = logger;
        }


        public void RegisterTarget(string callbackAddress)
        {
            callbackChannel = new NpClient<IGameDebuggerTarget>(new NpEndPoint(callbackAddress));
            target.TrySetResult(callbackChannel.Proxy);
        }

        public void OnGameExited()
        {
            GameExited?.Invoke();
        }

        public void OnLogMessage(SerializableLogMessage logMessage)
        {
            Log.Log(logMessage);
        }

        public void Dispose()
        {
            callbackChannel.Dispose();
        }
    }
}
