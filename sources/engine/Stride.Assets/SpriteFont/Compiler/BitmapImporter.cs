// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

// -----------------------------------------------------------------------------
// The following code is a port of MakeSpriteFont from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Stride.Assets.SpriteFont.Compiler
{
    // Extracts font glyphs from a specially marked 2D bitmap. Characters should be
    // arranged in a grid ordered from top left to bottom right. Monochrome characters
    // should use white for solid areas and black for transparent areas. To include
    // multicolored characters, add an alpha channel to the bitmap and use that to
    // control which parts of the character are solid. The spaces between characters
    // and around the edges of the grid should be filled with bright pink (red=255,
    // green=0, blue=255). It doesn't matter if your grid includes lots of wasted space,
    // because the converter will rearrange characters, packing as tightly as possible.
    internal class  BitmapImporter : IFontImporter
    {
        // Properties hold the imported font data.
        public IEnumerable<Glyph> Glyphs { get; private set; }

        public float LineSpacing { get; private set; }

        public float BaseLine { get { return 0; } }

        public void Import(SpriteFontAsset options, List<char> characters)
        {
            // Load the source bitmap.
            Bitmap bitmap;

            try
            {
                // TODO Check if source can be used as is from here
                bitmap = new Bitmap(options.FontSource.GetFontPath());
            }
            catch
            {
                throw new FontNotFoundException(options.FontSource.GetFontPath());
            }

            // Convert to our desired pixel format.
            bitmap = BitmapUtils.ChangePixelFormat(bitmap, PixelFormat.Format32bppArgb);

            // What characters are included in this font?
            int characterIndex = 0;
            char currentCharacter = '\0';

            // Split the source image into a list of individual glyphs.
            var glyphList = new List<Glyph>();

            Glyphs = glyphList;
            LineSpacing = 0;

            foreach (Rectangle rectangle in FindGlyphs(bitmap))
            {
                if (characterIndex < characters.Count)
                    currentCharacter = characters[characterIndex++];
                else
                    currentCharacter++;

                glyphList.Add(new Glyph(currentCharacter, bitmap, rectangle) { XAdvance = rectangle.Width });

                LineSpacing = Math.Max(LineSpacing, rectangle.Height);
            }

            // If the bitmap doesn't already have an alpha channel, create one now.
            if (BitmapUtils.IsAlphaEntirely(255, bitmap))
            {
                BitmapUtils.ConvertGreyToAlpha(bitmap, new Rectangle(0,0,bitmap.Width, bitmap.Height));
            }
        }

        // Seems to be the same as this one: http://www.tonicodes.net/blog/creating-custom-fonts-with-outline-for-wp7-and-xna/
        // Searches a 2D bitmap for characters that are surrounded by a marker pink color.
        static IEnumerable<Rectangle> FindGlyphs(Bitmap bitmap)
        {
            using (var bitmapData = new BitmapUtils.PixelAccessor(bitmap, ImageLockMode.ReadOnly))
            {
                for (int y = 1; y < bitmap.Height; y++)
                {
                    for (int x = 1; x < bitmap.Width; x++)
                    {
                        // Look for the top left corner of a character (a pixel that is not pink, but was pink immediately to the left and above it)
                        if (!IsMarkerColor(bitmapData[x, y]) &&
                             IsMarkerColor(bitmapData[x - 1, y]) &&
                             IsMarkerColor(bitmapData[x, y - 1]))
                        {
                            // Measure the size of this character.
                            int w = 1, h = 1;

                            while ((x + w < bitmap.Width) && !IsMarkerColor(bitmapData[x + w, y]))
                            {
                                w++;
                            }

                            while ((y + h < bitmap.Height) && !IsMarkerColor(bitmapData[x, y + h]))
                            {
                                h++;
                            }

                            yield return new Rectangle(x, y, w, h);
                        }
                    }
                }
            }
        }


        // Checks whether a color is the magic magenta marker value.
        static bool IsMarkerColor(Color color)
        {
            return color.ToArgb() == Color.Magenta.ToArgb();
        }
    }
}
