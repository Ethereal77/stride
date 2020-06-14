// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Graphics;

using Xunit;

namespace Stride.Particles.Tests
{
    public class VisualTestSoftEdge : GameTest
    {
        public VisualTestSoftEdge() : this(GraphicsProfile.Level_11_0) { }

        private VisualTestSoftEdge(GraphicsProfile profile) : base("VisualTestSoftEdge", profile) { }

        [Fact]
        public void RunVisualTests11()
        {
            RunGameTest(new VisualTestSoftEdge(GraphicsProfile.Level_11_0));
        }
    }
}
