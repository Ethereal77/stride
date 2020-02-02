// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if XENKO_GRAPHICS_API_DIRECT3D

using SharpDX.DXGI;

namespace Xenko.Graphics
{
    public partial class DisplayMode
    {
        internal ModeDescription ToDescription()
        {
            return new ModeDescription(Width, Height, RefreshRate.ToSharpDX(), format: (SharpDX.DXGI.Format)Format);
        }

        internal static DisplayMode FromDescription(ModeDescription description)
        {
            return new DisplayMode((PixelFormat)description.Format, description.Width, description.Height, new Rational(description.RefreshRate.Numerator, description.RefreshRate.Denominator));
        }
    }
}
#endif
