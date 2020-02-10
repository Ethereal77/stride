// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Media
{
    /// <summary>
    /// Current state (playing, paused, or stopped) of a media.
    /// </summary>
    public enum PlayState
    {
        /// <summary>
        /// The media is currently being played.
        /// </summary>
        Playing,

        /// <summary>
        /// The media is currently paused.
        /// </summary>
        Paused,

        /// <summary>
        /// The media is currently stopped.
        /// </summary>
        Stopped,
    }
}
