// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Audio
{
    public static class AudioEngineFactory
    {
        /// <summary>
        /// Based on compilation setting, returns the proper instance of sounds.
        /// </summary>
        /// <returns>A platform specific instance of <see cref="AudioEngine"/></returns>
        public static AudioEngine NewAudioEngine(AudioDevice device = null, AudioLayer.DeviceFlags deviceFlags = AudioLayer.DeviceFlags.None)
        {
            var engine = new AudioEngine(device);
            engine.InitializeAudioEngine(deviceFlags);
            return engine;
        }
    }
}
