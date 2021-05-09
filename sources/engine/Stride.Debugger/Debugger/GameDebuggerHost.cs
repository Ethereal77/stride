// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Threading.Tasks;

using ServiceWire.NamedPipes;

using Stride.Core.Diagnostics;

namespace Stride.Debugger.Target
{
    /// <summary>
    ///   A debugger host that can be controlled by a <see cref="IGameDebuggerTarget"/>.
    /// </summary>
    public class GameDebuggerHost : IGameDebuggerHost
    {
        private readonly TaskCompletionSource<IGameDebuggerTarget> target = new();

        private NpClient<IGameDebuggerTarget> callbackChannel;

        /// <summary>
        ///   Gets the debugging log.
        /// </summary>
        public LoggerResult Log { get; private set; }

        /// <summary>
        ///   Gets the debugging target assemblies.
        /// </summary>
        public Task<IGameDebuggerTarget> Target => target.Task;


        /// <summary>
        ///   Occurs when the game being debugged has exited.
        /// </summary>
        public event Action GameExited;


        /// <summary>
        ///   Initializes a new instance of the <see cref="GameDebuggerHost"/> class.
        /// </summary>
        /// <param name="logger">The logger where to write diagnostic messages.</param>
        public GameDebuggerHost(LoggerResult logger)
        {
            Log = logger;
        }


        /// <summary>
        ///   Registers a <see cref="IGameDebuggerTarget"/> and starts the communication with it.
        /// </summary>
        /// <param name="callbackAddress">The endpoint where to connect.</param>
        public void RegisterTarget(string callbackAddress)
        {
            callbackChannel = new NpClient<IGameDebuggerTarget>(new NpEndPoint(callbackAddress));
            target.TrySetResult(callbackChannel.Proxy);
        }

        /// <summary>
        ///   Called when the game being debugged has exited. Raises the <see cref="GameExited"/> event.
        /// </summary>
        public void OnGameExited() => GameExited?.Invoke();

        /// <summary>
        ///   Called when a diagnostic message needs to be logged.
        /// </summary>
        /// <param name="logMessage">The message to log.</param>
        public void OnLogMessage(SerializableLogMessage logMessage) => Log.Log(logMessage);

        /// <inheritdoc/>
        public void Dispose() => callbackChannel.Dispose();
    }
}
