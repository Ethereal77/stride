// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Audio
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
