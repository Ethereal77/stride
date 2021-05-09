// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.UI
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
