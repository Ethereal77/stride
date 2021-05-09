// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Stride.Core.Yaml
{
    /// <summary>
    /// Generic queue on which items may be inserted
    /// </summary>
    public class InsertionQueue<T>
    {
        // TODO: Use a more efficient data structure

        private readonly IList<T> items = new List<T>();

        /// <summary>
        /// Gets the number of items that are contained by the queue.
        /// </summary>
        public int Count { get { return items.Count; } }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item to be enqueued.</param>
        public void Enqueue(T item)
        {
            items.Add(item);
        }

        /// <summary>
        /// Dequeues an item.
        /// </summary>
        /// <returns>Returns the item that been dequeued.</returns>
        public T Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }

            T item = items[0];
            items.RemoveAt(0);
            return item;
        }

        /// <summary>
        /// Inserts an item at the specified index.
        /// </summary>
        /// <param name="index">The index where to insert the item.</param>
        /// <param name="item">The item to be inserted.</param>
        public void Insert(int index, T item)
        {
            items.Insert(index, item);
        }
    }
}