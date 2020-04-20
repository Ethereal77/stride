// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Assets.Textures.Packing
{
    /// <summary>
    /// The Heuristic methods used to place sprites in atlas.
    /// </summary>
    public enum TexturePackingMethod
    {
        Best,
        BestShortSideFit,
        BestLongSideFit,
        BestAreaFit,
        BottomLeftRule,
        ContactPointRule
    }
}
