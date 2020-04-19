// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Mathematics;

namespace Xenko.Input
{
    /// <summary>
    /// Event class for the Drag gesture.
    /// </summary>
    public sealed class GestureEventDrag : GestureEventTranslation
    {
        internal void Set(GestureState state, int numberOfFingers, TimeSpan deltaTime, TimeSpan totalTime,
                                    GestureShape shape, Vector2 startPos, Vector2 currPos, Vector2 deltaTrans)
        {
            Set(GestureType.Drag, state, numberOfFingers, deltaTime, totalTime, shape, startPos, currPos, deltaTrans);
        }
    }
}
