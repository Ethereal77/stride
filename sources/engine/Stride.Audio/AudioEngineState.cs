// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Audio
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
