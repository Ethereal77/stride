// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Rendering.LightProbes
{
    public struct TetrahedronSortKey : IComparable<TetrahedronSortKey>
    {
        public int Index;
        public int SortKey;

        public TetrahedronSortKey(int index, int sortKey)
        {
            Index = index;
            SortKey = sortKey;
        }

        public int CompareTo(TetrahedronSortKey other)
        {
            return SortKey.CompareTo(other.SortKey);
        }

        public override string ToString()
        {
            return $"Tetrahedron Index: {Index}; SortKey: {SortKey}";
        }
    }
}
