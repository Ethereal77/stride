// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xunit;

namespace Stride.Particles.Tests
{
    public class VisualTestCurves : GameTest
    {
        public VisualTestCurves() : base("VisualTestCurves")
        {
            IndividualTestVersion = 1;  //  Changed the default rotation for paritcles using curve rotation
        }

        [Fact]
        public void RunVisualTests()
        {
            RunGameTest(new VisualTestCurves());
        }
    }
}
