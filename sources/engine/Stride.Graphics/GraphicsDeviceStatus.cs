// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Graphics
{
    /// <summary>
    /// Describes the current status of a <see cref="GraphicsDevice"/>.
    /// </summary>
    public enum GraphicsDeviceStatus
    {
        /// <summary>
        /// The device is running fine.
        /// </summary>
        Normal,

        /// <summary>
        /// The video card has been physically removed from the system, or a driver upgrade for the video card has occurred. The application should destroy and recreate the device.
        /// </summary>
        Removed,

        /// <summary>
        /// The application's device failed due to badly formed commands sent by the application. This is an design-time issue that should be investigated and fixed.
        /// </summary>
        Hung,

        /// <summary>
        /// The device failed due to a badly formed command. This is a run-time issue; The application should destroy and recreate the device.
        /// </summary>
        Reset,

        /// <summary>
        /// The driver encountered a problem and was put into the device removed state.
        /// </summary>
        InternalError,

        /// <summary>
        /// The application provided invalid parameter data; this must be debugged and fixed before the application is released.
        /// </summary>
        InvalidCall,
    }
}
