// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

namespace Xenko.Core.Presentation.Collections
{
    /// <summary>
    /// This interface regroups the <see cref="IReadOnlyList{T}"/> interface, the
    /// <see cref="System.ComponentModel.INotifyPropertyChanged"/> interface, and the <see cref="System.Collections.Specialized.INotifyCollectionChanged"/>
    /// interface. It has no additional members.
    /// </summary>
    /// <typeparam name="T">The type of item contained in the collection.</typeparam>
    public interface IReadOnlyObservableList<out T> : IReadOnlyList<T>, IReadOnlyObservableCollection<T>
    {
    }
}
