// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Stride.Input
{
    /// <summary>
    ///   Defines the interface for interacting with a generic input device.
    /// </summary>
    public interface IInputDevice
    {
        /// <summary>
        ///   Gets the name of the device
        /// </summary>
        string Name { get; }

        /// <summary>
        ///   Gets the unique identifier of the device.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        ///   Gets or sets the device priority.
        ///   Larger values means higher priority when selecting the first device of some type.
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        ///   Gets the input source the device belongs to.
        /// </summary>
        IInputSource Source { get; }

        /// <summary>
        ///   Updates the input device, filling a list with input events that were generated by this device this frame.
        /// </summary>
        /// <param name="inputEvents">A list that gets filled with input events that were generated since the last frame.</param>
        /// <remarks>
        ///   Input devices are always updated after their respective input source.
        /// </remarks>
        void Update(List<InputEvent> inputEvents);
    }
}
