// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Xunit;

namespace Stride.Particles.Tests
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
