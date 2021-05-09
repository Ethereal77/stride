// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Input
{
    /// <summary>
    /// This class represents a sensor of type Accelerometer. It measures the acceleration forces (including gravity) applying on the device.
    /// </summary>
    public interface IAccelerometerSensor : ISensorDevice
    {
        /// <summary>
        /// Gets the current acceleration applied on the device (in meters/seconds^2).
        /// </summary>
        Vector3 Acceleration { get; }
    }
}