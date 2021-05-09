// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Core.Assets.Editor.Services;
using Stride.Core.BuildEngine;
using Stride.Assets;
using Stride.Editor.Build;

namespace Stride.Editor.Thumbnails
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
