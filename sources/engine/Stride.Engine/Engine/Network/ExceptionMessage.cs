// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Diagnostics;

namespace Stride.Engine.Network
{
    /// <summary>
    /// In the case of a SocketMessage when we use it in a SendReceiveAsync we want to propagate exceptions from the remote host
    /// </summary>
    public class ExceptionMessage : SocketMessage
    {
        /// <summary>
        /// Remote exception information
        /// </summary>
        public ExceptionInfo ExceptionInfo;
    }
}
