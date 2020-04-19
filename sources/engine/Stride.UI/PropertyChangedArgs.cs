// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.UI
{
    /// <summary>
    /// An argument class containing information about a property that changed.
    /// </summary>
    /// <typeparam name="T">The type of the property that changed</typeparam>
    public class PropertyChangedArgs<T>
    {
        public T OldValue { get; internal set; }
        public T NewValue { get; internal set; }
    }
}
