// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Xunit;

namespace Stride.Graphics.Tests
{
    public class TestDynamicSpriteFont : TestSpriteFont
    {
        public TestDynamicSpriteFont()
            : base("DynamicFonts/", "dyn")
        { }

        /// <summary>
        /// Run the test
        /// </summary>
        [Fact]
        public void RunTestDynamicSpriteFont()
        {
            RunGameTest(new TestDynamicSpriteFont());
        }
    }
}
