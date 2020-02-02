// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if XENKO_GRAPHICS_API_DIRECT3D

using System;

using SharpDX;
using SharpDX.Mathematics.Interop;

using Xenko.Core.Mathematics;

namespace Xenko.Graphics
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

#if XENKO_GRAPHICS_API_DIRECT3D12
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
