// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Input
{
    internal class CompassSensor : Sensor, ICompassSensor
    {
        public float Heading { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompassSensor"/> class.
        /// </summary>
        public CompassSensor(IInputSource source, string systemName) : base(source, systemName, "Compass")
        {
        }
    }
}