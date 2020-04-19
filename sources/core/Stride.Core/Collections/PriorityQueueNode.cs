// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Collections
{
    /// <summary>
    /// Represents a node in a priority queue, to allow O(n) removal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueueNode<T>
    {
        public T Value;

        public int Index { get; internal set; }

        public PriorityQueueNode(T value)
        {
            Value = value;
            Index = -1;
        }
    }
}
