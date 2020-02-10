// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Core.Annotations;

namespace Xenko.Core.Assets.Editor.Extensions
{
    public static class ViewModelExtensions
    {
        /// <summary>
        /// Checks whether the given collection is comprised of children of the same parent.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns><c>true</c> if all children have the same parent; otherwise, <c>false</c>.</returns>
        public static bool AllSiblings([NotNull] this IEnumerable<IChildViewModel> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            var parents = collection.Select(x => x.GetParent()).ToList();
            return parents.Count > 0 && parents.All(x => x == parents[0]);
        }
    }
}
