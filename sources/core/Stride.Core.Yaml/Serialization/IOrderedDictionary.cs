// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Core.Yaml.Serialization
{
    public interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        void Insert(int index, TKey key, TValue value);
        void RemoveAt(int index);
        int IndexOf(TKey key);
        KeyValuePair<TKey, TValue> this[int index] { get; set; }
    }
}