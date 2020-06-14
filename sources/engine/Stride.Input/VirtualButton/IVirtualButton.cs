// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Input
{
    /// <summary>
    ///   Defines the interface of a virtual button, a generic representation of an input that can be
    ///   signaled from any valid input device.
    /// </summary>
    public interface IVirtualButton
    {
        /// <summary>
        ///   Gets the value associated with this virtual button from an input manager.
        /// </summary>
        /// <param name="manager">The input manager.</param>
        /// <returns>Value of the virtual button.</returns>
        float GetValue(InputManager manager);

        /// <summary>
        ///   Gets a value that indicates whether the button is currently down.
        /// </summary>
        /// <param name="manager">The input manager</param>
        /// <returns><c>true</c> if the button is currently down; <c>false</c> otherwise.</returns>
        bool IsDown(InputManager manager);

        /// <summary>
        ///   Gets a value that indicates whether the button has been pressed since the last frame.
        /// </summary>
        /// <param name="manager">The input manager</param>
        /// <returns><c>true</c> if the button has been pressed; <c>false</c> otherwise.</returns>
        bool IsPressed(InputManager manager);

        /// <summary>
        ///   Gets a value that indicates whether the button has been released since the last frame.
        /// </summary>
        /// <param name="manager">The input manager</param>
        /// <returns><c>true</c> if the button has been released; <c>false</c> otherwise.</returns>
        bool IsReleased(InputManager manager);
    }
}
