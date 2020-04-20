// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

// -----------------------------------------------------------------------------
// The following code is a port of MakeSpriteFont from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System.Drawing;

namespace Stride.Assets.SpriteFont.Compiler
{
    // Crops unused space from around the edge of a glyph bitmap.
    internal static class GlyphCropper
    {
        public static void Crop(Glyph glyph)
        {
            // Crop the top.
            while ((glyph.Subrect.Height > 1) && BitmapUtils.IsAlphaEntirely(0, glyph.Bitmap, new Rectangle(glyph.Subrect.X, glyph.Subrect.Y, glyph.Subrect.Width, 1)))
            {
                glyph.Subrect.Y++;
                glyph.Subrect.Height--;

                glyph.YOffset++;
            }

            // Crop the bottom.
            while ((glyph.Subrect.Height > 1) && BitmapUtils.IsAlphaEntirely(0, glyph.Bitmap, new Rectangle(glyph.Subrect.X, glyph.Subrect.Bottom - 1, glyph.Subrect.Width, 1)))
            {
                glyph.Subrect.Height--;
            }

            // Crop the left.
            while ((glyph.Subrect.Width > 1) && BitmapUtils.IsAlphaEntirely(0, glyph.Bitmap, new Rectangle(glyph.Subrect.X, glyph.Subrect.Y, 1, glyph.Subrect.Height)))
            {
                glyph.Subrect.X++;
                glyph.Subrect.Width--;

                glyph.XOffset++;
            }

            // Crop the right.
            while ((glyph.Subrect.Width > 1) && BitmapUtils.IsAlphaEntirely(0, glyph.Bitmap, new Rectangle(glyph.Subrect.Right - 1, glyph.Subrect.Y, 1, glyph.Subrect.Height)))
            {
                glyph.Subrect.Width--;
            }
        }
    }
}
