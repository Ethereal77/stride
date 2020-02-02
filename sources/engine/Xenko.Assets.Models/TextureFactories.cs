// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xenko.Core.Assets;
using Xenko.Assets.Textures;

namespace Xenko.Assets.Models
{
    public class ColorTextureFactory : AssetFactory<TextureAsset>
    {
        public static TextureAsset Create()
        {
            return new TextureAsset { Type = new ColorTextureType() };
        }

        public override TextureAsset New()
        {
            return Create();
        }
    }

    public class NormalMapTextureFactory : AssetFactory<TextureAsset>
    {
        public static TextureAsset Create()
        {
            return new TextureAsset { Type = new NormapMapTextureType() };
        }

        public override TextureAsset New()
        {
            return Create();
        }
    }

    public class GrayscaleTextureFactory : AssetFactory<TextureAsset>
    {
        public static TextureAsset Create()
        {
            return new TextureAsset { Type = new GrayscaleTextureType() };
        }

        public override TextureAsset New()
        {
            return Create();
        }
    }
}
