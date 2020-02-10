// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xunit;

namespace Xenko.Particles.Tests
{
    public class VisualTestRibbons : GameTest
    {
        public VisualTestRibbons() : base("VisualTestRibbons")
        {
            IndividualTestVersion = 2;  // Negligible, but consistent differences in the two images (~4-5 pixels total)
            IndividualTestVersion += 4;  //  Changed to avoid collisions with 1.10
        }

        [Fact]
        public void RunVisualTests()
        {
            RunGameTest(new VisualTestRibbons());
        }
    }
}
