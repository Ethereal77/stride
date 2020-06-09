// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Diagnostics;

namespace Stride.Core
{
    public class ThreadThrottler
    {
        private static readonly double ONE_MILLISECOND = Stopwatch.Frequency / 1000.0;

        private long timestamp;
        private long minElapsedTicks;
        private long elapsedTicksError;
        private double spinwaitWindow;

        /// <summary>
        ///   Gets or sets the minimum amount of time allowed between update (logic) ticks.
        /// </summary>
        /// <value>
        ///   Minimum allowed time between ticks. Set to zero to disable throttling.
        /// </value>
        /// <remarks>
        ///   Conversion is lossy, getting this value back might not return the same value you set it to.
        /// </remarks>
        public TimeSpan MinimumElapsedTime
        {
            get => ToSpan(minElapsedTicks);
            set => minElapsedTicks = (long) ((double) Stopwatch.Frequency / TimeSpan.TicksPerSecond * value.Ticks);
        }

        /// <summary>
        ///   Gets the type of throttler used.
        /// </summary>
        /// <value>
        ///   Type of throttling. Default is <see cref="ThrottlerType.Standard"/>.
        /// </value>
        public ThrottlerType Type { get; private set; }


        /// <summary>
        ///   Initializes a new instance of the <see cref="ThreadThrottler"/> class.
        /// </summary>
        public ThreadThrottler()
        {
            timestamp = Stopwatch.GetTimestamp();

            SetToStandard();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ThreadThrottler"/> class.
        /// </summary>
        /// <param name="minimumElapsedTime">Minimum time allowed between ticks.</param>
        public ThreadThrottler(TimeSpan minimumElapsedTime) : this()
        {
            MinimumElapsedTime = minimumElapsedTime;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ThreadThrottler"/> class.
        /// </summary>
        /// <param name="maxFrequency">The maximum frequency (ticks per second) to allow.</param>
        public ThreadThrottler(int maxFrequency) : this()
        {
            SetMaxFrequency(maxFrequency);
        }


        /// <summary>
        ///   Sets the max allowed frequency (ticks per second) this throttler will allow.
        /// </summary>
        /// <remarks>
        ///   This method effectively transforms the <paramref name="frequencyMax"/> parameter from ticks per second
        ///   to the closest seconds per frame equivalent for it.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The frequency must be positive and greater than zero.</exception>
        public void SetMaxFrequency(int frequencyMax)
        {
            if (frequencyMax <= 0)
                throw new ArgumentOutOfRangeException(nameof(frequencyMax), "The frequency must be positive and greater than zero.");

            minElapsedTicks = (long) (Stopwatch.Frequency / (double) frequencyMax);
        }

        /// <summary>
        ///   Sets this throttler to "standard mode", in which it saves CPU cycles while waiting. This is the least precise mode but
        ///   also the lightest. This is the default mode.
        /// </summary>
        public void SetToStandard()
        {
            Type = ThrottlerType.Standard;
            spinwaitWindow = 0;
        }

        /// <summary>
        ///   Sets this throttler to "automatic precise mode", in which most of the timings will be perfectly precise to the system
        ///   timer at the cost of higher CPU usage.
        /// </summary>
        /// <remarks>
        ///   This mode uses and automatically scales a spin-waiting window based on system responsiveness to find the right balance
        ///   between <see cref="System.Threading.Thread.Sleep"/> calls and spin-waiting.
        /// </remarks>
        public void SetToPreciseAuto()
        {
            // Avoid window reset if already precise auto
            if (Type == ThrottlerType.PreciseAuto)
                return;

            Type = ThrottlerType.PreciseAuto;
            spinwaitWindow = 0;
        }

        /// <summary>
        ///   Sets this throttler to "manual precise mode", in which depending on the provided value, timings will be perfectly precise
        ///   to the system timer at the cost of higher CPU usage.
        /// </summary>
        /// <param name="spinwaitTicks">Duration of the spin-waiting window.</param>
        /// <remarks>
        ///   This mode uses the <paramref name="spinwaitTicks"/> value as the duration of the spin-waiting window.
        ///   The format of this value is based on <see cref="Stopwatch.Frequency"/>.
        /// </remarks>
        public void SetToPreciseManual(long spinwaitTicks)
        {
            Type = ThrottlerType.PreciseManual;
            spinwaitWindow = spinwaitTicks;
        }

        /// <summary>
        ///   Forces this thread to sleep when the time elapsed since the last call is lower than <see cref="MinimumElapsedTime"/>.
        ///   It will sleep for the time remaining to reach <see cref="MinimumElapsedTime"/>.
        ///   Use this function inside a loop when you want to lock to a specific rate.
        /// </summary>
        /// <param name="elapsedTime">
        ///   After this method returns, contains the time since the last call.
        ///   You can use it as your "delta time".
        /// </param>
        /// <returns><c>true</c> if the thread had to throttle; <c>false</c> otherwise.</returns>
        public bool Throttle(out TimeSpan elapsedTime)
        {
            var throttled = Throttle(out long stamp);
            elapsedTime = ToSpan(stamp);
            return throttled;
        }

        /// <summary>
        ///   Forces this thread to sleep when the time elapsed since the last call is lower than <see cref="MinimumElapsedTime"/>.
        ///   It will sleep for the time remaining to reach <see cref="MinimumElapsedTime"/>.
        ///   Use this function inside a loop when you want to lock to a specific rate.
        /// </summary>
        /// <param name="elapsedSeconds">
        ///   After this method returns, contains the time since the last call in seconds.
        ///   You can use it as your "delta time".
        /// </param>
        /// <returns><c>true</c> if the thread had to throttle; <c>false</c> otherwise.</returns>
        public bool Throttle(out double elapsedSeconds)
        {
            var throttled = Throttle(out long outTimestamp);
            elapsedSeconds = (double) outTimestamp / Stopwatch.Frequency;
            return throttled;
        }

        /// <summary>
        ///   Forces this thread to sleep when the time elapsed since the last call is lower than <see cref="MinimumElapsedTime"/>.
        ///   It will sleep for the time remaining to reach <see cref="MinimumElapsedTime"/>.
        ///   Use this function inside a loop when you want to lock to a specific rate.
        /// </summary>
        /// <param name="elapsedTicks">
        ///   After this method returns, contains the time since the last call in ticks (see <see cref="TimeSpan.Ticks"/>).
        ///   You can use it as your "delta time".
        /// </param>
        /// <returns><c>true</c> if the thread had to throttle; <c>false</c> otherwise.</returns>
        public bool Throttle(out long elapsedTicks)
        {
            bool throttled = false;

            try
            {
                // Reduce window to account for extreme cases
                if (Type == ThrottlerType.PreciseAuto)
                {
                    spinwaitWindow -= spinwaitWindow / 10;
                }

                while (true)
                {
                    // If < 1 ms, exit sleep loop
                    var idleDuration = minElapsedTicks - (Stopwatch.GetTimestamp() - timestamp - elapsedTicksError + spinwaitWindow);
                    if (idleDuration < ONE_MILLISECOND)
                    {
                        if (Type == ThrottlerType.Standard)
                            return false;

                        break;
                    }

                    throttled = true;
                    if (Type == ThrottlerType.PreciseAuto)
                    {
                        var sleepStart = Stopwatch.GetTimestamp();
                        Utilities.Sleep(1);

                        // Include excessive time sleep took on top of the time we specified
                        spinwaitWindow += Stopwatch.GetTimestamp() - sleepStart - ONE_MILLISECOND;
                        // Average to account for general system responsiveness
                        spinwaitWindow /= 2.0;
                    }
                    else if (Type == ThrottlerType.PreciseManual)
                    {
                        Utilities.Sleep(1);
                    }
                    else
                    {
                        // Don't let standard spinwait
                        Utilities.Sleep((int)(idleDuration / ONE_MILLISECOND));
                        return true;
                    }
                }

                var goalTimestamp = timestamp + minElapsedTicks + elapsedTicksError;
                if (Stopwatch.GetTimestamp() < goalTimestamp)
                {
                    // Spinwait for the rest of the duration
                    throttled = true;
                    while (Stopwatch.GetTimestamp() < goalTimestamp) { }
                }

                return throttled;
            }
            finally
            {
                var newStamp = Stopwatch.GetTimestamp();
                elapsedTicks = newStamp - timestamp;
                timestamp = newStamp;
                if (throttled)
                {
                    // Include time to catch-up or loose when our waiting method is lossy
                    elapsedTicksError += minElapsedTicks - elapsedTicks;
                }
                else
                {
                    elapsedTicksError = 0;
                }
            }
        }

        private static TimeSpan ToSpan(long timestamp)
        {
            return new TimeSpan(timestamp == 0 ? 0 : (long) (timestamp * TimeSpan.TicksPerSecond / (double) Stopwatch.Frequency));
        }
    }

    public enum ThrottlerType
    {
        Standard,
        PreciseManual,
        PreciseAuto,
    }
}
