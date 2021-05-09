// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Graphics
{
    public static class PrimitiveTypeExtensions
    {
        /// <summary>
        /// Interpret the vertex data as a patch list.
        /// </summary>
        /// <param name="controlPoints">Number of control points. Value must be in the range 1 to 32.</param>
        public static PrimitiveType ControlPointCount(this PrimitiveType primitiveType, int controlPoints)
        {
            if (primitiveType != PrimitiveType.PatchList)
                throw new ArgumentException("Control points apply only to PrimitiveType.PatchList", "primitiveType");

            if (controlPoints < 1 || controlPoints > 32)
                throw new ArgumentException("Value must be in between 1 and 32", "controlPoints");

            return PrimitiveType.PatchList + controlPoints - 1;
        }
    }
}
