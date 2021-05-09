// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Input
{
    /// <summary>
    /// An event used when a device was changed
    /// </summary>
    public class DeviceChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The input source this device belongs to
        /// </summary>
        public IInputSource Source;

        /// <summary>
        /// The device that changed
        /// </summary>
        public IInputDevice Device;

        /// <summary>
        /// The type of change that happened
        /// </summary>
        public DeviceChangedEventType Type;
    }
}