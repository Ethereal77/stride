// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Diagnostics;

namespace Stride.Games.Time
{
    /// <summary>
    /// Represent an absolute time measurement stopwatch. (with as few internal overhead as possible)
    /// It measures elapsed time in seconds between calls to Start method and Elapsed property.
    /// </summary>
    public class AbsoluteStopwatch
    {
        private long startTicks;

        /// <summary>
        /// Start the stopwatch. (use this method also to restart stopwatching)
        /// </summary>
        public void Start()
        {
            startTicks = Stopwatch.GetTimestamp();
        }

        /// <summary>
        /// Gets the time elapsed since previous call to Start method, in seconds.
        /// </summary>
        public double Elapsed
        {
            get
            {
                long elapsed = Stopwatch.GetTimestamp() - startTicks;
                if (elapsed < 0)
                    return 0.0;
                return (double)elapsed / (Stopwatch.Frequency);
            }
        }
    }
}
