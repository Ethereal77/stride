// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2004 Novell, Inc. (http://www.novell.com)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

//
// Authors:
//   Sergey Chaban (serge@wildwestsoftware.com)
//   Duncan Mak (duncan@ximian.com)
//   Herve Poussineau (hpoussineau@fr.st)
//   Zoltan Varga (vargaz@gmail.com)
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Stride.Core.Annotations;

namespace Stride.Core.Collections
{
    /// <summary>
    ///  Represents a collection of associated keys and values that are sorted by the keys and
    ///  are accessible by key and by index.
    /// </summary>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class SortedList<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary
    {
        private const int INITIAL_SIZE = 16;

        private enum EnumeratorMode { ByKey, ByValue, ByEntry }

        private int count;
        private int modificationCount;
        private KeyValuePair<TKey, TValue>[] table;
        private IComparer<TKey> comparer;
        private int defaultCapacity;


        public SortedList() : this(INITIAL_SIZE, null) { }

        public SortedList(int capacity) : this(capacity, null) { }

        public SortedList(IComparer<TKey> comparer) : this(INITIAL_SIZE, comparer) { }

        public SortedList(int capacity, IComparer<TKey> comparer)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            defaultCapacity = capacity == 0 ? 0 : INITIAL_SIZE;
            Initialize(comparer, capacity, true);
        }

        public SortedList([NotNull] IDictionary<TKey, TValue> dictionary) : this(dictionary, null) { }

