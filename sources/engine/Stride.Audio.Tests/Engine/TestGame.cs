// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Xunit;

using Stride.Engine;

namespace Stride.Audio.Tests.Engine
{
    /// <summary>
    /// Test the class <see cref="Game"/> augmented with the audio system.
    /// </summary>
    public class TestGame
    {
        /// <summary>
        /// Check that there is not problems during creation and destruction of the Game class.
        /// </summary>
        [Fact]
        public void TestCreationDestructionOfTheGame()
        {
            // Make sure this doesn't throw
            var game = new AudioTestGame();
            game.Dispose();
        }

        /// <summary>
        /// Check that we can access to the audio class and that it is not invalid.
        /// </summary>
        [Fact]
        public void TestAccessToAudio()
        {
            using (var game = new Game())
            {
                AudioSystem audioInterface = game.Audio;
                Assert.NotNull(audioInterface);
            }
        }
    }
}
