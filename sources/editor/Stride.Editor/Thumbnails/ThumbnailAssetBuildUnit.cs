// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Assets;
using Xenko.Core.Assets.Compiler;
using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.BuildEngine;
using Xenko.Assets;
using Xenko.Editor.Build;

namespace Xenko.Editor.Thumbnails
{
    public class ThumbnailAssetBuildUnit : AssetBuildUnit
    {
        private static readonly Guid ThumbnailBuildUnitContextId = Guid.NewGuid();
        private readonly AssetItem asset;
        private readonly GameStudioThumbnailService thumbnailService;
        private readonly GameSettingsAsset gameSettings;

        public ThumbnailAssetBuildUnit(AssetItem asset, GameSettingsAsset gameSettings, GameStudioThumbnailService thumbnailService, int priorityOrder)
            : base(new AssetBuildUnitIdentifier(ThumbnailBuildUnitContextId, asset.Id))
        {
            this.asset = asset;
            this.thumbnailService = thumbnailService;
            this.gameSettings = gameSettings;

            PriorityMajor = DefaultAssetBuilderPriorities.ThumbnailPriority;
            PriorityMinor = priorityOrder;
        }

        protected override ListBuildStep Prepare()
        {
            return thumbnailService.Compile(asset, gameSettings);
        }
    }
}