        public SortedList([NotNull] IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));

            Initialize(comparer, dictionary.Count, true);

            foreach (var kvp in dictionary)
                Add(kvp.Key, kvp.Value);
        }


        public int Count => count;

        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this;

        bool IDictionary.IsFixedSize => false;
        bool IDictionary.IsReadOnly => false;

        public TValue this[[NotNull] TKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                var i = Find(key);

                if (i >= 0)
                    return table[i].Value;

                throw new KeyNotFoundException();
            }
            set
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                SetEntry(key, value, true);
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                if (key is TKey validKey)
                    return this[validKey];

                return null;
            }

            set => this[ToKey(key)] = ToValue(value);
        }

        public int Capacity
        {
            get => table.Length;

            set
            {
                var oldCapacity = table.Length;

                if (count > value)
                    throw new ArgumentOutOfRangeException("capacity too small");

                if (value == 0)
                {
                    // Return to default size
                    var newTable = new KeyValuePair<TKey, TValue>[defaultCapacity];
                    Array.Copy(table, newTable, count);
                    table = newTable;
                }
                else if (value > count)
                {
                    var newTable = new KeyValuePair<TKey, TValue>[value];
                    Array.Copy(table, newTable, count);
                    table = newTable;
                }
                else if (value > oldCapacity)
                {
                    var newTable = new KeyValuePair<TKey, TValue>[value];
                    Array.Copy(table, newTable, oldCapacity);
                    table = newTable;
                }
            }
        }

        [NotNull]
        public IList<TKey> Keys => new ListKeys(this);

        [NotNull]
        public IList<TValue> Values => new ListValues(this);

        ICollection IDictionary.Keys => new ListKeys(this);

        ICollection IDictionary.Values => new ListValues(this);

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        public IComparer<TKey> Comparer => comparer;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;


        public void Add([NotNull] TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            SetEntry(key, value, false);
        }

        public bool ContainsKey([NotNull] TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return Find(key) >= 0;
        }

        public bool Remove([NotNull] TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var i = IndexOfKey(key);
            if (i >= 0)
            {
                RemoveAt(i);
                return true;
            }
            return false;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear() => Clear();

        public void Clear()
        {
            defaultCapacity = INITIAL_SIZE;
            table = new KeyValuePair<TKey, TValue>[defaultCapacity];
            count = 0;
            modificationCount++;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (Count == 0)
                return;

            if (array is null)
                throw new ArgumentNullException();
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException();
            if (arrayIndex >= array.Length)
                throw new ArgumentException("arrayIndex is greater than or equal to array.Length.", nameof(arrayIndex));
            if (Count > (array.Length - arrayIndex))
                throw new ArgumentException("Not enough space in array from arrayIndex to end of array");

            var i = arrayIndex;
            foreach (var pair in this)
                array[i++] = pair;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Add(keyValuePair.Key, keyValuePair.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            var i = Find(keyValuePair.Key);

            if (i >= 0)
                return Comparer<KeyValuePair<TKey, TValue>>.Default.Compare(table[i], keyValuePair) == 0;
            return false;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            var i = Find(keyValuePair.Key);

            if (i >= 0 && (Comparer<KeyValuePair<TKey, TValue>>.Default.Compare(table[i], keyValuePair) == 0))
            {
                RemoveAt(i);
                return true;
            }
            return false;
        }

        // IEnumerable<KeyValuePair<TKey, TValue>>

        [NotNull]
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        // IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // IDictionary

        void IDictionary.Add(object key, object value)
        {
            SetEntry(ToKey(key), ToValue(value), false);
        }

        bool IDictionary.Contains(object key)
        {
            if (null == key)
                throw new ArgumentNullException();
            if (!(key is TKey))
                return false;

            return (Find((TKey)key) >= 0);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator(this, EnumeratorMode.ByEntry);
        }

        void IDictionary.Remove(object key)
        {
            if (null == key)
                throw new ArgumentNullException(nameof(key));
            if (!(key is TKey))
                return;
            var i = IndexOfKey((TKey)key);
            if (i >= 0) RemoveAt(i);
        }

        // ICollection

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            if (Count == 0)
                return;

            if (null == array)
                throw new ArgumentNullException();

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException();

            if (array.Rank > 1)
                throw new ArgumentException("array is multi-dimensional");
            if (arrayIndex >= array.Length)
                throw new ArgumentNullException("arrayIndex is greater than or equal to array.Length");
            if (Count > (array.Length - arrayIndex))
                throw new ArgumentNullException("Not enough space in array from arrayIndex to end of array");

            IEnumerator<KeyValuePair<TKey, TValue>> it = GetEnumerator();
            var i = arrayIndex;

            while (it.MoveNext())
            {
                array.SetValue(it.Current, i++);
            }
        }

        //
        // SortedList<TKey, TValue>
        //

        public void RemoveAt(int index)
        {
            var table = this.table;
            var cnt = Count;
            if (index >= 0 && index < cnt)
            {
                if (index != cnt - 1)
                {
                    Array.Copy(table, index + 1, table, index, cnt - 1 - index);
                }
                else
                {
                    table[index] = default;
                }
                --count;
                ++modificationCount;
            }
            else
            {
                throw new ArgumentOutOfRangeException("index out of range");
            }
        }

        public int IndexOfKey([NotNull] TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int index;
            try
            {
                index = Find(key);
            }
            catch
            {
                throw new InvalidOperationException();
            }

            return (index | (index >> 31));
        }

        public int IndexOfValue(TValue value)
        {
            if (count == 0)
                return -1;

            for (var i = 0; i < count; i++)
            {
                var current = table[i];

                if (Equals(value, current.Value))
                    return i;
            }

            return -1;
        }

        public bool ContainsValue(TValue value)
        {
            return IndexOfValue(value) >= 0;
        }

        public void TrimExcess()
        {
            if (count < table.Length * 0.9)
                Capacity = count;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var i = Find(key);

            if (i >= 0)
            {
                value = table[i].Value;
                return true;
            }
            value = default;
            return false;
        }

        //
        // Private methods
        //

        private void EnsureCapacity(int n, int free)
        {
            var table = this.table;
            KeyValuePair<TKey, TValue>[] newTable = null;
            var cap = Capacity;
            var gap = (free >= 0 && free < Count);

            if (n > cap)
            {
                newTable = new KeyValuePair<TKey, TValue>[n << 1];
            }

            if (newTable != null)
            {
                if (gap)
                {
                    var copyLen = free;
                    if (copyLen > 0)
                    {
                        Array.Copy(table, 0, newTable, 0, copyLen);
                    }
                    copyLen = Count - free;
                    if (copyLen > 0)
                    {
                        Array.Copy(table, free, newTable, free + 1, copyLen);
                    }
                }
                else
                {
                    // Just a resizing, copy the entire table.
                    Array.Copy(table, newTable, Count);
                }
                this.table = newTable;
            }
            else if (gap)
            {
                Array.Copy(table, free, table, free + 1, Count - free);
            }
        }

        private void SetEntry([NotNull] TKey key, TValue value, bool overwrite)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var table = this.table;

            int freeIndex;
            try
            {
                freeIndex = Find(key);
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }

            if (freeIndex >= 0)
            {
                if (!overwrite)
                    throw new ArgumentException("element already exists");

                table[freeIndex] = new KeyValuePair<TKey, TValue>(key, value);
                ++modificationCount;
                return;
            }

            freeIndex = ~freeIndex;

            if (freeIndex > Capacity + 1)
                throw new Exception("SortedList::internal error (" + key + ", " + value + ") at [" + freeIndex + "]");


            EnsureCapacity(Count + 1, freeIndex);

            table = this.table;
            table[freeIndex] = new KeyValuePair<TKey, TValue>(key, value);

            ++count;
            ++modificationCount;

        }

        private void Initialize(IComparer<TKey> comparer, int capacity, bool forceSize)
        {
            if (comparer is null)
                comparer = Comparer<TKey>.Default;
            this.comparer = comparer;
            if (!forceSize && (capacity < defaultCapacity))
                capacity = defaultCapacity;
            table = new KeyValuePair<TKey, TValue>[capacity];
            count = 0;
            modificationCount = 0;
        }

        private void CopyToArray([NotNull] Array arr, int i,
                       EnumeratorMode mode)
        {
            if (arr is null)
                throw new ArgumentNullException(nameof(arr));

            if (i < 0 || i + Count > arr.Length)
                throw new ArgumentOutOfRangeException(nameof(i));

            IEnumerator it = new DictionaryEnumerator(this, mode);

            while (it.MoveNext())
            {
                arr.SetValue(it.Current, i++);
            }
        }

        private int Find(TKey key)
        {
            var table = this.table;
            var len = Count;

            if (len == 0)
                return ~0;

            var left = 0;
            var right = len - 1;

            while (left <= right)
            {
                var guess = (left + right) >> 1;

                var cmp = comparer.Compare(table[guess].Key, key);
                if (cmp == 0)
                    return guess;

                if (cmp < 0)
                    left = guess + 1;
                else right = guess - 1;
            }

            return ~left;
        }

        [NotNull]
        private TKey ToKey([NotNull] object key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (!(key is TKey))
                throw new ArgumentException("The value \"" + key + "\" isn't of type \"" + typeof(TKey) + "\" and can't be used in this generic collection.", nameof(key));

            return (TKey) key;
        }

        private TValue ToValue(object value)
        {
            if (!(value is TValue))
                throw new ArgumentException("The value \"" + value + "\" isn't of type \"" + typeof(TValue) + "\" and can't be used in this generic collection.", nameof(value));

            return (TValue)value;
        }

        internal TKey KeyAt(int index)
        {
            if (index >= 0 && index < Count)
                return table[index].Key;

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        internal TValue ValueAt(int index)
        {
            if (index >= 0 && index < Count)
                return table[index].Value;

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        //
        // Inner classes
        //

        public sealed class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private SortedList<TKey, TValue> host;
            private int pos = -1;

            public Enumerator(SortedList<TKey, TValue> host)
            {
                this.host = host;
            }

            public void Dispose()
            {
                host = null;
            }

            public bool MoveNext()
            {
                return ++pos < host.count;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            [NotNull]
            object IEnumerator.Current => Current;

            public KeyValuePair<TKey, TValue> Current => host.table[pos];
        }


        private sealed class DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
        {
            private readonly SortedList<TKey, TValue> host;
            private int stamp;
            private int pos;
            private int size;
            private readonly EnumeratorMode mode;

            private object currentKey;
            private object currentValue;

            bool invalid = false;

            private static readonly string xstr = "SortedList.Enumerator: snapshot out of sync.";

            public DictionaryEnumerator([NotNull] SortedList<TKey, TValue> host, EnumeratorMode mode)
            {
                this.host = host;
                stamp = host.modificationCount;
                size = host.Count;
                this.mode = mode;
                Reset();
            }

            public DictionaryEnumerator([NotNull] SortedList<TKey, TValue> host)
                : this(host, EnumeratorMode.ByEntry)
            {
            }

            public void Reset()
            {
                if (host.modificationCount != stamp || invalid)
                    throw new InvalidOperationException(xstr);

                pos = -1;
                currentKey = null;
                currentValue = null;
            }

            public bool MoveNext()
            {
                if (host.modificationCount != stamp || invalid)
                    throw new InvalidOperationException(xstr);

                var table = host.table;

                if (++pos < size)
                {
                    var entry = table[pos];

                    currentKey = entry.Key;
                    currentValue = entry.Value;
                    return true;
                }

                currentKey = null;
                currentValue = null;
                return false;
            }

            public DictionaryEntry Entry
            {
                get
                {
                    if (invalid || pos >= size || pos == -1)
                        throw new InvalidOperationException(xstr);

                    return new DictionaryEntry(currentKey,
                                                currentValue);
                }
            }

            public object Key
            {
                get
                {
                    if (invalid || pos >= size || pos == -1)
                        throw new InvalidOperationException(xstr);
                    return currentKey;
                }
            }

            public object Value
            {
                get
                {
                    if (invalid || pos >= size || pos == -1)
                        throw new InvalidOperationException(xstr);
                    return currentValue;
                }
            }

            public object Current
            {
                get
                {
                    if (invalid || pos >= size || pos == -1)
                        throw new InvalidOperationException(xstr);

                    switch (mode)
                    {
                        case EnumeratorMode.ByKey:
                            return currentKey;

                        case EnumeratorMode.ByValue:
                            return currentValue;

                        case EnumeratorMode.ByEntry:
                            return Entry;

                        default:
                            throw new NotSupportedException(mode + " is not a supported mode.");
                    }
                }
            }

            // ICloneable

            [NotNull]
            public object Clone()
            {
                var e = new DictionaryEnumerator(host, mode);
                e.stamp = stamp;
                e.pos = pos;
                e.size = size;
                e.currentKey = currentKey;
                e.currentValue = currentValue;
                e.invalid = invalid;
                return e;
            }
        }

        struct KeyEnumerator : IEnumerator<TKey>, IDisposable
        {
            const int NOT_STARTED = -2;

            // this MUST be -1, because we depend on it in move next.
            // we just decr the size, so, 0 - 1 == FINISHED
            const int FINISHED = -1;

            readonly SortedList<TKey, TValue> l;
            int idx;
            readonly int ver;

            internal KeyEnumerator([NotNull] SortedList<TKey, TValue> l)
            {
                this.l = l;
                idx = NOT_STARTED;
                ver = l.modificationCount;
            }

            public void Dispose()
            {
                idx = NOT_STARTED;
            }

            public bool MoveNext()
            {
                if (ver != l.modificationCount)
                    throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");

                if (idx == NOT_STARTED)
                    idx = l.Count;

                return idx != FINISHED && --idx != FINISHED;
            }

            public TKey Current
            {
                get
                {
                    if (idx < 0)
                        throw new InvalidOperationException();

                    return l.KeyAt(l.Count - 1 - idx);
                }
            }

            void IEnumerator.Reset()
            {
                if (ver != l.modificationCount)
                    throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");

                idx = NOT_STARTED;
            }

            object IEnumerator.Current => Current;
        }

        struct ValueEnumerator : IEnumerator<TValue>, IDisposable
        {
            const int NOT_STARTED = -2;

            // this MUST be -1, because we depend on it in move next.
            // we just decr the size, so, 0 - 1 == FINISHED
            const int FINISHED = -1;

            readonly SortedList<TKey, TValue> l;
            int idx;
            readonly int ver;

            internal ValueEnumerator([NotNull] SortedList<TKey, TValue> l)
            {
                this.l = l;
                idx = NOT_STARTED;
                ver = l.modificationCount;
            }

            public void Dispose()
            {
                idx = NOT_STARTED;
            }

            public bool MoveNext()
            {
                if (ver != l.modificationCount)
                    throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");

                if (idx == NOT_STARTED)
                    idx = l.Count;

                return idx != FINISHED && --idx != FINISHED;
            }

            public TValue Current
            {
                get
                {
                    if (idx < 0)
                        throw new InvalidOperationException();

                    return l.ValueAt(l.Count - 1 - idx);
                }
            }

            void IEnumerator.Reset()
            {
                if (ver != l.modificationCount)
                    throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");

                idx = NOT_STARTED;
            }

            object IEnumerator.Current => Current;
        }

        private class ListKeys : IList<TKey>, IReadOnlyList<TKey>, ICollection, IEnumerable
        {

            private readonly SortedList<TKey, TValue> host;

            public ListKeys([NotNull] SortedList<TKey, TValue> host)
            {
                if (host == null)
                    throw new ArgumentNullException();

                this.host = host;
            }

            // ICollection<TKey>

            public virtual void Add(TKey item)
            {
                throw new NotSupportedException();
            }

            public virtual bool Remove(TKey key)
            {
                throw new NotSupportedException();
            }

            public virtual void Clear()
            {
                throw new NotSupportedException();
            }

            public virtual void CopyTo(TKey[] array, int arrayIndex)
            {
                if (host.Count == 0)
                    return;
                if (array is null)
                    throw new ArgumentNullException(nameof(array));
                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException();
                if (arrayIndex >= array.Length)
                    throw new ArgumentOutOfRangeException("arrayIndex is greater than or equal to array.Length");
                if (Count > (array.Length - arrayIndex))
                    throw new ArgumentOutOfRangeException("Not enough space in array from arrayIndex to end of array");

                var j = arrayIndex;
                for (var i = 0; i < Count; ++i)
                    array[j++] = host.KeyAt(i);
            }

            public virtual bool Contains([NotNull] TKey item)
            {
                return host.IndexOfKey(item) > -1;
            }

            //
            // IList<TKey>
            //
            public virtual int IndexOf([NotNull] TKey item)
            {
                return host.IndexOfKey(item);
            }

            public virtual void Insert(int index, TKey item)
            {
                throw new NotSupportedException();
            }

            public virtual void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            public virtual TKey this[int index]
            {
                get
                {
                    return host.KeyAt(index);
                }
                set
                {
                    throw new NotSupportedException("attempt to modify a key");
                }
            }

            //
            // IEnumerable<TKey>
            //

            public virtual IEnumerator<TKey> GetEnumerator()
            {
                /* We couldn't use yield as it does not support Reset () */
                return new KeyEnumerator(host);
            }

            //
            // ICollection
            //

            public virtual int Count => host.Count;

            public virtual bool IsSynchronized => ((ICollection)host).IsSynchronized;

            public virtual bool IsReadOnly => true;

            public virtual object SyncRoot => ((ICollection)host).SyncRoot;

            public virtual void CopyTo(Array array, int arrayIndex)
            {
                host.CopyToArray(array, arrayIndex, EnumeratorMode.ByKey);
            }

            //
            // IEnumerable
            //

            IEnumerator IEnumerable.GetEnumerator()
            {
                for (var i = 0; i < host.Count; ++i)
                    yield return host.KeyAt(i);
            }
        }

        private class ListValues : IList<TValue>, IReadOnlyList<TValue>, ICollection, IEnumerable
        {

            private readonly SortedList<TKey, TValue> host;

            public ListValues([NotNull] SortedList<TKey, TValue> host)
            {
                if (host == null)
                    throw new ArgumentNullException();

                this.host = host;
            }

            // ICollection<TValue>

            public virtual void Add(TValue item)
            {
                throw new NotSupportedException();
            }

            public virtual bool Remove(TValue value)
            {
                throw new NotSupportedException();
            }

            public virtual void Clear()
            {
                throw new NotSupportedException();
            }

            public virtual void CopyTo(TValue[] array, int arrayIndex)
            {
                if (host.Count == 0)
                    return;
                if (array is null)
                    throw new ArgumentNullException(nameof(array));
                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException();
                if (arrayIndex >= array.Length)
                    throw new ArgumentOutOfRangeException("arrayIndex is greater than or equal to array.Length");
                if (Count > (array.Length - arrayIndex))
                    throw new ArgumentOutOfRangeException("Not enough space in array from arrayIndex to end of array");

                var j = arrayIndex;
                for (var i = 0; i < Count; ++i)
                    array[j++] = host.ValueAt(i);
            }

            public virtual bool Contains(TValue item)
            {
                return host.IndexOfValue(item) > -1;
            }

            //
            // IList<TValue>
            //
            public virtual int IndexOf(TValue item)
            {
                return host.IndexOfValue(item);
            }

            public virtual void Insert(int index, TValue item)
            {
                throw new NotSupportedException();
            }

            public virtual void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            public virtual TValue this[int index]
            {
                get
                {
                    return host.ValueAt(index);
                }
                set
                {
                    throw new NotSupportedException("attempt to modify a key");
                }
            }

            //
            // IEnumerable<TValue>
            //

            public virtual IEnumerator<TValue> GetEnumerator()
            {
                /* We couldn't use yield as it does not support Reset () */
                return new ValueEnumerator(host);
            }

            //
            // ICollection
            //

            public virtual int Count => host.Count;

            public virtual bool IsSynchronized => ((ICollection)host).IsSynchronized;

            public virtual bool IsReadOnly => true;

            public virtual object SyncRoot => ((ICollection)host).SyncRoot;

            public virtual void CopyTo(Array array, int arrayIndex)
            {
                host.CopyToArray(array, arrayIndex, EnumeratorMode.ByValue);
            }

            //
            // IEnumerable
            //

            IEnumerator IEnumerable.GetEnumerator()
            {
                for (var i = 0; i < host.Count; ++i)
                    yield return host.ValueAt(i);
            }
        }
    }
}
