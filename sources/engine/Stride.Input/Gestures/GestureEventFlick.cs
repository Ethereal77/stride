// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Mathematics;

namespace Stride.Input
{
    /// <summary>
    /// Event class for the Flick gesture.
    /// </summary>
    public sealed class GestureEventFlick : GestureEventTranslation
    {
        internal void Set(int numberOfFingers, TimeSpan time, GestureShape shape, Vector2 startPos, Vector2 currPos)
        {
            Set(GestureType.Flick, GestureState.Occurred, numberOfFingers, time, time, shape, startPos, currPos, currPos - startPos);
        }
    }
}
