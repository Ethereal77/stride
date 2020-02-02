// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Assets.Editor.ViewModel
{
    /// <summary>
    /// An flag enum representing the modifier keys currently active when invoking methods of <see cref="IAddChildViewModel"/>.
    /// </summary>
    [Flags]
    public enum AddChildModifiers
    {
        /// <summary>
        /// No modifier key is pressed.
        /// </summary>
        None = 0,
        /// <summary>
        /// The Ctrl key is pressed.
        /// </summary>
        Ctrl = 1,
        /// <summary>
        /// The Shift key is pressed.
        /// </summary>
        Shift = 2,
        /// <summary>
        /// The Alt key is pressed.
        /// </summary>
        Alt = 4,
    }
}
