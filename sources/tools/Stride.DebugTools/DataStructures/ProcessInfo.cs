// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.DebugTools.DataStructures
{
    public class ProcessInfo
    {
        public List<FrameInfo> Frames { get; private set; }

        public ProcessInfo()
        {
            Frames = new List<FrameInfo>();
        }

        public ProcessInfo Duplicate()
        {
            ProcessInfo duplicate = new ProcessInfo();

            Frames.ForEach(item => duplicate.Frames.Add(item.Duplicate()));

            return duplicate;
        }

        public double BeginTime
        {
            get
            {
                if (Frames == null || Frames.Count == 0)
                    return -1.0;
                return Frames[0].BeginTime;
            }
        }

        public double EndTime
        {
            get
            {
                if (Frames == null || Frames.Count == 0)
                    return -1.0;
                return Frames[Frames.Count - 1].EndTime;
            }
        }

        public double TimeLength
        {
            get
            {
                if (Frames == null || Frames.Count == 0)
                    return -1.0;
                return Frames[Frames.Count - 1].EndTime - Frames[0].BeginTime;
            }
        }
    }
}
