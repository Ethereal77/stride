// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Annotations;

namespace Stride.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue TryGetValue<TKey, TValue>([NotNull] this IReadOnlyDictionary<TKey, TValue> thisObject, [NotNull] TKey key)
        {
            TValue result;
            thisObject.TryGetValue(key, out result);
            return result;
        }

        public static void AddRange<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> thisObject, [NotNull] IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                thisObject.Add(keyValuePair);
            }
        }

        public static void Merge<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> thisObject, [NotNull] IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                thisObject[keyValuePair.Key] = keyValuePair.Value;
            }
        }
    }
}
