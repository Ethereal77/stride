// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Stride.Core.Reflection
{
    public class DefaultKeyComparer : IComparer<object>
    {
        public int Compare(object x, object y)
        {
            var left = x as IMemberDescriptor;
            var right = y as IMemberDescriptor;
            if (left != null && right != null)
            {
                // If order is defined, first order by order
                if (left.Order.HasValue | right.Order.HasValue)
                {
                    var leftOrder = left.Order ?? int.MaxValue;
                    var rightOrder = right.Order ?? int.MaxValue;
                    return leftOrder.CompareTo(rightOrder);
                }

                // else order by name (dynamic members, etc...)
                return left.DefaultNameComparer.Compare(left.Name, right.Name);
            }

            var sx = x as string;
            var sy = y as string;
            if (sx != null && sy != null)
            {
                return string.CompareOrdinal(sx, sy);
            }

            var leftComparable = x as IComparable;
            if (leftComparable != null)
            {
                return leftComparable.CompareTo(y);
            }

            var rightComparable = y as IComparable;
            return rightComparable?.CompareTo(y) ?? 0;
        }
    }
}
