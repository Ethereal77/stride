// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Stride.Core.Assets.Editor.Quantum.NodePresenters.Commands;
using Stride.Core;
using Stride.Core.Presentation.Quantum.Presenters;

namespace Stride.Core.Assets.Editor.Quantum.NodePresenters.Keys
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
