// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Audio
{
    /// <summary>
    /// Reprensent an Audio Hardware Device.
    /// Can be used when creating an <see cref="AudioEngine"/> to specify the device on which to play the sound.
    /// </summary>
    public class AudioDevice
    {
        /// <summary>
        /// Returns the name of the current device.
        /// </summary>
        public string Name { get; set; }

        public AudioDevice()
        {
            Name = "default";
        }
    }
}
