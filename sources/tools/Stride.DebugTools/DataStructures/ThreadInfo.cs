// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.DebugTools.DataStructures
{
    public class ThreadInfo
    {
        public int Id { get; set; }
        public List<MicroThreadInfo> MicroThreadItems { get; private set; }

        public ThreadInfo()
        {
            MicroThreadItems = new List<MicroThreadInfo>();
        }

        public ThreadInfo Duplicate()
        {
            ThreadInfo duplicate = new ThreadInfo();

            duplicate.Id = Id;
            MicroThreadItems.ForEach(item => duplicate.MicroThreadItems.Add(item.Duplicate()));

            return duplicate;
        }
    }
}
