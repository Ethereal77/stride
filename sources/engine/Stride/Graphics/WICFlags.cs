// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

// -----------------------------------------------------------------------------
// The following code is a port of DirectXTex http://directxtex.codeplex.com
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System;

namespace Stride.Graphics
{
    [Flags]
    internal enum WICFlags
    {
        None = 0x0,

        /// <summary>
        ///   Loads DXGI 1.1 BGR formats as DXGI_FORMAT_R8G8B8A8_UNORM to avoid use of optional WDDM 1.1 formats.
        /// </summary>
        ForceRgb = 0x1,

        // <summary>
        ///   Loads DXGI 1.1 X2 10:10:10:2 format as DXGI_FORMAT_R10G10B10A2_UNORM.
        /// </summary>
        NoX2Bias = 0x2,

        // <summary>
        ///   Loads 565, 5551, and 4444 formats as 8888 to avoid use of optional WDDM 1.2 formats.
        /// </summary>
        No16Bpp = 0x4,

        // <summary>
        ///   Loads 1-bit monochrome (black & white) as R1_UNORM rather than 8-bit greyscale.
        /// </summary>
        FlagsAllowMono = 0x8,

        // <summary>
        ///   Loads all images in a multi-frame file, converting/resizing to match the first frame as needed, defaults to 0th frame otherwise.
        /// </summary>
        AllFrames = 0x10,

        // <summary>
        ///   Use ordered 4x4 dithering for any required conversions.
        /// </summary>
        Dither = 0x10000,
        // <summary>
        ///   Use error-diffusion dithering for any required conversions.
        /// </summary>
        DitherDiffusion = 0x20000,

        /// <summary>
        ///   Use point (nearest neighbour) filtering mode any required image resizing (only needed when loading arrays of differently sized images).
        /// </summary>
        FilterPoint = 0x100000,
        /// <summary>
        ///   Use linear filtering mode any required image resizing (only needed when loading arrays of differently sized images).
        /// </summary>
        FilterLinear = 0x200000,
        /// <summary>
        ///   Use cubic filtering mode any required image resizing (only needed when loading arrays of differently sized images).
        /// </summary>
        FilterCubic = 0x300000,
        /// <summary>
        ///   Use Fant (combination of Linear and Box filter) filtering mode any required image resizing (only needed when loading arrays of differently sized images). This is the default.
        /// </summary>
        FilterFant = 0x400000,
    }
}
