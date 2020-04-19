// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Yaml.Serialization;

namespace Xenko.Core.Yaml
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
