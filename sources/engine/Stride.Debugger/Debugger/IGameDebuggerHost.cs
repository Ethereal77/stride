// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using Stride.Core.Diagnostics;

namespace Stride.Debugger.Target
{
    /// <summary>
    ///   Provides methods for a debugging host that can be accessed by a <see cref="IGameDebuggerTarget"/>.
    /// </summary>
    public interface IGameDebuggerHost : IDisposable
    {
        void RegisterTarget(string callbackAddress);

        void OnGameExited();

        void OnLogMessage(SerializableLogMessage logMessage);
    }
}
