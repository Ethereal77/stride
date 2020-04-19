// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.Quantum.NodePresenters.Keys;

namespace Xenko.Core.Assets.Editor.Quantum.NodePresenters.Updaters
{
    internal sealed class OwnerAssetUpdater : AssetNodePresenterUpdaterBase
    {
        protected override void UpdateNode(IAssetNodePresenter node)
        {
            if (node.Asset != null)
            {
                node.AttachedProperties.Add(OwnerAssetData.Key, node.Asset);
            }
        }
    }
}
