// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Mathematics;

namespace Stride.Input
{
    /// <summary>
    /// Event class for the Tap gesture.
    /// </summary>
    public sealed class GestureEventTap : GestureEvent
    {
        /// <summary>
        /// The number of time the use successively touched the screen.
        /// </summary>
        public int NumberOfTaps { get; internal set; }

        /// <summary>
        /// The position of the tap.
        /// </summary>
        public Vector2 TapPosition { get; internal set; }
        
        internal void Set(TimeSpan takenTime, int numberOfFingers, int numberOfTaps, Vector2 position)
        {
            Type = GestureType.Tap;
            State = GestureState.Occurred;
            DeltaTime = takenTime;
            TotalTime = takenTime;
            NumberOfFinger = numberOfFingers;
            NumberOfTaps = numberOfTaps;
            TapPosition = position;
        }
    }
}
