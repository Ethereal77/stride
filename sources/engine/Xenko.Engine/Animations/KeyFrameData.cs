// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Globalization;
using System.Runtime.InteropServices;

using Xenko.Core;

namespace Xenko.Animations
{
    /// <summary>
    /// A single key frame value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyFrameData<T>
    {
        public KeyFrameData(CompressedTimeSpan time, T value)
        {
            Time = time;
            Value = value;
        }

        public CompressedTimeSpan Time;
        public T Value;

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Time: {0} Value:{1}", Time.Ticks, Value);
        }
    }
}
