// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Reflection;
using Stride.Core.Yaml.Serialization;

namespace Stride.Core.Yaml
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
