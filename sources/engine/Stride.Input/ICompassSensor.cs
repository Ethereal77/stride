// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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