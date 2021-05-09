// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Stride.Core.Annotations;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Serializers;

namespace Stride.Core.Collections
{
    /// <summary>
    ///   Represents a light collection that maintains the order of the elements with valuetype enumerator to avoid allocating
    ///   in <see langword="foreach"/> loops, and various helper functions.
    /// </summary>
    /// <typeparam name="T">Type of elements of this collection.</typeparam>
    [DataSerializer(typeof(ListAllSerializer<,>), Mode = DataSerializerGenericMode.TypeAndGenericArguments)]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class OrderedCollection<T> : ICollection<T>
    {
        private const int DefaultCapacity = 4;

        private readonly IComparer<T> comparer;
        private T[] items;
        private int size;

        /// <summary>
        ///   Initializes a new instance of the <see cref="OrderedCollection{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer providing information about order between elements.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is a <c>null</c> reference.</exception>
        public OrderedCollection([NotNull] IComparer<T> comparer)
        {
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            this.comparer = comparer;
            items = Array.Empty<T>();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="OrderedCollection{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer providing information about order between elements.</param>
        /// <param name="capacity">Initial capacity.</param>
        public OrderedCollection([NotNull] IComparer<T> comparer, int capacity) : this(comparer)
        {
            items = new T[capacity];
        }

        /// <summary>
        ///   Gets or sets the number of elements this collection has space for before growing.
        /// </summary>
        public int Capacity
        {
            get => items.Length;

            set
            {
                if (value != items.Length)
                {
                    if (value > 0)
                    {
                        var destinationArray = new T[value];
                        if (size > 0)
                            Array.Copy(items, 0, destinationArray, 0, size);

                        items = destinationArray;
                    }
                    else
                    {
                        items = Array.Empty<T>();
                    }
                }
            }
        }

        /// <summary>
        ///   Gets the number of elements in this collection.
        /// </summary>
        public int Count => size;

        /// <summary>
        ///   Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <value>
        ///   The element at the specified <paramref name="index"/>.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range.</exception>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= size)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return items[index];
            }
        }

        /// <summary>
        ///   Adds an element to the collection.
        /// </summary>
        /// <param name="item">The element to add.</param>
        public void Add(T item)
        {
            AddItem(item);
        }

        /// <summary>
        ///   Removes all elements from the collection.
        /// </summary>
        public void Clear()
        {
            ClearItems();
        }

        /// <summary>
        ///   Determines if an element exists in the collection.
        /// </summary>
        /// <param name="item">Element to look for.</param>
        /// <returns><c>true</c> if the element exists in the collection; <c>false</c> otherwise.</returns>
        public bool Contains(T item)
        {
            if (item == null)
            {
                for (var j = 0; j < size; j++)
                    if (items[j] == null)
                        return true;

                return false;
            }

            var equalComparer = EqualityComparer<T>.Default;

            for (var i = 0; i < size; i++)
                if (equalComparer.Equals(items[i], item))
                    return true;

            return false;
        }

        /// <summary>
        ///   Copies the contents of the collection to an array.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The index of the array where to start copying to.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(items, 0, array, arrayIndex, size);
        }

        /// <summary>
        ///   Searches for the specified element in the collection and returns the first occurrence.
        /// </summary>
        /// <param name="item">The element to search for.</param>
        /// <returns>Index of the first occurrence of <paramref name="item"/> in the collection; or -1 if not found.</returns>
        public int IndexOf(T item)
        {
            return Array.IndexOf(items, item, 0, size);
        }

        /// <summary>
        ///   Removes the specified element from the collection.
        /// </summary>
        /// <param name="item">The element to remove.</param>
        /// <returns><c>true</c> if the element was found and removed; <c>false</c> otherwise.</returns>
        public bool Remove(T item)
        {
            var index = IndexOf(item);

            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        ///   Removes the element from the collection at the specified index.
        /// </summary>
        /// <param name="index">Index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= size)
                throw new ArgumentOutOfRangeException(nameof(index));

            RemoveItem(index);
        }

        /// <summary>
        ///   Removes all the elements from the collection.
        ///   Override this method in a derived class to implement custom logic for when the collection should be cleared.
        /// </summary>
        protected virtual void ClearItems()
        {
            if (size > 0)
                Array.Clear(items, 0, size);

            size = 0;
        }

        /// <summary>
        ///   Adds an element to the collection.
        ///   Override this method in a derived class to implement custom logic for adding an element.
        /// </summary>
        /// <param name="item">The element to add.</param>
        protected virtual void AddItem(T item)
        {
            var index = Array.BinarySearch(items, index: 0, size, item, comparer);

            if (index < 0)
                // Insert at the end of the list
                index = ~index;

            if (size == items.Length)
                EnsureCapacity(size + 1);

            if (index < size)
                Array.Copy(items, index, items, index + 1, size - index);

            items[index] = item;
            size++;
        }

        /// <summary>
        ///   Remove an element from the collection at the specified index.
        ///   Override this method in a derived class to implement custom logic for removing an element.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        protected virtual void RemoveItem(int index)
        {
            size--;

            if (index < size)
                Array.Copy(items, index + 1, items, index, size - index);

            items[size] = default;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        /// <summary>
        ///   Adds the elements of the specified source to the end of the collection.
        /// </summary>
        /// <param name="itemsToAdd">The elements to add to this collection.</param>
        public void AddRange<TEnumerable>([NotNull] TEnumerable itemsToAdd) where TEnumerable : IEnumerable<T>
        {
            foreach (var item in itemsToAdd)
                Add(item);
        }

        /// <summary>
        ///   Gets an <see cref="Enumerator"/> that can be used to iterate the collection.
        /// </summary>
        /// <returns>A valuetype enumerator to iterate this collection.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        bool ICollection<T>.IsReadOnly => false;

        /// <summary>
        ///   Checks the collection has enough space to store a specific number of elements. If not, grows the
        ///   backing storage to have the needed space.
        /// </summary>
        /// <param name="min">Minimum number of elements the collection should have capacity for.</param>
        public void EnsureCapacity(int min)
        {
            if (items.Length < min)
            {
                var num = (items.Length == 0) ? DefaultCapacity : (items.Length * 2);

                if (num < min)
                    num = min;

                Capacity = num;
            }
        }


        /// <summary>
        ///   A valuetype enumerator that can be used to iterate through the elements of a <see cref="FastCollection{T}"/>.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Enumerator : IEnumerator<T>
        {
            private readonly OrderedCollection<T> collection;
            private int index;
            private T current;

            internal Enumerator(OrderedCollection<T> collection)
            {
                this.collection = collection;
                index = 0;
                current = default;
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                if (index < collection.size)
                {
                    current = collection.items[index];
                    index++;
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                index = collection.size + 1;
                current = default;
                return false;
            }

            public T Current => current;

            object IEnumerator.Current => Current;

            void IEnumerator.Reset()
            {
                index = 0;
                current = default;
            }
        }
    }
}
