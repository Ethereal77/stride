// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1402 // File may only contain a single type

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.Games.Time
{
    public interface ITimedValue<out T>
    {
        double Time { get; }
        T Value { get; }
    }

    public class ReadOnlyTimedValue<T> : ITimedValue<T>
    {
        public ReadOnlyTimedValue(double time, T value)
        {
            Time = time;
            Value = value;
        }

        public ReadOnlyTimedValue(ITimedValue<T> timedValue)
        {
            Time = timedValue.Time;
            Value = timedValue.Value;
        }

        public double Time { get; private set; }
        public T Value { get; private set; }
    }

    public class TimedValue<T> : ITimedValue<T>
    {
        public TimedValue(double time, T value)
        {
            Time = time;
            Value = value;
        }

        public double Time { get; set; }
        public T Value { get; set; }
    }
}
