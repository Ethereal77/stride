// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Mathematics;

namespace Stride.Input
{
    /// <summary>
    /// Event class for the LongPress gesture.
    /// </summary>
    public sealed class GestureEventLongPress : GestureEvent
    {
        /// <summary>
        /// The position where the LongPress gesture happened.
        /// </summary>
        public Vector2 Position { get; internal set; }

        internal void Set(int numberOfFinger, TimeSpan time, Vector2 position)
        {
            State = GestureState.Occurred;
            Type = GestureType.LongPress;
            NumberOfFinger = numberOfFinger;
            DeltaTime = time;
            TotalTime = time;
            Position = position;
        }
    }
}
