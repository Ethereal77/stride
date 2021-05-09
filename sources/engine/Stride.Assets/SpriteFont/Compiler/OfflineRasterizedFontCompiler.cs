// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

// -----------------------------------------------------------------------------
// The following code is a port of MakeSpriteFont from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using Stride.Graphics;
using Stride.Graphics.Font;

namespace Stride.Assets.SpriteFont.Compiler
{
    /// <summary>
    /// Main class used to compile a Font file XML file.
    /// </summary>
    public class OfflineRasterizedFontCompiler
    {
        /// <summary>
        /// Compiles the specified font description into a <see cref="OfflineRasterizedSpriteFont" /> object.
        /// </summary>
        /// <param name="fontFactory">The font factory used to create the fonts</param>
        /// <param name="fontAsset">The font description.</param>
        /// <param name="srgb"></param>
        /// <returns>A SpriteFontData object.</returns>
        public static Graphics.SpriteFont Compile(IFontFactory fontFactory, SpriteFontAsset fontAsset, bool srgb)
        {
            var fontTypeStatic = fontAsset.FontType as OfflineRasterizedSpriteFontType;
            if (fontTypeStatic == null)
                throw new ArgumentException("Tried to compile a dynamic sprite font with compiler for static fonts");

            float lineSpacing;
            float baseLine;

            var glyphs = ImportFont(fontAsset, out lineSpacing, out baseLine);

            // Optimize.
            foreach (Glyph glyph in glyphs)
                GlyphCropper.Crop(glyph);

            Bitmap bitmap = GlyphPacker.ArrangeGlyphs(glyphs);

            // Automatically detect whether this is a monochromatic or color font?
            //if (fontAsset.Format == FontTextureFormat.Auto)
            //{
            //    bool isMono = BitmapUtils.IsRgbEntirely(Color.White, bitmap);
            //
            //    fontAsset.Format = isMono ? FontTextureFormat.CompressedMono :
            //                                     FontTextureFormat.Rgba32;
            //}

            // Convert to pre-multiplied alpha format.
            if (fontAsset.FontType.IsPremultiplied)
            {
                if (fontAsset.FontType.AntiAlias == FontAntiAliasMode.ClearType)
                {
                    BitmapUtils.PremultiplyAlphaClearType(bitmap, srgb);
                }
                else
                {
                    BitmapUtils.PremultiplyAlpha(bitmap, srgb);
                }
            }

            return OfflineRasterizedSpriteFontWriter.CreateSpriteFontData(fontFactory, fontAsset, glyphs, lineSpacing, baseLine, bitmap, srgb);
        }

        static Glyph[] ImportFont(SpriteFontAsset options, out float lineSpacing, out float baseLine)
        {
            // Which importer knows how to read this source font?
            IFontImporter importer;

            var sourceExtension = (Path.GetExtension(options.FontSource.GetFontPath()) ?? "").ToLowerInvariant();
            var bitmapFileExtensions = new List<string> { ".bmp", ".png", ".gif" };
            var importFromBitmap = bitmapFileExtensions.Contains(sourceExtension);

            importer = importFromBitmap ? (IFontImporter) new BitmapImporter() : new TrueTypeImporter();

            // create the list of character to import
            var characters = GetCharactersToImport(options); 

            // Import the source font data.
            importer.Import(options, characters);

            lineSpacing = importer.LineSpacing;
            baseLine = importer.BaseLine;

            // Get all glyphs
            var glyphs = new List<Glyph>(importer.Glyphs);

            // Validate.
            if (glyphs.Count == 0)
            {
                throw new Exception("Font does not contain any glyphs.");
            }
            if (!importFromBitmap && options.FontType.AntiAlias != FontAntiAliasMode.ClearType)
            {
                foreach (var glyph in importer.Glyphs)
                    BitmapUtils.ConvertGreyToAlpha(glyph.Bitmap, glyph.Subrect);
            }

            // Sort the glyphs
            glyphs.Sort((left, right) => left.Character.CompareTo(right.Character));


            // Check that the default character is part of the glyphs
            if (options.DefaultCharacter != 0)
            {
                bool defaultCharacterFound = false;
                foreach (var glyph in glyphs)
                {
                    if (glyph.Character == options.DefaultCharacter)
                    {
                        defaultCharacterFound = true;
                        break;
                    }
                }
                if (!defaultCharacterFound)
                {
                    throw new InvalidOperationException("The specified DefaultCharacter is not part of this font.");
                }
            }

            return glyphs.ToArray();
        }

        public static List<char> GetCharactersToImport(SpriteFontAsset asset)
        {
            var characters = new List<char>();

            var fontTypeStatic = asset.FontType as OfflineRasterizedSpriteFontType;
            if (fontTypeStatic == null)
                throw new ArgumentException("Tried to compile a dynamic sprite font with compiler for signed distance field fonts");

            // extract the list from the provided file if it exits
            if (File.Exists(fontTypeStatic.CharacterSet))
            {
                string text;
                using (var streamReader = new StreamReader(fontTypeStatic.CharacterSet, Encoding.UTF8))
                    text = streamReader.ReadToEnd();
                characters.AddRange(text);
            }

            // add character coming from character ranges
            characters.AddRange(CharacterRegion.Flatten(fontTypeStatic.CharacterRegions));

            // remove duplicated characters
            characters = characters.Distinct().ToList();

            return characters;
        }
    }
}
