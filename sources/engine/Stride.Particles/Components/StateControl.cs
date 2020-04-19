// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Particles.Components
{
    /// <summary>
    /// State control for the particle system
    /// </summary>
    [DataContract]
    public enum StateControl
    {
        /// <summary>
        /// The state is active and currently playing
        /// </summary>
        Play,

        /// <summary>
        /// The state is active, but currently not playing (paused)
        /// </summary>
        Pause,

        /// <summary>
        /// The state is inactive
        /// </summary>
        Stop,
    }

}
