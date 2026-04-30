// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

using Stride.Core;

namespace Stride.Animations
{
    public class AnimationCurveEvaluatorOptimizedBlittableGroup<T> : AnimationCurveEvaluatorOptimizedBlittableGroupBase<T>
    {
        protected override unsafe void ProcessChannel(ref Channel channel, CompressedTimeSpan currentTime, IntPtr location, float factor)
        {
            Unsafe.AsRef<T>((void*)(location + channel.Offset)) = channel.ValueStart.Value;
        }
    }
}
