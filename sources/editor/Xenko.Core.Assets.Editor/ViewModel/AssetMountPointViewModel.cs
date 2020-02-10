// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Assets.Editor.ViewModel
{
    public class AssetMountPointViewModel : MountPointViewModel
    {
        public AssetMountPointViewModel(PackageViewModel package)
            : base(package)
        {
        }

        public override string Name { get { return "Assets"; } set { throw new InvalidOperationException("The asset mount point cannot be renamed"); } }

        public override bool IsEditable => false;

        public override bool CanDelete(out string error)
        {
            error = "Unable to delete the asset root folder.";
            return false;
        }

        public override bool AcceptAssetType(Type assetType)
        {
            return !typeof(IProjectAsset).IsAssignableFrom(assetType);
        }
    }
}
