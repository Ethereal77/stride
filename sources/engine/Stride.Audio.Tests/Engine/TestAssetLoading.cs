// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Xunit;

using Stride.Engine;

namespace Stride.Audio.Tests.Engine
{
    /// <summary>
    /// Test that <see cref="SoundEffect"/> and <see cref="SoundMusic"/> can be loaded without problem with the asset loader.
    /// </summary>
    public class TestAssetLoading
    {
        /// <summary>
        /// Test loading and playing resulting <see cref="SoundEffect"/> 
        /// </summary>
        [Fact]
        public void TestSoundEffectLoading()
        {
            TestUtilities.ExecuteScriptInUpdateLoop(TestSoundEffectLoadingImpl, TestUtilities.ExitGameAfterSleep(1000));
        }

        private static SoundInstance testInstance;

        private static void TestSoundEffectLoadingImpl(Game game)
        {
            var sound = game.Content.Load<Sound>("EffectBip");
            Assert.NotNull(sound);
            testInstance = sound.CreateInstance(game.Audio.AudioEngine.DefaultListener);
            testInstance.Play();
            // Should hear the sound here.
        }

        /// <summary>
        /// Test loading and playing resulting <see cref="SoundMusic"/>
        /// </summary>
        [Fact]
        public void TestSoundMusicLoading()
        {
            TestUtilities.ExecuteScriptInUpdateLoop(TestSoundMusicLoadingImpl, TestUtilities.ExitGameAfterSleep(2000));
        }

        private static void TestSoundMusicLoadingImpl(Game game)
        {
            var sound = game.Content.Load<Sound>("EffectBip");
            Assert.NotNull(sound);
            testInstance = sound.CreateInstance(game.Audio.AudioEngine.DefaultListener);
            testInstance.Play();
            // Should hear the sound here.
        }
    }
}
