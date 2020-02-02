// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Graphics;

namespace Xenko.Rendering
{
    public static class XenkoTessellationMethodExtensions
    {
        public static bool PerformsAdjacentEdgeAverage(this XenkoTessellationMethod method)
        {
            return (method & XenkoTessellationMethod.AdjacentEdgeAverage) != 0;
        }

        public static PrimitiveType GetPrimitiveType(this XenkoTessellationMethod method)
        {
            if ((method & XenkoTessellationMethod.PointNormal) == 0)
                return PrimitiveType.TriangleList;

            var controlsCount = method.PerformsAdjacentEdgeAverage() ? 12 : 3;
            return PrimitiveType.PatchList.ControlPointCount(controlsCount);
        }
    }
}
