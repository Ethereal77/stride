// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Audio
{
    /// <summary>
    /// Describe the possible states of the <see cref="AudioEngine"/>.
    /// </summary>
    public enum AudioEngineState
    {
        /// <summary>
        /// The audio engine is currently running.
        /// </summary>
        Running,

        /// <summary>
        /// The audio engine is currently paused. Any calls to play will be dropped.
        /// </summary>
        Paused,

        /// <summary>
        /// The audio engine is not currently usable due to missing audio hardware or unplugged audio output.
        /// </summary>
        Invalidated,

        /// <summary>
        /// The audio engine is disposed. The current instance cannot be used to play or create sounds anymore.
        /// </summary>
        Disposed,
    }
}
