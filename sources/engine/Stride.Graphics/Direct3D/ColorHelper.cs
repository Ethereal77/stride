// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_GRAPHICS_API_DIRECT3D

using System;

using SharpDX;
using SharpDX.Mathematics.Interop;

using Stride.Core.Mathematics;

namespace Stride.Graphics
{
    internal class ColorHelper
    {
        public static unsafe RawColor4 Convert(Color4 color)
        {
            return *(RawColor4*)&color;
        }

        public static unsafe Color4 Convert(RawColor4 color)
        {
            return *(Color4*)&color;
        }

        public static unsafe RawVector4 ConvertToVector4(Color4 color)
        {
            return *(RawVector4*)&color;
        }

#if STRIDE_GRAPHICS_API_DIRECT3D12
        public static unsafe SharpDX.Direct3D12.StaticBorderColor ConvertStatic(Color4 color)
        {
            if (color == Color4.Black)
            {
                return SharpDX.Direct3D12.StaticBorderColor.OpaqueBlack;
            }
            else if (color == Color4.White)
            {
                return SharpDX.Direct3D12.StaticBorderColor.OpaqueWhite;
            }
            else if (color == new Color4())
            {
                return SharpDX.Direct3D12.StaticBorderColor.TransparentBlack;
            }

            throw new NotSupportedException("Static sampler can only have opaque black, opaque white or transparent white as border color.");
        }
#endif
    }
}

 
#endif 
