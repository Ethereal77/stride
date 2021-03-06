// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Diagnostics;

namespace Stride.Debugger.Target
{
    /// <summary>
    ///   Defines the methods for a debugging host that can be accessed by a <see cref="IGameDebuggerTarget"/>.
    /// </summary>
    public interface IGameDebuggerHost : IDisposable
    {
        void RegisterTarget(string callbackAddress);

        void OnGameExited();

        void OnLogMessage(SerializableLogMessage logMessage);
    }
}
