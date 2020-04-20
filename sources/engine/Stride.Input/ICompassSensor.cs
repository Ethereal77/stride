// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Input
{
    /// <summary>
    /// This class represents a sensor of type compass. It measures the angle between the device and the north.
    /// </summary>
    public interface ICompassSensor : ISensorDevice
    {
        /// <summary>
        /// Gets the value of north heading, that is the angle (in radian) between the top of the device and north.
        /// </summary>
        float Heading { get; }
    }
}