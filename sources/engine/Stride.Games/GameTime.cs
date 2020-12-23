// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Games
{
    /// <summary>
    ///   Represents a spapshot of the current time used for variable-step (real time) or fixed-step (game time) games.
    /// </summary>
    public class GameTime
    {
        private TimeSpan accumulatedElapsedTime;
        private int accumulatedFrameCountPerSecond;

        private double factor;


        /// <summary>
        ///   Initializes a new instance of the <see cref="GameTime" /> class.
        /// </summary>
        public GameTime()
        {
            accumulatedElapsedTime = TimeSpan.Zero;
            factor = 1;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GameTime" /> class.
        /// </summary>
        /// <param name="totalTime">The total game time since the start of the game.</param>
        /// <param name="elapsedTime">The elapsed game time since the last update.</param>
        public GameTime(TimeSpan totalTime, TimeSpan elapsedTime)
        {
            Total = totalTime;
            Elapsed = elapsedTime;
            accumulatedElapsedTime = TimeSpan.Zero;
            factor = 1;
        }


        /// <summary>
        ///   Gets the elapsed game time since the last update.
        /// </summary>
        /// <value>The elapsed game time.</value>
        public TimeSpan Elapsed { get; private set; }

        /// <summary>
        ///   Gets the amount of game time since the start of the game.
        /// </summary>
        /// <value>The total game time.</value>
        public TimeSpan Total { get; private set; }

        /// <summary>
        ///   Gets the current frame count since the start of the game.
        /// </summary>
        public int FrameCount { get; private set; }

        /// <summary>
        ///   Gets the number of frame per second (FPS) for the current running game.
        /// </summary>
        /// <value>The frame per second.</value>
        public float FramePerSecond { get; private set; }

        /// <summary>
        ///   Gets the time per frame.
        /// </summary>
        /// <value>The time per frame.</value>
        public TimeSpan TimePerFrame { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether the <see cref="FramePerSecond"/> and <see cref="TimePerFrame"/>
        ///   metrics has been updated for this frame.
        /// </summary>
        /// <value>
        ///   <c>true</c> if <see cref="FramePerSecond"/> and <see cref="TimePerFrame"/> has been updated;
        ///   otherwise, <c>false</c>.
        /// </value>
        public bool FramePerSecondUpdated { get; private set; }

        /// <summary>
        ///   Gets the amount of time <see cref="Elapsed"/> since the last update multiplied by the time <see cref="Factor"/>.
        /// </summary>
        /// <value>The warped elapsed time.</value>
        public TimeSpan WarpElapsed { get; private set; }


        /// <summary>
        ///   Gets or sets the time factor.
        /// </summary>
        /// <value>The time multiplier, a value higher than or equal to 0.</value>
        /// <remarks>
        ///   This value controls how much the time flows. This affects physics, animations and particles.
        ///   A value between 0 and 1 will slow time. A value above 1 will make it faster.
        /// </remarks>
        public double Factor
        {
            get => factor;
            set => factor = value > 0 ? value : 0;
        }


        internal void Update(TimeSpan totalGameTime, TimeSpan elapsedGameTime, bool incrementFrameCount)
        {
            Total = totalGameTime;
            Elapsed = elapsedGameTime;
            WarpElapsed = TimeSpan.FromTicks((long)(Elapsed.Ticks * Factor));

            FramePerSecondUpdated = false;

            if (incrementFrameCount)
            {
                accumulatedElapsedTime += elapsedGameTime;
                var accumulatedElapsedGameTimeInSeconds = accumulatedElapsedTime.TotalSeconds;
                if (accumulatedFrameCountPerSecond > 0 && accumulatedElapsedGameTimeInSeconds > 1.0)
                {
                    TimePerFrame = TimeSpan.FromTicks(accumulatedElapsedTime.Ticks / accumulatedFrameCountPerSecond);
                    FramePerSecond = (float)(accumulatedFrameCountPerSecond / accumulatedElapsedGameTimeInSeconds);
                    accumulatedFrameCountPerSecond = 0;
                    accumulatedElapsedTime = TimeSpan.Zero;
                    FramePerSecondUpdated = true;
                }

                accumulatedFrameCountPerSecond++;
                FrameCount++;
            }
        }

        internal void Reset(TimeSpan totalGameTime)
        {
            Update(totalGameTime, elapsedGameTime: TimeSpan.Zero, incrementFrameCount: false);

            accumulatedElapsedTime = TimeSpan.Zero;
            accumulatedFrameCountPerSecond = 0;
            FrameCount = 0;
        }

        public void ResetTimeFactor()
        {
            factor = 1;
        }
    }
}
