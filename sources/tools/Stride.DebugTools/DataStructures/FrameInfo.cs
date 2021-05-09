// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Framework.MicroThreading;

namespace Stride.DebugTools.DataStructures
{
    public class FrameInfo
    {
        public uint FrameNumber { get; set; }
        public double BeginTime { get; set; }
        public double EndTime { get; set; }
        public List<ThreadInfo> ThreadItems { get; private set; }

        public FrameInfo()
        {
            ThreadItems = new List<ThreadInfo>();
        }

        public FrameInfo Duplicate()
        {
            FrameInfo duplicate = new FrameInfo();

            duplicate.FrameNumber = FrameNumber;
            duplicate.BeginTime = BeginTime;
            duplicate.EndTime = EndTime;
            ThreadItems.ForEach(item => duplicate.ThreadItems.Add(item.Duplicate()));

            return duplicate;
        }
    }
}
