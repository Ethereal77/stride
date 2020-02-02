// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xunit;

namespace Xenko.Particles.Tests
{
    public class VisualTestChildren : GameTest
    {
        public VisualTestChildren() : base("VisualTestChildren")
        {
            IndividualTestVersion = 1;  //  Changes in particle spawning
            IndividualTestVersion += 4;  //  Changed to avoid collisions with 1.10
        }

        [Fact]
        public void RunVisualTests()
        {
            RunGameTest(new VisualTestChildren());
        }
    }
}
