// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Stride.Graphics
{
    /// <summary>
    ///   Provides extension methods to query information about <see cref="PixelFormat"/>s.
    /// </summary>
    public static class PixelFormatExtensions
    {
        private struct PixelFormatSizeInfo
        {
            public bool IsCompressed;

            public byte BlockWidth;
            public byte BlockHeight;
            public byte BlockSize;
        }

        private static readonly PixelFormatSizeInfo[] sizeInfos = new PixelFormatSizeInfo[256];

        private static readonly bool[] srgbFormats = new bool[256];
        private static readonly bool[] hdrFormats = new bool[256];
        private static readonly bool[] alpha32Formats = new bool[256];
        private static readonly bool[] typelessFormats = new bool[256];

        private static readonly Dictionary<PixelFormat, PixelFormat> sRgbConversion;

        private static int GetIndex(PixelFormat format)
        {
            // DirectX official pixel formats (0..115 use 0..127 in the arrays)
            // Custom pixel formats (1024..1151? use 128..255 in the array)
            return (int) format >= 1024 ?
                (int) format - 1024 + 128 :
                (int) format;
        }

        public static int BlockSize(this PixelFormat format) => sizeInfos[GetIndex(format)].BlockSize;

        public static int BlockWidth(this PixelFormat format) => sizeInfos[GetIndex(format)].BlockWidth;

        public static int BlockHeight(this PixelFormat format) => sizeInfos[GetIndex(format)].BlockHeight;

        /// <summary>
        ///   Calculates the size of a <see cref="PixelFormat"/> in bytes.
        /// </summary>
        /// <param name="format">The pixel format.</param>
        /// <returns>The size in bytes.</returns>
        public static int SizeInBytes(this PixelFormat format)
        {
            var sizeInfo = sizeInfos[GetIndex(format)];
            return sizeInfo.BlockSize / (sizeInfo.BlockWidth * sizeInfo.BlockHeight);
        }

        /// <summary>
        ///   Calculates the size of a <see cref="PixelFormat"/> in bits.
        /// </summary>
        /// <param name="format">The pixel format.</param>
        /// <returns>The size in bits.</returns>
        public static int SizeInBits(this PixelFormat format)
        {
            var sizeInfo = sizeInfos[GetIndex(format)];
            return sizeInfo.BlockSize * 8 / (sizeInfo.BlockWidth * sizeInfo.BlockHeight); ;
        }

        /// <summary>
        ///   Calculates the size of the alpha channel in bits of a <see cref="PixelFormat"/>.
        /// </summary>
        /// <param name="format">The pixel format.</param>
        /// <returns>The size in bits of the alpha channel, or zero if the format has no alpha channel.</returns>
        public static int AlphaSizeInBits(this PixelFormat format) => format switch
        {
            PixelFormat.R32G32B32A32_Typeless or
            PixelFormat.R32G32B32A32_Float or
            PixelFormat.R32G32B32A32_UInt or
            PixelFormat.R32G32B32A32_SInt
                => 32,

            PixelFormat.R16G16B16A16_Typeless or
            PixelFormat.R16G16B16A16_Float or
            PixelFormat.R16G16B16A16_UNorm or
            PixelFormat.R16G16B16A16_UInt or
            PixelFormat.R16G16B16A16_SNorm or
            PixelFormat.R16G16B16A16_SInt
                => 16,

            PixelFormat.R10G10B10A2_Typeless or
            PixelFormat.R10G10B10A2_UNorm or
            PixelFormat.R10G10B10A2_UInt or
            PixelFormat.R10G10B10_Xr_Bias_A2_UNorm
                => 2,

            PixelFormat.R8G8B8A8_Typeless or
            PixelFormat.R8G8B8A8_UNorm or
            PixelFormat.R8G8B8A8_UNorm_SRgb or
            PixelFormat.R8G8B8A8_UInt or
            PixelFormat.R8G8B8A8_SNorm or
            PixelFormat.R8G8B8A8_SInt or
            PixelFormat.B8G8R8A8_UNorm or
            PixelFormat.B8G8R8A8_Typeless or
            PixelFormat.B8G8R8A8_UNorm_SRgb or
            PixelFormat.A8_UNorm
                => 8,

            (PixelFormat) 115 => 4, // DXGI_FORMAT_B4G4R4A4_UNOR

            PixelFormat.B5G5R5A1_UNorm => 1,

            PixelFormat.BC1_Typeless or
            PixelFormat.BC1_UNorm or
            PixelFormat.BC1_UNorm_SRgb
                => 1,

            PixelFormat.BC2_Typeless or
            PixelFormat.BC2_UNorm or
            PixelFormat.BC2_UNorm_SRgb
                => 4,

            PixelFormat.BC3_Typeless or
            PixelFormat.BC3_UNorm or
            PixelFormat.BC3_UNorm_SRgb
                => 8,

            PixelFormat.BC7_Typeless or
            PixelFormat.BC7_UNorm or
            PixelFormat.BC7_UNorm_SRgb
                => 8,

            PixelFormat.ETC2_RGBA or
            PixelFormat.ETC2_RGBA_SRgb
                => 8,

            PixelFormat.ETC2_RGB_A1 => 1,

            _ => 0
        };

        /// <summary>
        ///   Determines if if a <see cref="PixelFormat"/> is valid.
        /// </summary>
        /// <param name="format">The pixel format to validate.</param>
        /// <returns><c>true</c> if the pixel format is valid.</returns>
        public static bool IsValid(this PixelFormat format) =>
            (int) format >= 1 &&
            (int) format <= 115; // DirectX formats

        /// <summary>
        ///   Determines if a <see cref="PixelFormat"/> is a compressed format.
        /// </summary>
        /// <param name="format">The pixel format to check.</param>
        /// <returns><c>true</c> if the pixel format is a compressed format.</returns>
        public static bool IsCompressed(this PixelFormat format) => sizeInfos[GetIndex(format)].IsCompressed;

        /// <summary>
        ///   Determines if a <see cref="PixelFormat"/> is an uncompressed 32-bit color format with an Alpha channel.
        /// </summary>
        /// <param name="format">The pixel format to check.</param>
        /// <returns><c>true</c> if the pixel format is an uncompressed 32-bit color format with an Alpha channel.</returns>
        public static bool HasAlpha32Bits(this PixelFormat format) => alpha32Formats[GetIndex(format)];

        /// <summary>
        ///   Determines if a <see cref="PixelFormat"/> has an Alpha channel.
        /// </summary>
        /// <param name="format">The pixel format to check.</param>
        /// <returns><c>true</c> if the pixel format has an Alpha channel.</returns>
        public static bool HasAlpha(this PixelFormat format) => AlphaSizeInBits(format) != 0;

        /// <summary>
        ///   Determines if the specified <see cref="PixelFormat"/> is a packed format.
        /// </summary>
        /// <param name="format">The pixel format to check.</param>
        /// <returns><c>true</c> if the specified pixel format is packed; otherwise, <c>false</c>.</returns>
        public static bool IsPacked(this PixelFormat format) => format == PixelFormat.R8G8_B8G8_UNorm ||
                                                                format == PixelFormat.G8R8_G8B8_UNorm;

        /// <summary>
        ///   Determines if the specified <see cref="PixelFormat"/> is a video format.
        /// </summary>
        /// <param name="format">The pixel format to check.</param>
        /// <returns><c>true</c> if the specified pixel format is a video format; otherwise, <c>false</c>.</returns>
        public static bool IsVideo(this PixelFormat format)
        {
#if DIRECTX11_1
            switch (format)
            {
                case Format.AYUV:
                case Format.Y410:
                case Format.Y416:
                case Format.NV12:
                case Format.P010:
                case Format.P016:
                case Format.YUY2:
                case Format.Y210:
                case Format.Y216:
                case Format.NV11:
                    // These video formats can be used with the 3D pipeline through special view mappings
                    return true;

                case Format.Opaque420:
                case Format.AI44:
                case Format.IA44:
                case Format.P8:
                case Format.A8P8:
                    // These are limited use video formats not usable in any way by the 3D pipeline
                    return true;

                default:
                    return false;
                }
#else
            // !DXGI_1_2_FORMATS
            return false;
#endif
        }

        /// <summary>
        ///   Determines if the specified <see cref="PixelFormat"/> is a sRGB format.
        /// </summary>
        /// <param name="format">The pixel format to check.</param>
        /// <returns><c>true</c> if the specified pixel format is a sRGB format; otherwise, <c>false</c>.</returns>
        public static bool IsSRgb(this PixelFormat format) => srgbFormats[GetIndex(format)];

        /// <summary>
        ///   Determines if the specified <see cref="PixelFormat"/> is an HDR format (either 16 or 32-bits floating point).
        /// </summary>
        /// <param name="format">The pixel format to check.</param>
        /// <returns><c>true</c> if the specified pixel format is a floating point HDR format; otherwise, <c>false</c>.</returns>
        public static bool IsHDR(this PixelFormat format) => hdrFormats[GetIndex(format)];

        /// <summary>
        ///   Determines if the specified <see cref="PixelFormat"/> is a typeless format.
        /// </summary>
        /// <param name="format">The pixel format to check.</param>
        /// <returns><c>true</c> if the specified pixel format is typeless; otherwise, <c>false</c>.</returns>
        public static bool IsTypeless(this PixelFormat format) => typelessFormats[GetIndex(format)];

        /// <summary>
        ///   Computes the minimum row and slice sizes (pitch) needed for a texture with the specified <see cref="PixelFormat"/>.
        /// </summary>
        /// <param name="format">The pixel format.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        /// <param name="rowPitch">When this method returns, contains the minimum size in bytes for the rows of the texture.</param>
        /// <param name="slicePitch">
        ///   When this method returns, contains the minimum size in bytes for the slices of the texture.
        ///   This is only useful if the texture is a 3D texture.
        /// </param>
        public static void ComputePitch(this PixelFormat format, int width, int height, out int rowPitch, out int slicePitch)
        {
            var sizeInfo = sizeInfos[GetIndex(format)];

            rowPitch = (width + sizeInfo.BlockWidth - 1) / sizeInfo.BlockWidth * sizeInfo.BlockSize;
            slicePitch = rowPitch * ((height + sizeInfo.BlockHeight - 1) / sizeInfo.BlockHeight);
        }

        /// <summary>
        ///   Determines if a <see cref="PixelFormat"/> has an equivalent sRGB format.
        /// </summary>
        /// <param name="format">The non-sRGB pixel format.</param>
        /// <returns><c>true</c> if the pixel format has an sRGB equivalent.</returns>
        public static bool HasSRgbEquivalent(this PixelFormat format)
        {
            if (format.IsSRgb())
                throw new ArgumentException($"The '{format}' format is already an sRGB format.");

            return sRgbConversion.ContainsKey(format);
        }

        /// <summary>
        ///   Determines if a <see cref="PixelFormat"/> has an equivalent non-sRGB format.
        /// </summary>
        /// <param name="format">The sRGB pixel format.</param>
        /// <returns><c>true</c> if the pixel format has an non-sRGB equivalent.</returns>
        public static bool HasNonSRgbEquivalent(this PixelFormat format)
        {
            if (!format.IsSRgb())
                throw new ArgumentException($"The '{format}' format is not a sRGB format.");

            return sRgbConversion.ContainsKey(format);
        }

        /// <summary>
        ///   Gets the equivalent sRGB <see cref="PixelFormat"/> to the provided one.
        /// </summary>
        /// <param name="format">The non-sRGB pixel format.</param>
        /// <returns>
        ///   The equivalent sRGB pixel format if any, or the provided <paramref name="format"/> otherwise.
        /// </returns>
        public static PixelFormat ToSRgb(this PixelFormat format)
        {
            if (format.IsSRgb() || !sRgbConversion.ContainsKey(format))
                return format;

            return sRgbConversion[format];
        }

        /// <summary>
        ///   Gets the equivalent non-sRGB <see cref="PixelFormat"/> to the provided one.
        /// </summary>
        /// <param name="format">The sRGB pixel format.</param>
        /// <returns>
        ///   The equivalent non-sRGB pixel format if any, or the provided <paramref name="format"/> otherwise.
        /// </returns>
        public static PixelFormat ToNonSRgb(this PixelFormat format)
        {
            if (!format.IsSRgb() || !sRgbConversion.ContainsKey(format))
                return format;

            return sRgbConversion[format];
        }

        /// <summary>
        ///   Determines if a <see cref="PixelFormat"/> is in RGBA byte order.
        /// </summary>
        /// <param name="format">The pixel format to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified format is in RGBA order; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRgbaOrder(this PixelFormat format) => format switch
        {
            PixelFormat.R32G32B32A32_Typeless or
            PixelFormat.R32G32B32A32_Float or
            PixelFormat.R32G32B32A32_UInt or
            PixelFormat.R32G32B32A32_SInt or
            PixelFormat.R32G32B32_Typeless or
            PixelFormat.R32G32B32_Float or
            PixelFormat.R32G32B32_UInt or
            PixelFormat.R32G32B32_SInt or
            PixelFormat.R16G16B16A16_Typeless or
            PixelFormat.R16G16B16A16_Float or
            PixelFormat.R16G16B16A16_UNorm or
            PixelFormat.R16G16B16A16_UInt or
            PixelFormat.R16G16B16A16_SNorm or
            PixelFormat.R16G16B16A16_SInt or
            PixelFormat.R32G32_Typeless or
            PixelFormat.R32G32_Float or
            PixelFormat.R32G32_UInt or
            PixelFormat.R32G32_SInt or
            PixelFormat.R32G8X24_Typeless or
            PixelFormat.R10G10B10A2_Typeless or
            PixelFormat.R10G10B10A2_UNorm or
            PixelFormat.R10G10B10A2_UInt or
            PixelFormat.R11G11B10_Float or
            PixelFormat.R8G8B8A8_Typeless or
            PixelFormat.R8G8B8A8_UNorm or
            PixelFormat.R8G8B8A8_UNorm_SRgb or
            PixelFormat.R8G8B8A8_UInt or
            PixelFormat.R8G8B8A8_SNorm or
            PixelFormat.R8G8B8A8_SInt => true,

            _ => false
        };

        /// <summary>
        ///   Determines if a <see cref="PixelFormat"/> is in BGRA byte order.
        /// </summary>
        /// <param name="format">The pixel format to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified format is in BGRA order; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBgraOrder(this PixelFormat format) => format switch
        {
            PixelFormat.B8G8R8A8_UNorm or
            PixelFormat.B8G8R8X8_UNorm or
            PixelFormat.B8G8R8A8_Typeless or
            PixelFormat.B8G8R8A8_UNorm_SRgb or
            PixelFormat.B8G8R8X8_Typeless or
            PixelFormat.B8G8R8X8_UNorm_SRgb => true,

            _ => false
        };

        // ============================================================================================================

        //
        // Static initializer to speed up size calculation (not sure the JIT is enough "smart" for this kind of thing).
        //
        static PixelFormatExtensions()
        {
            InitFormatsSize(new[]
            {
                PixelFormat.A8_UNorm,
                PixelFormat.R8_SInt,
                PixelFormat.R8_SNorm,
                PixelFormat.R8_Typeless,
                PixelFormat.R8_UInt,
                PixelFormat.R8_UNorm
            },
            pixelSize: 1);

            InitFormatsSize(new[]
            {
                PixelFormat.B5G5R5A1_UNorm,
                PixelFormat.B5G6R5_UNorm,
                PixelFormat.D16_UNorm,
                PixelFormat.R16_Float,
                PixelFormat.R16_SInt,
                PixelFormat.R16_SNorm,
                PixelFormat.R16_Typeless,
                PixelFormat.R16_UInt,
                PixelFormat.R16_UNorm,
                PixelFormat.R8G8_SInt,
                PixelFormat.R8G8_SNorm,
                PixelFormat.R8G8_Typeless,
                PixelFormat.R8G8_UInt,
                PixelFormat.R8G8_UNorm,
#if DIRECTX11_1
                PixelFormat.B4G4R4A4_UNorm,
#endif
            },
            pixelSize: 2);

            InitFormatsSize(new[]
            {
                PixelFormat.B8G8R8X8_Typeless,
                PixelFormat.B8G8R8X8_UNorm,
                PixelFormat.B8G8R8X8_UNorm_SRgb,
                PixelFormat.D24_UNorm_S8_UInt,
                PixelFormat.D32_Float,
                PixelFormat.D32_Float_S8X24_UInt,
                PixelFormat.R10G10B10_Xr_Bias_A2_UNorm,
                PixelFormat.R10G10B10A2_Typeless,
                PixelFormat.R10G10B10A2_UInt,
                PixelFormat.R10G10B10A2_UNorm,
                PixelFormat.R11G11B10_Float,
                PixelFormat.R16G16_Float,
                PixelFormat.R16G16_SInt,
                PixelFormat.R16G16_SNorm,
                PixelFormat.R16G16_Typeless,
                PixelFormat.R16G16_UInt,
                PixelFormat.R16G16_UNorm,
                PixelFormat.R24_UNorm_X8_Typeless,
                PixelFormat.R24G8_Typeless,
                PixelFormat.R32_Float,
                PixelFormat.R32_Float_X8X24_Typeless,
                PixelFormat.R32_SInt,
                PixelFormat.R32_Typeless,
                PixelFormat.R32_UInt,
                PixelFormat.R8G8B8A8_SInt,
                PixelFormat.R8G8B8A8_SNorm,
                PixelFormat.R8G8B8A8_Typeless,
                PixelFormat.R8G8B8A8_UInt,
                PixelFormat.R8G8B8A8_UNorm,
                PixelFormat.R8G8B8A8_UNorm_SRgb,
                PixelFormat.B8G8R8A8_Typeless,
                PixelFormat.B8G8R8A8_UNorm,
                PixelFormat.B8G8R8A8_UNorm_SRgb,
                PixelFormat.R9G9B9E5_Sharedexp,
                PixelFormat.X24_Typeless_G8_UInt,
                PixelFormat.X32_Typeless_G8X24_UInt,
            },
            pixelSize: 4);

            InitFormatsSize(new[]
            {
                PixelFormat.R16G16B16A16_Float,
                PixelFormat.R16G16B16A16_SInt,
                PixelFormat.R16G16B16A16_SNorm,
                PixelFormat.R16G16B16A16_Typeless,
                PixelFormat.R16G16B16A16_UInt,
                PixelFormat.R16G16B16A16_UNorm,
                PixelFormat.R32G32_Float,
                PixelFormat.R32G32_SInt,
                PixelFormat.R32G32_Typeless,
                PixelFormat.R32G32_UInt,
                PixelFormat.R32G8X24_Typeless,
            },
            pixelSize: 8);

            InitFormatsSize(new[]
            {
                PixelFormat.R32G32B32_Float,
                PixelFormat.R32G32B32_SInt,
                PixelFormat.R32G32B32_Typeless,
                PixelFormat.R32G32B32_UInt,
            },
            pixelSize: 12);

            InitFormatsSize(new[]
            {
                PixelFormat.R32G32B32A32_Float,
                PixelFormat.R32G32B32A32_SInt,
                PixelFormat.R32G32B32A32_Typeless,
                PixelFormat.R32G32B32A32_UInt,
            },
            pixelSize: 16);

            // Init compressed formats
            InitBlockFormats(new[]
            {
                PixelFormat.BC1_Typeless,
                PixelFormat.BC1_UNorm,
                PixelFormat.BC1_UNorm_SRgb,
                PixelFormat.BC4_SNorm,
                PixelFormat.BC4_Typeless,
                PixelFormat.BC4_UNorm,
                PixelFormat.ETC1,
                PixelFormat.ETC2_RGB,
                PixelFormat.ETC2_RGB_SRgb,
                PixelFormat.ETC2_RGB_A1,
                PixelFormat.EAC_R11_Unsigned,
                PixelFormat.EAC_R11_Signed,
            },
            blockSize: 8,
            blockWidth: 4,
            blockHeight: 4);

            InitBlockFormats(new[]
            {
                PixelFormat.BC2_Typeless,
                PixelFormat.BC2_UNorm,
                PixelFormat.BC2_UNorm_SRgb,
                PixelFormat.BC3_Typeless,
                PixelFormat.BC3_UNorm,
                PixelFormat.BC3_UNorm_SRgb,
                PixelFormat.BC5_SNorm,
                PixelFormat.BC5_Typeless,
                PixelFormat.BC5_UNorm,
                PixelFormat.BC6H_Sf16,
                PixelFormat.BC6H_Typeless,
                PixelFormat.BC6H_Uf16,
                PixelFormat.BC7_Typeless,
                PixelFormat.BC7_UNorm,
                PixelFormat.BC7_UNorm_SRgb,
                PixelFormat.ETC2_RGBA,
                PixelFormat.EAC_RG11_Unsigned,
                PixelFormat.EAC_RG11_Signed,
                PixelFormat.ETC2_RGBA_SRgb,
            },
            blockSize: 16,
            blockWidth: 4,
            blockHeight: 4);

            InitBlockFormats(new[]
            {
                PixelFormat.R8G8_B8G8_UNorm,
                PixelFormat.G8R8_G8B8_UNorm,
            },
            blockSize: 4,
            blockWidth: 2,
            blockHeight: 1);

            InitBlockFormats(new[]
            {
                PixelFormat.R1_UNorm,
            },
            blockSize: 1,
            blockWidth: 8,
            blockHeight: 1);

            // Init sRGB formats
            InitDefaults(new[]
            {
                PixelFormat.R8G8B8A8_UNorm_SRgb,
                PixelFormat.BC1_UNorm_SRgb,
                PixelFormat.BC2_UNorm_SRgb,
                PixelFormat.BC3_UNorm_SRgb,
                PixelFormat.B8G8R8A8_UNorm_SRgb,
                PixelFormat.B8G8R8X8_UNorm_SRgb,
                PixelFormat.BC7_UNorm_SRgb,
                PixelFormat.ETC2_RGBA_SRgb,
                PixelFormat.ETC2_RGB_SRgb,
            },
            srgbFormats);

            // Init 32bpp formats with alpha channel
            InitDefaults(new[]
            {
                PixelFormat.R8G8B8A8_UNorm,
                PixelFormat.R8G8B8A8_UNorm_SRgb,
                PixelFormat.B8G8R8A8_UNorm,
                PixelFormat.B8G8R8A8_UNorm_SRgb,
            },
            alpha32Formats);

            // Init HDR floating-point formats
            InitDefaults(new[]
            {
                PixelFormat.R16G16B16A16_Float,
                PixelFormat.R32G32B32A32_Float,
                PixelFormat.R16G16B16A16_Float,
                PixelFormat.R16G16_Float,
                PixelFormat.R16_Float,
                PixelFormat.BC6H_Sf16,
                PixelFormat.BC6H_Uf16,
            },
            hdrFormats);

            // Init typeless formats
            InitDefaults(new[]
            {
                PixelFormat.R32G32B32A32_Typeless,
                PixelFormat.R32G32B32_Typeless,
                PixelFormat.R16G16B16A16_Typeless,
                PixelFormat.R32G32_Typeless,
                PixelFormat.R32G8X24_Typeless,
                PixelFormat.R32_Float_X8X24_Typeless,
                PixelFormat.X32_Typeless_G8X24_UInt,
                PixelFormat.R10G10B10A2_Typeless,
                PixelFormat.R8G8B8A8_Typeless,
                PixelFormat.R16G16_Typeless,
                PixelFormat.R32_Typeless,
                PixelFormat.R24G8_Typeless,
                PixelFormat.R24_UNorm_X8_Typeless,
                PixelFormat.X24_Typeless_G8_UInt,
                PixelFormat.R8G8_Typeless,
                PixelFormat.R16_Typeless,
                PixelFormat.R8_Typeless,
                PixelFormat.BC1_Typeless,
                PixelFormat.BC2_Typeless,
                PixelFormat.BC3_Typeless,
                PixelFormat.BC4_Typeless,
                PixelFormat.BC5_Typeless,
                PixelFormat.B8G8R8A8_Typeless,
                PixelFormat.B8G8R8X8_Typeless,
                PixelFormat.BC6H_Typeless,
                PixelFormat.BC7_Typeless,
            },
            typelessFormats);

            // Setup sRGB <-> RGB conversions
            sRgbConversion = new Dictionary<PixelFormat, PixelFormat>
            {
                { PixelFormat.R8G8B8A8_UNorm_SRgb, PixelFormat.R8G8B8A8_UNorm },
                { PixelFormat.R8G8B8A8_UNorm,      PixelFormat.R8G8B8A8_UNorm_SRgb },
                { PixelFormat.BC1_UNorm_SRgb,      PixelFormat.BC1_UNorm },
                { PixelFormat.BC1_UNorm,           PixelFormat.BC1_UNorm_SRgb },
                { PixelFormat.BC2_UNorm_SRgb,      PixelFormat.BC2_UNorm },
                { PixelFormat.BC2_UNorm,           PixelFormat.BC2_UNorm_SRgb },
                { PixelFormat.BC3_UNorm_SRgb,      PixelFormat.BC3_UNorm },
                { PixelFormat.BC3_UNorm,           PixelFormat.BC3_UNorm_SRgb },
                { PixelFormat.B8G8R8A8_UNorm_SRgb, PixelFormat.B8G8R8A8_UNorm },
                { PixelFormat.B8G8R8A8_UNorm,      PixelFormat.B8G8R8A8_UNorm_SRgb },
                { PixelFormat.B8G8R8X8_UNorm_SRgb, PixelFormat.B8G8R8X8_UNorm },
                { PixelFormat.B8G8R8X8_UNorm,      PixelFormat.B8G8R8X8_UNorm_SRgb },
                { PixelFormat.BC7_UNorm_SRgb,      PixelFormat.BC7_UNorm },
                { PixelFormat.BC7_UNorm,           PixelFormat.BC7_UNorm_SRgb },
                { PixelFormat.ETC2_RGBA_SRgb,      PixelFormat.ETC2_RGBA },
                { PixelFormat.ETC2_RGBA,           PixelFormat.ETC2_RGBA_SRgb },
                { PixelFormat.ETC2_RGB_SRgb,       PixelFormat.ETC2_RGB },
                { PixelFormat.ETC2_RGB,            PixelFormat.ETC2_RGB_SRgb },
            };
        }

        private static void InitBlockFormats(IEnumerable<PixelFormat> formats, byte blockSize, byte blockWidth, byte blockHeight)
        {
            foreach (var format in formats)
                sizeInfos[GetIndex(format)] = new PixelFormatSizeInfo
                {
                    BlockSize = blockSize,
                    BlockWidth = blockWidth,
                    BlockHeight = blockHeight,
                    IsCompressed = true
                };
        }

        private static void InitFormatsSize(IEnumerable<PixelFormat> formats, byte pixelSize)
        {
            foreach (var format in formats)
                sizeInfos[GetIndex(format)] = new PixelFormatSizeInfo
                {
                    BlockSize = pixelSize,
                    BlockWidth = 1,
                    BlockHeight = 1,
                    IsCompressed = false
                };
        }

        private static void InitDefaults(IEnumerable<PixelFormat> formats, bool[] outputArray)
        {
            foreach (var format in formats)
                outputArray[GetIndex(format)] = true;
        }
    }
}
