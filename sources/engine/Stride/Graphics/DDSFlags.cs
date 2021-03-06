// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

// -----------------------------------------------------------------------------
// The following code is a port of DirectXTex http://directxtex.codeplex.com
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System;

namespace Stride.Graphics
{
    /// <summary>
    /// Flags used by <see cref="DDSHelper.LoadFromDDSMemory"/>.
    /// </summary>
    [Flags]
    internal enum DDSFlags
    {
        None = 0x0,
        
        /// <summary>
        ///   Assume pitch is DWORD aligned instead of BYTE aligned (used by some legacy DDS files).
        /// </summary>
        LegacyDword = 0x1,
        
        /// <summary>
        ///   Do not implicitly convert legacy formats that result in larger pixel sizes (24 bpp, 3:3:2, A8L8, A4L4, P8, A8P8).
        /// </summary>
        NoLegacyExpansion = 0x2,
        
        /// <summary>
        ///   Do not use work-around for long-standing D3DX DDS file format issue which reversed the 10:10:10:2 color order masks.
        /// </summary>
        NoR10B10G10A2Fixup = 0x4,
        
        /// <summary>
        ///   Convert DXGI 1.1 BGR formats to Format.R8G8B8A8_UNorm to avoid use of optional WDDM 1.1 formats.
        /// </summary>
        ForceRgb = 0x8,
        
        /// <summary>
        ///   Conversions avoid use of 565, 5551, and 4444 formats and instead expand to 8888 to avoid use of optional WDDM 1.2 formats.
        /// </summary>
        No16Bpp = 0x10,
        
        /// <summary>
        ///   The content of the memory passed to the DDS Loader is copied to another internal buffer.
        /// </summary>
        CopyMemory = 0x20,
        
        /// <summary>
        ///   Always use the 'DX10' header extension for DDS writer (i.e. don't try to write DX9 compatible DDS files).
        /// </summary>
        ForceDX10Ext = 0x10000,
    }
}
