// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Graphics
{
    /// <summary>
    /// Service providing method to access GraphicsDevice life-cycle.
    /// </summary>
    public interface IGraphicsDeviceService
    {
        /// <summary>
        /// Occurs when a device is created.
        /// </summary>
        event EventHandler<EventArgs> DeviceCreated;

        /// <summary>
        /// Occurs when a device is disposing.
        /// </summary>
        event EventHandler<EventArgs> DeviceDisposing;

        /// <summary>
        /// Occurs when a device is reseted.
        /// </summary>
        event EventHandler<EventArgs> DeviceReset;

        /// <summary>
        /// Occurs when a device is resetting.
        /// </summary>
        event EventHandler<EventArgs> DeviceResetting;

        /// <summary>
        /// Gets the current graphcs device.
        /// </summary>
        /// <value>The graphics device.</value>
        GraphicsDevice GraphicsDevice { get; }
    }
}
