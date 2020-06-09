// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.ExecServer
{
    /// <summary>
    ///   Provides a method to log back standard output and error to client from a server through ServiceWire.
    /// </summary>
    public interface IServerLogger
    {
        void OnLog(string text, ConsoleColor color);
    }
}
