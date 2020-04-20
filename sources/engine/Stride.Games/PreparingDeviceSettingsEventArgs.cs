// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Games
{
    /// <summary>
    /// Describes settings to apply before preparing a device for creation, used by <see cref="GraphicsDeviceManager.OnPreparingDeviceSettings"/>.
    /// </summary>
    public class PreparingDeviceSettingsEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreparingDeviceSettingsEventArgs" /> class.
        /// </summary>
        /// <param name="graphicsDeviceInformation">The graphics device information.</param>
        public PreparingDeviceSettingsEventArgs(GraphicsDeviceInformation graphicsDeviceInformation)
        {
            GraphicsDeviceInformation = graphicsDeviceInformation;
        }

        /// <summary>
        /// Gets the graphics device information.
        /// </summary>
        /// <value>The graphics device information.</value>
        public GraphicsDeviceInformation GraphicsDeviceInformation { get; private set; }
    }
}
