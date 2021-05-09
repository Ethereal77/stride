// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets;

namespace Stride.Assets.Physics
{
    public class HeightmapFactory : AssetFactory<HeightmapAsset>
    {
        public static HeightmapAsset Create()
        {
            return new HeightmapAsset();
        }

        public override HeightmapAsset New()
        {
            return Create();
        }
    }
}
