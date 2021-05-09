// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Graphics;

namespace Stride.Rendering
{
    public static class StrideTessellationMethodExtensions
    {
        public static bool PerformsAdjacentEdgeAverage(this StrideTessellationMethod method)
        {
            return (method & StrideTessellationMethod.AdjacentEdgeAverage) != 0;
        }

        public static PrimitiveType GetPrimitiveType(this StrideTessellationMethod method)
        {
            if ((method & StrideTessellationMethod.PointNormal) == 0)
                return PrimitiveType.TriangleList;

            var controlsCount = method.PerformsAdjacentEdgeAverage() ? 12 : 3;
            return PrimitiveType.PatchList.ControlPointCount(controlsCount);
        }
    }
}
