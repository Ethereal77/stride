// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

// -----------------------------------------------------------------------------
// The following code is a port of MakeSpriteFont from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using Xenko.Core.Mathematics;
using Xenko.Graphics.Font;

namespace Xenko.Assets.SpriteFont.Compiler
{
    // Writes the output sprite font binary file.
    internal static class SignedDistanceFieldFontWriter
    {
        public static Graphics.SpriteFont CreateSpriteFontData(IFontFactory fontFactory, SpriteFontAsset options, Glyph[] glyphs, float lineSpacing, float baseLine, Bitmap bitmap)
        {
            var fontGlyphs = ConvertGlyphs(glyphs);
            var images = new[] { GetImage(options, bitmap) };
            var sizeInPixels = options.FontType.Size;

            return fontFactory.NewScalable(sizeInPixels, fontGlyphs, images, baseLine, lineSpacing, null, options.Spacing, options.LineSpacing, options.DefaultCharacter);
        }

        static Graphics.Font.Glyph[] ConvertGlyphs(Glyph[] glyphs)
        {
            var fontGlyphs = new Graphics.Font.Glyph[glyphs.Length];

            for (var i = 0; i < glyphs.Length; ++i)
            {
                var glyph = glyphs[i];
                fontGlyphs[i] = new Graphics.Font.Glyph
                {
                    Character = glyph.Character,
                    Subrect = new Core.Mathematics.Rectangle(glyph.Subrect.X, glyph.Subrect.Y, glyph.Subrect.Width, glyph.Subrect.Height),
                    Offset = new Vector2(glyph.XOffset, glyph.YOffset),
                    XAdvance = glyph.XAdvance,
                };
            }

            return fontGlyphs;
        }

        static Graphics.Image GetImage(SpriteFontAsset options, Bitmap bitmap)
        {
            // TODO Currently we only support Rgba32 as an option. Grayscale might be added later
            return GetImageRgba32(bitmap);
        }

        // Writes an uncompressed 32 bit font texture.
        // We have distance values encoded so we want uncompressed texture
        static Graphics.Image GetImageRgba32(Bitmap bitmap)
        {
            // TODO Maybe switch to 3-channel texture
            var image = Graphics.Image.New2D(bitmap.Width, bitmap.Height, 1, Graphics.PixelFormat.R8G8B8A8_UNorm);
            var pixelBuffer = image.PixelBuffer[0];
            using (var bitmapData = new BitmapUtils.PixelAccessor(bitmap, ImageLockMode.ReadOnly))
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        var color = bitmapData[x, y];
                        pixelBuffer.SetPixel(x, y, new Core.Mathematics.Color(color.R, color.G, color.B, color.A));
                    }
                }
            }
            return image;
        }
   
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct BC2Pixel
        {
            public long AlphaBits;
            public uint EndPoint;
            public int RgbBits;
        }        
    }
}
