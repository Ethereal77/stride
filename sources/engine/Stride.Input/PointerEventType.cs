// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Input
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