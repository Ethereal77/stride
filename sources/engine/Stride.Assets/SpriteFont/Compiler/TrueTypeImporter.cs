// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

using Stride.Graphics.Font;

namespace Stride.Assets.SpriteFont.Compiler
{
    using System.Drawing;
    using System.Drawing.Imaging;

    using SharpDX.DirectWrite;

    using Factory = SharpDX.DirectWrite.Factory;

    // This code was originally taken from DirectXTk but rewritten with DirectWrite
    // for more accuracy in font rendering
    internal class TrueTypeImporter : IFontImporter
    {
        // Properties hold the imported font data.
        public IEnumerable<Glyph> Glyphs { get; private set; }

        public float LineSpacing { get; private set; }

        public float BaseLine { get; private set; }

        public void Import(SpriteFontAsset options, List<char> characters)
        {
            var factory = new Factory();

            var fontFace = options.FontSource.GetFontFace();

            var fontMetrics = fontFace.Metrics;

            // Create a bunch of GDI+ objects.
            var fontSize = options.FontType.Size;

            var glyphList = new List<Glyph>();

            // Remap the LineMap coming from the font with a user defined remapping
            // Note:
            // We are remapping the lineMap to allow to shrink the LineGap and to reposition it at the top and/or bottom of the
            // font instead of using only the top
            // According to http://stackoverflow.com/questions/13939264/how-to-determine-baseline-position-using-directwrite#comment27947684_14061348
            // (The response is from a MSFT employee), the BaseLine should be = LineGap + Ascent but this is not what
            // we are experiencing when comparing with MSWord (LineGap + Ascent seems to offset too much.)
            //
            // So we are first applying a factor to the line gap:
            //     NewLineGap = LineGap * LineGapFactor
            var lineGap = fontMetrics.LineGap * options.LineGapFactor;

            // Store the font height.
            LineSpacing = (float)(lineGap + fontMetrics.Ascent + fontMetrics.Descent) / fontMetrics.DesignUnitsPerEm * fontSize;

            // And then the baseline is also changed in order to allow the linegap to be distributed between the top and the
            // bottom of the font:
            //     BaseLine = NewLineGap * LineGapBaseLineFactor
            BaseLine = (float)(lineGap * options.LineGapBaseLineFactor + fontMetrics.Ascent) / fontMetrics.DesignUnitsPerEm * fontSize;

            // Rasterize each character in turn.
            foreach (var character in characters)
                glyphList.Add(ImportGlyph(factory, fontFace, character, fontMetrics, fontSize, options.FontType.AntiAlias));

            Glyphs = glyphList;

            factory.Dispose();
        }

        private Glyph ImportGlyph(Factory factory, FontFace fontFace, char character, FontMetrics fontMetrics, float fontSize, FontAntiAliasMode antiAliasMode)
        {
            var indices = fontFace.GetGlyphIndices(new int[] { character });

            var metrics = fontFace.GetDesignGlyphMetrics(indices, false);
            var metric = metrics[0];

            var width = (float)(metric.AdvanceWidth - metric.LeftSideBearing - metric.RightSideBearing) / fontMetrics.DesignUnitsPerEm * fontSize;
            var height = (float)(metric.AdvanceHeight - metric.TopSideBearing - metric.BottomSideBearing) / fontMetrics.DesignUnitsPerEm * fontSize;

            var xOffset = (float)metric.LeftSideBearing / fontMetrics.DesignUnitsPerEm * fontSize;
            var yOffset = (float)(metric.TopSideBearing - metric.VerticalOriginY) / fontMetrics.DesignUnitsPerEm * fontSize;

            var advanceWidth = (float)metric.AdvanceWidth / fontMetrics.DesignUnitsPerEm * fontSize;
            //var advanceHeight = (float)metric.AdvanceHeight / fontMetrics.DesignUnitsPerEm * fontSize;

            var pixelWidth = (int)Math.Ceiling(width + 4);
            var pixelHeight = (int)Math.Ceiling(height + 4);

            var matrix = new RawMatrix3x2
            {
                M11 = 1,
                M22 = 1,
                M31 = -(float)Math.Floor(xOffset) + 1,
                M32 = -(float)Math.Floor(yOffset) + 1
            };

            Bitmap bitmap;
            if (char.IsWhiteSpace(character))
            {
                bitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            }
            else
            {
                var glyphRun = new GlyphRun
                {
                    FontFace = fontFace,
                    Advances = new[] { (float)Math.Ceiling(advanceWidth) },
                    FontSize = fontSize,
                    BidiLevel = 0,
                    Indices = indices,
                    IsSideways = false,
                    Offsets = new[] { new GlyphOffset() }
                };


                RenderingMode renderingMode;
                if (antiAliasMode != FontAntiAliasMode.Aliased)
                {
                    var rtParams = new RenderingParams(factory);
                    renderingMode = fontFace.GetRecommendedRenderingMode(fontSize, 1.0f, MeasuringMode.Natural, rtParams);
                    rtParams.Dispose();
                }
                else
                {
                    renderingMode = RenderingMode.Aliased;
                }

                using (var runAnalysis = new GlyphRunAnalysis(factory,
                    glyphRun,
                    1.0f,
                    matrix,
                    renderingMode,
                    MeasuringMode.Natural,
                    0.0f,
                    0.0f))
                {

                    var bounds = new RawRectangle(0, 0, pixelWidth, pixelHeight);
                    bitmap = new Bitmap(pixelWidth, pixelHeight, PixelFormat.Format32bppArgb);

                    if (renderingMode == RenderingMode.Aliased)
                    {

                        var texture = new byte[pixelWidth * pixelHeight];
                        runAnalysis.CreateAlphaTexture(TextureType.Aliased1x1, bounds, texture, texture.Length);
                        for (int y = 0; y < pixelHeight; y++)
                        {
                            for (int x = 0; x < pixelWidth; x++)
                            {
                                int pixelX = y * pixelWidth + x;
                                var grey = texture[pixelX];
                                var color = Color.FromArgb(grey, grey, grey);

                                bitmap.SetPixel(x, y, color);
                            }
                        }
                    }
                    else
                    {
                        var texture = new byte[pixelWidth * pixelHeight * 3];
                        runAnalysis.CreateAlphaTexture(TextureType.Cleartype3x1, bounds, texture, texture.Length);
                        for (int y = 0; y < pixelHeight; y++)
                        {
                            for (int x = 0; x < pixelWidth; x++)
                            {
                                int pixelX = (y * pixelWidth + x) * 3;
                                var red = LinearToGamma(texture[pixelX]);
                                var green = LinearToGamma(texture[pixelX + 1]);
                                var blue = LinearToGamma(texture[pixelX + 2]);
                                var color = Color.FromArgb(red, green, blue);

                                bitmap.SetPixel(x, y, color);
                            }
                        }
                    }
                }
            }

            var glyph = new Glyph(character, bitmap)
            {
                XOffset = -matrix.M31,
                XAdvance = advanceWidth,
                YOffset = -matrix.M32,
            };
            return glyph;
        }

        private static byte LinearToGamma(byte color)
        {
            return (byte)(Math.Pow(color / 255.0f, 1 / 2.2f) * 255.0f);
        }
    }
}
