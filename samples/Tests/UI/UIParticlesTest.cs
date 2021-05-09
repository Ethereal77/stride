// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Games.Testing;

using Xunit;

namespace Stride.Samples.Tests
{
    public class UIParticlesTest : IClassFixture<UIParticlesTest.Fixture>
    {
        private const string Path = "..\\..\\..\\..\\..\\samplesGenerated\\UIParticles";

        public class Fixture : SampleTestFixture
        {
            public Fixture() : base(Path, new Guid("DA4B1982-2A93-48FB-8EDA-7B13AD79E6A2"))
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
                game.Wait(TimeSpan.FromMilliseconds(2000));

                game.TakeScreenshot();

		// TODO Simulate taps

                game.Tap(new Vector2(179f / 600f, 235f / 600f), TimeSpan.FromMilliseconds(150));
                game.Wait(TimeSpan.FromMilliseconds(250));
                game.TakeScreenshot();

                game.Tap(new Vector2(360f / 600f, 328f / 600f), TimeSpan.FromMilliseconds(150));
                game.Wait(TimeSpan.FromMilliseconds(1250));
                game.TakeScreenshot();

                game.Tap(new Vector2(179f / 600f, 235f / 600f), TimeSpan.FromMilliseconds(150));
                game.Wait(TimeSpan.FromMilliseconds(250));
                game.TakeScreenshot();

                game.Wait(TimeSpan.FromMilliseconds(2000));
            }
        }
    }
}
