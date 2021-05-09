// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Yaml.Serialization;

namespace Stride.Core.Yaml
{
    /// <summary>
    /// A container used to serialize dictionary whose entries have identifiers.
    /// </summary>
    /// <typeparam name="TKey">The type of key contained in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of value contained in the dictionary.</typeparam>
    [DataContract]
    public class DictionaryWithItemIds<TKey, TValue> : OrderedDictionary<KeyWithId<TKey>, TValue>
    {

    }
}
