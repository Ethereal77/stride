// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Rendering
{
    /// <summary>
    /// Sort key used 
    /// </summary>
    public struct SortKey : IComparable<SortKey>
    {
        public ulong Value;
        public int Index;
        public int StableIndex;

        public int CompareTo(SortKey other)
        {
            var result = Value.CompareTo(other.Value);
            return result != 0 ? result : StableIndex.CompareTo(other.StableIndex);
        }
    }
}
