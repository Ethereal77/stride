// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets;

namespace Xenko.Assets.Sprite
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
