// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Input
{
    internal class UserAccelerationSensor : Sensor, IUserAccelerationSensor
    {
        public Vector3 Acceleration { get; internal set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccelerationSensor"/> class.
        /// </summary>
        public UserAccelerationSensor(IInputSource source, string systemName) : base(source, systemName, "User Acceleration")
        {
        }
    }
}