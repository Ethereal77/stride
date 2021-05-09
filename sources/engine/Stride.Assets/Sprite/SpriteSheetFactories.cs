// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets;

namespace Stride.Assets.Sprite
{
    public class SpriteSheetSprite2DFactory : AssetFactory<SpriteSheetAsset>
    {
        public static SpriteSheetAsset Create()
        {
            return new SpriteSheetAsset
            {
                Type = SpriteSheetType.Sprite2D,
            };
        }

        public override SpriteSheetAsset New()
        {
            return Create();
        }
    }

    public class SpriteSheetUIFactory : AssetFactory<SpriteSheetAsset>
    {
        public static SpriteSheetAsset Create()
        {
            return new SpriteSheetAsset
            {
                Type = SpriteSheetType.UI,
            };
        }

        public override SpriteSheetAsset New()
        {
            return Create();
        }
    }

}
