// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Mathematics;

namespace Xenko.Input
{
    /// <summary>
    /// This class represents a sensor of type Gyroscope. It measures the rotation speed of device along the x/y/z axis.
    /// </summary>
    public interface IGyroscopeSensor : ISensorDevice
    {
        /// <summary>
        /// Gets the current rotation speed of the device along x/y/z axis.
        /// </summary>
        Vector3 RotationRate { get; }
    }
}