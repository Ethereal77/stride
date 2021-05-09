// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Yaml.Serialization;

namespace Stride.Core.Assets
{
    public interface IAssetUpgrader
    {
        void Upgrade(AssetMigrationContext context, string dependencyName, PackageVersion currentVersion, PackageVersion targetVersion, YamlMappingNode yamlAssetNode, PackageLoadingAssetFile assetFile);
    }
}
