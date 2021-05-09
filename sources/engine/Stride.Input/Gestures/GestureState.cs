// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Input
{
    /// <summary>
    /// The different possible states of a gestures.
    /// </summary>
    public enum GestureState
    {
        /// <summary>
        /// A discrete gesture has occurred.
        /// </summary>
        Occurred,
        /// <summary>
        /// A continuous gesture has started.
        /// </summary>
        Began,
        /// <summary>
        /// A continuous gesture parameters changed.
        /// </summary>
        Changed,
        /// <summary>
        /// A continuous gesture has stopped.
        /// </summary>
        Ended,
    }
}
