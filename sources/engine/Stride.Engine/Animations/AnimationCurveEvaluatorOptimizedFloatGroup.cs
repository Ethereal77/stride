// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Animations
{
    public class AnimationCurveEvaluatorOptimizedFloatGroup : AnimationCurveEvaluatorOptimizedBlittableGroupBase<float>
    {
        protected unsafe override void ProcessChannel(ref Channel channel, CompressedTimeSpan currentTime, IntPtr location, float factor)
        {
            if (channel.InterpolationType == AnimationCurveInterpolationType.Cubic)
            {
                *(float*)(location + channel.Offset) = Interpolator.Cubic(
                    channel.ValuePrev.Value,
                    channel.ValueStart.Value,
                    channel.ValueEnd.Value,
                    channel.ValueNext.Value,
                    factor);
            }
            else if (channel.InterpolationType == AnimationCurveInterpolationType.Linear)
            {
                *(float*)(location + channel.Offset) = Interpolator.Linear(
                    channel.ValueStart.Value,
                    channel.ValueEnd.Value,
                    factor);
            }
            else if (channel.InterpolationType == AnimationCurveInterpolationType.Constant)
            {
                *(float*)(location + channel.Offset) = channel.ValueStart.Value;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
