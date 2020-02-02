// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Reflection;
using Xenko.Core.Yaml.Serialization;

namespace Xenko.Core.Yaml
{
    /// <summary>
    /// A container used to serialize collection whose items have identifiers.
    /// </summary>
    /// <typeparam name="TItem">The type of item contained in the collection.</typeparam>
    [DataContract]
    public class CollectionWithItemIds<TItem> : OrderedDictionary<ItemId, TItem>
    {
    }
}
