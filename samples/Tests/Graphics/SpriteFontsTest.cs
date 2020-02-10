// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Input;
using Xenko.Games.Testing;

using Xunit;

namespace Xenko.Samples.Tests
{
    public class SpriteFontsTest : IClassFixture<SpriteFontsTest.Fixture>
    {
        private const string Path = "..\\..\\..\\..\\..\\samplesGenerated\\SpriteFonts";

        public class Fixture : SampleTestFixture
        {
            public Fixture() : base(Path, new Guid("1EEB50EC-1AA7-4D1F-9DDD-E5E12404B001"))
            {
            }
        }

        [Fact]
        public void TestLaunch()
        {
            using (var game = new GameTestingClient(Path, SampleTestsData.TestPlatform))
            {
                game.Wait(TimeSpan.FromMilliseconds(2000));
            }
        }

        [Fact]
        public void TestInputs()
        {
            using (var game = new GameTestingClient(Path, SampleTestsData.TestPlatform))
            {
                game.Wait(TimeSpan.FromMilliseconds(1000));
                game.TakeScreenshot();
                game.Wait(TimeSpan.FromMilliseconds(4000));
                game.TakeScreenshot();
                game.Wait(TimeSpan.FromMilliseconds(4000));
                game.TakeScreenshot();
                game.Wait(TimeSpan.FromMilliseconds(5000));
                game.TakeScreenshot();
                game.Wait(TimeSpan.FromMilliseconds(4000));
                game.TakeScreenshot();
                game.Wait(TimeSpan.FromMilliseconds(4000));
                game.TakeScreenshot();
            }
        }
    }
}
