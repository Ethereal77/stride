// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Assets.Editor.ViewModel
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
