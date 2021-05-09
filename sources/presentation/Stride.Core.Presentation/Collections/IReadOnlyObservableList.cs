// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Core.Presentation.Collections
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
