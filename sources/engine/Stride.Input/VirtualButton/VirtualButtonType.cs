// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Input
{
    /// <summary>
    ///   Defines the different types of a <see cref="VirtualButton"/>.
    /// </summary>
    public enum VirtualButtonType
    {
        /// <summary>
        ///   A keyboard virtual button.
        /// </summary>
        Keyboard = 1 << 28,

        /// <summary>
        ///   A mouse virtual button.
        /// </summary>
        Mouse = 2 << 28,

        /// <summary>
        ///   A pointer virtual button.
        /// </summary>
        Pointer = 3 << 28,

        /// <summary>
        ///   A gamepad virtual button.
        /// </summary>
        GamePad = 4 << 28,
    }
}
