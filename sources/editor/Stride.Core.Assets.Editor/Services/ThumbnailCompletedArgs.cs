// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Assets.Editor.Services
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
