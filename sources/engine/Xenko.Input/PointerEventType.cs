// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Input
{
    /// <summary>
    /// State of a pointer event.
    /// </summary>
    public enum PointerEventType
    {
        /// <summary>
        /// The pointer just started to hit the digitizer.
        /// </summary>
        Pressed,

        /// <summary>
        /// The pointer is moving on the digitizer.
        /// </summary>
        Moved,

        /// <summary>
        /// The pointer just released pressure to the digitizer.
        /// </summary>
        Released,

        /// <summary>
        /// The pointer has been canceled.
        /// </summary>
        Canceled,
    }
}