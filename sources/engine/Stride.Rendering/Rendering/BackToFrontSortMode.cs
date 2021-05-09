// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Rendering
{
    /// <summary>
    /// Sort elements according to the pattern: [RenderFeature Sort Key 8 bits] [Distance back to front 32 bits] [RenderObject states 24 bits]
    /// </summary>
    [DataContract("BackToFrontSortMode")]
    public class BackToFrontSortMode : SortModeDistance
    {
        public BackToFrontSortMode() : base(true)
        {
            distancePrecision = 32;
            distancePosition = 24;

            statePrecision = 24;
        }
    }
}
