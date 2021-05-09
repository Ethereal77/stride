// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

using Xunit;

using Stride.TextureConverter;

namespace Stride.TextureConverter.Tests
{
    public class TexImageTest : IDisposable
    {
        private readonly TexImage image = new TexImage(Marshal.AllocHGlobal(699104), 699104, 512, 512, 1, Stride.Graphics.PixelFormat.BC3_UNorm, 10, 2, TexImage.TextureDimension.Texture2D);

        public void Dispose()
        {
            Marshal.FreeHGlobal(image.Data);
        }

        [Fact(Skip = "Need check")]
        public void TestEquals()
        {
            TexImage image2 = new TexImage(new IntPtr(), 699104, 512, 512, 1, Stride.Graphics.PixelFormat.BC3_UNorm, 10, 2, TexImage.TextureDimension.Texture2D);
            Assert.True(image.Equals(image2));

            image2 = new TexImage(new IntPtr(), 699104, 512, 256, 1, Stride.Graphics.PixelFormat.BC3_UNorm, 10, 2, TexImage.TextureDimension.Texture2D);
            Assert.False(image.Equals(image2));
        }

        [Fact(Skip = "Need check")]
        public void TestClone()
        {
            TexImage clone = (TexImage)image.Clone();

            Assert.True(image.Equals(clone));

            clone.Dispose();
        }
    }
}
