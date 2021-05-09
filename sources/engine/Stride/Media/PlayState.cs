// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Media
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
