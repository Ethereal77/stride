// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Framework.MicroThreading;

namespace Stride.DebugTools.DataStructures
{
    internal class MicroThreadPendingState
    {
        internal int ThreadId { get; set; }
        internal double Time { get; set; }
        internal MicroThreadState State { get; set; }
        internal MicroThread MicroThread { get; set; }
    }
}
