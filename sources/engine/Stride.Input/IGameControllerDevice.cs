// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Stride.Input
{
    /// <summary>
    ///   Defines the interface for interacting with game controller devices.
    /// </summary>
    public interface IGameControllerDevice : IInputDevice
    {
        /// <summary>
        ///   Gets the product Id of the device.
        /// </summary>
        Guid ProductId { get; }

        /// <summary>
        ///   Gets information about the buttons on this game controller.
        /// </summary>
        IReadOnlyList<GameControllerButtonInfo> ButtonInfos { get; }

        /// <summary>
        ///   Gets information about the axis on this game controller.
        /// </summary>
        IReadOnlyList<GameControllerAxisInfo> AxisInfos { get; }

        /// <summary>
        ///   Gets information about the directions on this game controller.
        /// </summary>
        IReadOnlyList<GameControllerDirectionInfo> DirectionInfos { get; }

        /// <summary>
        ///   Gets the buttons that have been pressed since the last frame.
        /// </summary>
        Core.Collections.IReadOnlySet<int> PressedButtons { get; }

        /// <summary>
        ///   Gets the buttons that have been released since the last frame.
        /// </summary>
        Core.Collections.IReadOnlySet<int> ReleasedButtons { get; }

        /// <summary>
        ///   Gets the buttons that are down.
        /// </summary>
        Core.Collections.IReadOnlySet<int> DownButtons { get; }

        /// <summary>
        ///   Retrieves the state of a single axis.
        /// </summary>
        /// <param name="index">The axis' index, as exposed in <see cref="AxisInfos"/>.</param>
        /// <returns>The value read directly from the axis.</returns>
        float GetAxis(int index);

        /// <summary>
        ///   Retrieves the state of a single direction.
        /// </summary>
        /// <param name="index">The direction index, as exposed in <see cref="DirectionInfos"/>.</param>
        /// <returns>The current state of the direction.</returns>
        Direction GetDirection(int index);
    }
}
