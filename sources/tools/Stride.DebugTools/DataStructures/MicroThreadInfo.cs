// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Framework.MicroThreading;

namespace Stride.DebugTools.DataStructures
{
    public class MicroThreadInfo
    {
        public long Id { get; set; }
        public MicroThreadState BeginState { get; set; }
        public MicroThreadState EndState { get; set; }
        public double BeginTime { get; set; }
        public double EndTime { get; set; }

        public MicroThreadInfo Duplicate()
        {
            MicroThreadInfo duplicate = new MicroThreadInfo();

            duplicate.Id = Id;
            duplicate.BeginState = BeginState;
            duplicate.EndState = EndState;
            duplicate.BeginTime = BeginTime;
            duplicate.EndTime = EndTime;

            return duplicate;
        }
    }
}
