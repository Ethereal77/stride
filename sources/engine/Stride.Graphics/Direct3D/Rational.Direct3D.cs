// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_GRAPHICS_API_DIRECT3D 

using System;

namespace Stride.Graphics
{
    public partial struct Rational
    {
        /// <summary>
        /// Converts from SharpDX representation.
        /// </summary>
        /// <param name="rational">The rational.</param>
        /// <returns>Rational.</returns>
        internal static Rational FromSharpDX(SharpDX.DXGI.Rational rational)
        {
            return new Rational(rational.Numerator, rational.Denominator);
        }

        /// <summary>
        /// Converts to SharpDX representation.
        /// </summary>
        /// <returns>SharpDX.DXGI.Rational.</returns>
        internal SharpDX.DXGI.Rational ToSharpDX()
        {
            return new SharpDX.DXGI.Rational(Numerator, Denominator);
        }
    }
}
#endif
