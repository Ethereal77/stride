// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core.Annotations;

namespace Xenko.Core.Presentation.Collections
{
    /// <summary>
    /// This interface regroups the <see cref="IList{T}"/> interface and the <see cref="IObservableCollection{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of item contained in the list.</typeparam>
    public interface IObservableList<T> : IList<T>, IObservableCollection<T>
    {
        [CollectionAccess(CollectionAccessType.UpdatedContent)]
        void AddRange([NotNull] IEnumerable<T> items);
    }
}
