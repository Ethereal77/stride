// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Input
{
    /// <summary>
    ///   Defines the inteface for interacting with a mouse device, with specific functionality to mouse input
    ///   such as buttons, wheels, mouse locking and setting cursor position.
    /// </summary>
    public interface IMouseDevice : IPointerDevice
    {
        /// <summary>
        ///   Gets the normalized position of the mouse inside the window.
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        ///   Gets the mouse delta, the difference in <see cref="Position"/> since the last frame.
        /// </summary>
        Vector2 Delta { get; }

        /// <summary>
        ///   Gets the mouse buttons that have been pressed since the last frame.
        /// </summary>
        Core.Collections.IReadOnlySet<MouseButton> PressedButtons { get; }

        /// <summary>
        ///   Gets the mouse buttons that have been released since the last frame.
        /// </summary>
        Core.Collections.IReadOnlySet<MouseButton> ReleasedButtons { get; }

        /// <summary>
        ///   Gets the mouse buttons that are currently down.
        /// </summary>
        Core.Collections.IReadOnlySet<MouseButton> DownButtons { get; }

        /// <summary>
        ///   Gets if the mouse position is locked to the screen.
        /// </summary>
        bool IsPositionLocked { get; }

        /// <summary>
        ///   Locks the mouse position to the screen.
        /// </summary>
        /// <param name="forceCenter">Value indicating whether to force the mouse position to the center of the screen.</param>
        void LockPosition(bool forceCenter = false);

        /// <summary>
        ///   Unlocks the mouse position if it was locked.
        /// </summary>
        void UnlockPosition();

        /// <summary>
        ///   Attempts to set the pointer position.
        /// </summary>
        /// <param name="normalizedPosition">Normalized position of the pointer.</param>
        /// <remarks>This only makes sense for mouse pointers.</remarks>
        void SetPosition(Vector2 normalizedPosition);
    }
}
