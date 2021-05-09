// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Xunit;

using Stride.Graphics;
using Stride.Rendering.Sprites;
using Stride.UI.Controls;

namespace Stride.UI.Tests.Layering
{
    /// <summary>
    /// A class that contains test functions for layering of the <see cref="Button"/> class.
    /// </summary>
    [System.ComponentModel.Description("Tests for Button layering")]
    public class ButtonTests : Button
    {
        [Fact]
        public void TestProperties()
        {
            var control = new Button();

            // test properties default values
            Assert.Equal(new Thickness(10, 5, 10, 7), control.Padding);
        }

        /// <summary>
        /// Test the invalidations generated object property changes.
        /// </summary>
        [Fact]
        public void TestBasicInvalidations()
        {
            SizeToContent = true;
            // - test the properties that are not supposed to invalidate the object layout state
            UIElementLayeringTests.TestNoInvalidation(this, () => PressedImage = (SpriteFromTexture)new Sprite());
            UIElementLayeringTests.TestNoInvalidation(this, () => NotPressedImage = (SpriteFromTexture)new Sprite());

            SizeToContent = false;
            // - test the properties that are supposed to invalidate the object layout state
            UIElementLayeringTests.TestMeasureInvalidation(this, () => PressedImage = (SpriteFromTexture)new Sprite());
            UIElementLayeringTests.TestMeasureInvalidation(this, () => NotPressedImage = (SpriteFromTexture)new Sprite());
        }
    }
}
