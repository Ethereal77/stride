// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Collections
{
    /// <summary>
    /// Array helper for a particular type, useful to get an empty array.
    /// </summary>
    /// <typeparam name="T">Type of the array element</typeparam>
    public struct ArrayHelper<T>
    {
        /// <summary>
        /// An empty array of the specified <see cref="T"/> element type.
        /// </summary>
        public static readonly T[] Empty = new T[0];
    }
}
