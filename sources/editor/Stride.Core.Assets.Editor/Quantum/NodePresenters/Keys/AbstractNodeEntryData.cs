// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

using Xenko.Core.Assets.Editor.Quantum.NodePresenters.Commands;
using Xenko.Core;
using Xenko.Core.Presentation.Quantum.Presenters;

namespace Xenko.Core.Assets.Editor.Quantum.NodePresenters.Keys
{
    public static class AbstractNodeEntryData
    {
        public const string AbstractNodeMatchingEntries = nameof(AbstractNodeMatchingEntries);
        public static readonly PropertyKey<IEnumerable<AbstractNodeEntry>> Key = new PropertyKey<IEnumerable<AbstractNodeEntry>>(AbstractNodeMatchingEntries, typeof(AbstractNodeEntryData), new PropertyCombinerMetadata(CombineProperty));

        public static object CombineProperty(IEnumerable<object> properties)
        {
            var result = new HashSet<AbstractNodeEntry>();
            var hashSets = new List<HashSet<AbstractNodeEntry>>();
            hashSets.AddRange(properties.Cast<IEnumerable<AbstractNodeEntry>>().Select(x => new HashSet<AbstractNodeEntry>(x)));
            result = hashSets[0];
            // We display only component types that are available for all entities
            for (var i = 1; i < hashSets.Count; ++i)
            {
                result.IntersectWith(hashSets[i]);
            }
            return result.OrderBy(x => x.Order).ThenBy(x => x.DisplayValue);
        }
    }
}
