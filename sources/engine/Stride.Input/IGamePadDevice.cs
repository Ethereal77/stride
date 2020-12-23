// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Input
{
    /// <summary>
    ///   Defines the interface for interacting with a gamepad.
    /// </summary>
    /// <remarks>
    ///   A gamepad is a game controller that has a fixed button mapping, stored in <see cref="State"/>.
    /// </remarks>
    public interface IGamePadDevice : IInputDevice
    {
        /// <summary>
        ///   Gets the product Id of the device.
        /// </summary>
        Guid ProductId { get; }

        /// <summary>
        ///   Gets the state of the gamepad.
        /// </summary>
        GamePadState State { get; }

        /// <summary>
        ///   Gets or sets the index of the gamepad assigned by the input manager.
        /// </summary>
        /// <value>
        ///   Index of the gamepad. If <see cref="CanChangeIndex"/> is <c>false</c>, this value can not be changed.
        /// </value>
        int Index { get; set; }

        /// <summary>
        ///   Gets a value indicating whether the index of this gamepad can be changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the <see cref="Index"/> can be changed; <c>false</c> otherwise.
        /// </value>
        bool CanChangeIndex { get; }

        /// <summary>
        ///   Gets the gamepad buttons that have been pressed since the last frame.
        /// </summary>
        Core.Collections.IReadOnlySet<GamePadButton> PressedButtons { get; }

        /// <summary>
        ///   Gets the gamepad buttons that have been released since the last frame.
        /// </summary>
        Core.Collections.IReadOnlySet<GamePadButton> ReleasedButtons { get; }

        /// <summary>
        ///   Gets the gamepad buttons that are down.
        /// </summary>
        Core.Collections.IReadOnlySet<GamePadButton> DownButtons { get; }

        /// <summary>
        ///   Raised if the <see cref="Index"/> assigned to this gamepad has changed.
        /// </summary>
        event EventHandler<GamePadIndexChangedEventArgs> IndexChanged;

        /// <summary>
        ///   Sets the vibration of the gamepad.
        /// </summary>
        /// <param name="smallLeft">The small left side motor. Values range from 0 (off) to 1 (maximum vibration).</param>
        /// <param name="smallRight">The small right side motor. Values range from 0 (off) to 1 (maximum vibration).</param>
        /// <param name="largeLeft">The large left side motor. Values range from 0 (off) to 1 (maximum vibration).</param>
        /// <param name="largeRight">The large right side motor. Values range from 0 (off) to 1 (maximum vibration).</param>
        /// <remarks>
        ///   This method sets 4 vibration values for the motors on the device. If less than 4 motors are available,
        ///   it approximates the effect.
        ///   <para/>
        ///   Currently vibration is only supported on Windows for XInput devices and supported gamepads.
        /// </remarks>
        void SetVibration(float smallLeft, float smallRight, float largeLeft, float largeRight);
    }
}
