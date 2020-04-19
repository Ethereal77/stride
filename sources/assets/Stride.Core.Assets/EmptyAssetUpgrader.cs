// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Core.Assets
{
    /// <summary>
    /// Empty asset upgrader (useful when we want to bump version without changing anything).
    /// </summary>
    public class EmptyAssetUpgrader : AssetUpgraderBase
    {
        /// <inheritdoc/>
        protected override void UpgradeAsset(AssetMigrationContext context, PackageVersion currentVersion, PackageVersion targetVersion, dynamic asset, PackageLoadingAssetFile assetFile, OverrideUpgraderHint overrideHint)
        {
        }
    }
}
