// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Rendering
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
