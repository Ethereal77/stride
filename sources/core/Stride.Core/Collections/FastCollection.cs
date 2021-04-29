// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Stride.Core.Annotations;
using Stride.Core.Diagnostics;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Serializers;

namespace Stride.Core.Collections
{
    /// <summary>
    ///   Represents a faster and lighter implementation of <see cref="System.Collections.ObjectModel.Collection{T}"/> with valuetype
    ///   enumerators to avoid allocating in <see langword="foreach"/> loops, and various helper functions.
    /// </summary>
    /// <typeparam name="T">Type of elements of this collection </typeparam>
    [DataSerializer(typeof(ListAllSerializer<,>), Mode = DataSerializerGenericMode.TypeAndGenericArguments)]
    [DebuggerTypeProxy(typeof(CollectionDebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class FastCollection<T> : IList<T>, IReadOnlyList<T>
    {
        private const int DefaultCapacity = 4;

        private T[] items;
        private int size;


        /// <summary>
        ///   Initializes a new instance of the <see cref="FastCollection{T}"/> class.
        /// </summary>
        public FastCollection()
        {
            items = Array.Empty<T>();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FastCollection{T}"/> class.
        /// </summary>
        /// <param name="collection">Collection of elements to add initially.</param>
        public FastCollection([NotNull] IEnumerable<T> collection)
        {
            if (collection is ICollection<T> c)
            {
                var count = c.Count;
                items = new T[count];
                c.CopyTo(items, 0);
                size = count;
            }
            else
            {
                size = 0;
                items = new T[DefaultCapacity];

                using var enumerator = collection.GetEnumerator();
                while (enumerator.MoveNext())
                    Add(enumerator.Current);
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="FastCollection{T}"/> class.
        /// </summary>
        /// <param name="capacity">Initial capacity.</param>
        public FastCollection(int capacity)
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
        ///   Gets or sets the element at the specified index.
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
            set
            {
                if (index < 0 || index >= size)
                    throw new ArgumentOutOfRangeException(nameof(index));

                SetItem(index, value);
            }
        }


        /// <summary>
        ///   Adds an element to the collection.
        /// </summary>
        /// <param name="item">The element to add.</param>
        public void Add(T item) => InsertItem(size, item);

        /// <summary>
        ///   Removes all elements from the collection.
        /// </summary>
        public void Clear() => ClearItems();

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

            var comparer = EqualityComparer<T>.Default;

            for (var i = 0; i < size; i++)
                if (comparer.Equals(items[i], item))
                    return true;

            return false;
        }

        /// <summary>
        ///   Copies the contents of the collection to an array.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The index of the array where to start copying to.</param>
        public void CopyTo(T[] array, int arrayIndex) => Array.Copy(items, 0, array, arrayIndex, size);

        /// <summary>
        ///   Searches for the specified element in the collection and returns the first occurrence.
        /// </summary>
        /// <param name="item">The element to search for.</param>
        /// <returns>Index of the first occurrence of <paramref name="item"/> in the collection; or -1 if not found.</returns>
        public int IndexOf(T item) => Array.IndexOf(items, item, 0, size);

        /// <summary>
        ///   Inserts the specified element in the collection at the specified index.
        /// </summary>
        /// <param name="index">Index where to insert the element.</param>
        /// <param name="item">The element to insert.</param>
        public void Insert(int index, T item)
        {
            if (index < 0 || index > size)
                throw new ArgumentOutOfRangeException(nameof(index));

            InsertItem(index, item);
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
        public Enumerator GetEnumerator() => new Enumerator(this);

        /// <summary>
        ///   Sorts the elements of the collection using the default comparer.
        /// </summary>
        public void Sort() => Sort(index: 0, Count, comparer: null);

        /// <summary>
        ///   Sorts the elements of the collection using a custom comparer.
        /// </summary>
        /// <param name="comparer">An object that can compare the elements of this collection.</param>
        public void Sort(IComparer<T> comparer) => Sort(index: 0, Count, comparer);

        /// <summary>
        ///   Sorts the elements of the collection using a custom comparer.
        /// </summary>
        /// <param name="index">Index from which to start sorting the elements of the collection.</param>
        /// <param name="count">Number of elements to sort starting from <paramref name="index"/>.</param>
        /// <param name="comparer">An object that can compare the elements of this collection.</param>
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            Array.Sort(items, index, count, comparer);
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
        ///   Inserts an element at the specified index in the collection.
        ///   Override this method in a derived class to implement custom logic for adding an element.
        /// </summary>
        /// <param name="index">The index where to insert the element.</param>
        /// <param name="item">The element to add.</param>
        protected virtual void InsertItem(int index, T item)
        {
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

        /// <summary>
        ///   Replaces an element at the specified index in the collection.
        ///   Override this method in a derived class to implement custom logic for replacing an element.
        /// </summary>
        /// <param name="index">The index where to replace the element.</param>
        /// <param name="item">The element to put in place of the replaced one.</param>
        protected virtual void SetItem(int index, T item)
        {
            items[index] = item;
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
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            private readonly FastCollection<T> list;

            private int index;
            private T current;

            internal Enumerator(FastCollection<T> list)
            {
                this.list = list;
                index = 0;
                current = default(T);
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                var list = this.list;

                if (index < list.size)
                {
                    current = list.items[index];
                    index++;
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                index = list.size + 1;
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
