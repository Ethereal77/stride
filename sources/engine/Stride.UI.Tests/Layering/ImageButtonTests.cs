// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Xunit;

using Stride.UI.Controls;

namespace Stride.UI.Tests.Layering
{
    /// <summary>
    /// A class that contains test functions for layering of the <see cref="ImageButton"/> class.
    /// </summary>
    [System.ComponentModel.Description("Tests for ImageButton layering")]
    public class ImageButtonTests
    {
        [Fact(Skip = "ImageButton is deprecated")]
        public void TestProperties()
        {
            var control = new ImageButton();

            // test properties default values
            Assert.Equal(new Thickness(0, 0, 0, 0), control.Padding);
        }
    }
}
