// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Mathematics;

namespace Stride.Animations
{
    public class AnimationCurveEvaluatorDirectFloatGroup : AnimationCurveEvaluatorDirectBlittableGroupBase<float>
    {
        protected unsafe override void ProcessChannel(ref Channel channel, CompressedTimeSpan newTime, IntPtr location)
        {
            SetTime(ref channel, newTime);

            var currentTime = channel.CurrentTime;
            var currentIndex = channel.CurrentIndex;

            var keyFrames = channel.Curve.KeyFrames;
            var keyFramesItems = keyFrames.Items;
            var keyFramesCount = keyFrames.Count;

            // Extract data
            int timeStart = keyFrames[currentIndex + 0].Time.Ticks;
            int timeEnd = keyFrames[currentIndex + 1].Time.Ticks;

            // Compute interpolation factor and avoid NaN operations when timeStart >= timeEnd
            float t = (timeEnd <= timeStart) ? 0 : ((float)currentTime.Ticks - (float)timeStart) / ((float)timeEnd - (float)timeStart);

            if (channel.InterpolationType == AnimationCurveInterpolationType.Cubic)
            {
                *(float*)(location + channel.Offset) = Interpolator.Cubic(
                    keyFramesItems[currentIndex > 0 ? currentIndex - 1 : 0].Value,
                    keyFramesItems[currentIndex].Value,
                    keyFramesItems[currentIndex + 1].Value,
                    keyFramesItems[currentIndex + 2 >= keyFramesCount ? currentIndex + 1 : currentIndex + 2].Value,
                    t);
            }
            else if (channel.InterpolationType == AnimationCurveInterpolationType.Linear)
            {
                *(float*)(location + channel.Offset) = MathUtil.Lerp(keyFramesItems[currentIndex].Value, keyFramesItems[currentIndex + 1].Value, t);
            }
            else if (channel.InterpolationType == AnimationCurveInterpolationType.Constant)
            {
                *(float*)(location + channel.Offset) = keyFrames[currentIndex].Value;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
