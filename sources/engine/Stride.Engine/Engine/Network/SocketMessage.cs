// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading;

using Stride.Core;

namespace Stride.Engine.Network
{
    [DataContract(Inherited = true)]
    public class SocketMessage
    {
        /// <summary>
        /// An ID that will identify the message, in order to answer to it.
        /// </summary>
        public int StreamId;

        public static int NextStreamId => Interlocked.Increment(ref globalStreamId);

        private static int globalStreamId;
    }
}
