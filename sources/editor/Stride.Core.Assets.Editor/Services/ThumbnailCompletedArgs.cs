// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Core.Assets.Editor.Services
{
    public class ThumbnailCompletedArgs : EventArgs
    {
        public ThumbnailCompletedArgs(AssetId assetId, ThumbnailData data)
        {
            AssetId = assetId;
            Data = data;
        }

        public AssetId AssetId { get; private set; }

        public ThumbnailData Data { get; private set; }
    }
}
