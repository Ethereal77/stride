// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Linq;

using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Core.IO;

namespace Xenko.Core.Assets.Editor.Quantum.NodePresenters.Commands
{
    class AssetInitialDirectoryProvider : IInitialDirectoryProvider
    {
        private readonly SessionViewModel session;

        public AssetInitialDirectoryProvider(SessionViewModel session)
        {
            this.session = session;
        }

        public UDirectory GetInitialDirectory(UDirectory currentPath)
        {
            if (session != null && session.ActiveAssetView.SelectedAssets.Count == 1 && session.ActiveAssetView.SelectedAssetsPackage != null && currentPath != null)
            {
                var asset = session.ActiveAssetView.SelectedAssets.First();
                var projectPath = session.ActiveAssetView.SelectedAssetsPackage.PackagePath;
                if (projectPath != null)
                {
                    var assetFullPath = UPath.Combine(projectPath.GetFullDirectory(), new UFile(asset.Url));

                    if (string.IsNullOrWhiteSpace(currentPath))
                    {
                        return assetFullPath.GetFullDirectory();
                    }
                    var defaultPath = UPath.Combine(assetFullPath.GetFullDirectory(), currentPath);
                    return defaultPath.GetFullDirectory();
                }
            }
            return currentPath;
        }
    }
}
