// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Input
{
    /// <summary>
    ///   Defines the interface for interacting with a keyboard device.
    /// </summary>
    public interface IKeyboardDevice : IInputDevice
    {
        /// <summary>
        ///   Gets the keys that have been pressed since the last frame.
        /// </summary>
        Core.Collections.IReadOnlySet<Keys> PressedKeys { get; }

        /// <summary>
        ///   Gets the keys that have been released since the last frame.
        /// </summary>
        Core.Collections.IReadOnlySet<Keys> ReleasedKeys { get; }

        /// <summary>
        ///   Gets the keys that are currently down on this keyboard.
        /// </summary>
        Core.Collections.IReadOnlySet<Keys> DownKeys { get; }
    }
}
