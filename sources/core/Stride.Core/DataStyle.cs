// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core
{
    /// <summary>
    /// Specifies the style used for textual serialization when an array/list or a dictionary/map must
    /// be serialized.
    /// </summary>
    public enum DataStyle
    {
        /// <summary>
        /// Let the emitter choose the style.
        /// </summary>
        Any,

        /// <summary>
        /// The normal style (One line per item, structured by space).
        /// </summary>
        Normal,

        /// <summary>
        /// The compact style (style embraced by [] or {})
        /// </summary>
        Compact,
    }
}
