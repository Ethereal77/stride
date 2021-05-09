// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Input
{
    internal class AccelerometerSensor : Sensor, IAccelerometerSensor
    {
        public Vector3 Acceleration { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelerometerSensor"/> class.
        /// </summary>
        public AccelerometerSensor(IInputSource source, string systemName) : base(source, systemName, "Accelerometer")
        {
        }
    }
}