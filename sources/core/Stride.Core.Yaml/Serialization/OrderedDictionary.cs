// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Stride.Core.Yaml.Serialization
{
    public class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue>, IList<KeyValuePair<TKey, TValue>>, IDictionary
    {
        private readonly KeyedCollection items = new KeyedCollection();

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return items.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return items.Remove(item);
        }

        public void Add(TKey key, TValue value)
        {
            items.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public bool ContainsKey(TKey key)
        {
            return items.Contains(key);
        }

        public bool Remove(TKey key)
        {
            return items.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!items.Contains(key))
            {
                value = default(TValue);
                return false;
            }

            value = items[key].Value;
            return true;
        }

        public void Insert(int index, TKey key, TValue value)
        {
            items.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        public int IndexOf(KeyValuePair<TKey, TValue> item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, KeyValuePair<TKey, TValue> item)
        {
            items.Insert(index, item);
        }

        public int IndexOf(TKey key)
        {
            if (!items.Contains(key))
                return -1;

            return items.IndexOf(items[key]);
        }

        public KeyValuePair<TKey, TValue> this[int index] { get { return items[index]; } set { items[index] = value; } }

        public TValue this[TKey key]
        {
            get { return items[key].Value; }
            set
            {
                var item = new KeyValuePair<TKey, TValue>(key, value);
                var index = IndexOf(key);
                if (index != -1)
                    items[index] = item;
                else
                    items.Add(item);
            }
        }

        public ICollection<TKey> Keys { get { return items.Select(x => x.Key).ToList(); } }

        public ICollection<TValue> Values { get { return items.Select(x => x.Value).ToList(); } }

        object ICollection.SyncRoot => ((ICollection)items).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)items).IsSynchronized;

        object IDictionary.this[object key] { get { return this[(TKey)key]; } set { this[(TKey)key] = (TValue)value; } }

        ICollection IDictionary.Keys => (ICollection)Keys;

        ICollection IDictionary.Values => (ICollection)Values;

        bool IDictionary.IsFixedSize => false;

        bool IDictionary.Contains(object key) => ContainsKey((TKey)key);

        void IDictionary.Add(object key, object value) => Add((TKey)key, (TValue)value);

        void IDictionary.Remove(object key) => Remove((TKey)key);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)items).CopyTo(array, index);

        IDictionaryEnumerator IDictionary.GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class Enumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator<KeyValuePair<TKey, TValue>> enumerator;

            public Enumerator(OrderedDictionary<TKey, TValue> dictionary)
            {
                enumerator = dictionary.GetEnumerator();
            }

            public bool MoveNext() => enumerator.MoveNext();
            public void Reset() => enumerator.Reset();
            public object Current => enumerator.Current;
            public object Key => enumerator.Current.Key;
            public object Value => enumerator.Current.Value;
            public DictionaryEntry Entry => new DictionaryEntry(Key, Value);
        }

        class KeyedCollection : KeyedCollection<TKey, KeyValuePair<TKey, TValue>>
        {
            protected override TKey GetKeyForItem(KeyValuePair<TKey, TValue> item)
            {
                return item.Key;
            }
        }
    }
}
