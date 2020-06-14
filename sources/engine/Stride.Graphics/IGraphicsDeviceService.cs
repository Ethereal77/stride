// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Graphics
{
    /// <summary>
    ///   Defines the interface of a service providing methods to get access to the <see cref="Graphics.GraphicsDevice"/>.
    /// </summary>
    public interface IGraphicsDeviceService
    {
        /// <summary>
        ///   Event raised when a device is created.
        /// </summary>
        event EventHandler<EventArgs> DeviceCreated;

        /// <summary>
        ///   Event raised when a device is being disposed.
        /// </summary>
        event EventHandler<EventArgs> DeviceDisposing;

        /// <summary>
        ///   Event raised when a device state is reset.
        /// </summary>
        event EventHandler<EventArgs> DeviceReset;

        /// <summary>
        ///   Event raised when a device is being reset.
        /// </summary>
        event EventHandler<EventArgs> DeviceResetting;

        /// <summary>
        ///   Gets the current graphcs device.
        /// </summary>
        /// <value>The graphics device.</value>
        GraphicsDevice GraphicsDevice { get; }
    }
}
