// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xunit;

namespace Xenko.Particles.Tests
{
    public class VisualTestGeneral : GameTest
    {
        public VisualTestGeneral() : base("VisualTestGeneral")
        {
            IndividualTestVersion = 1;  //  Changes in particle spawning
        }

        [Fact]
        public void RunVisualTests()
        {
            RunGameTest(new VisualTestGeneral());
        }
    }
}
