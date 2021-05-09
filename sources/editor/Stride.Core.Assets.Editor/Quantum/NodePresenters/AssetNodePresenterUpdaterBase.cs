// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;
using Stride.Core.Presentation.Quantum.Presenters;

namespace Stride.Core.Assets.Editor.Quantum.NodePresenters
{
    public abstract class AssetNodePresenterUpdaterBase : NodePresenterUpdaterBase
    {
        public sealed override void UpdateNode(INodePresenter node)
        {
            var assetNode = node as IAssetNodePresenter;
            if (assetNode != null)
            {
                UpdateNode(assetNode);
            }
        }

        public sealed override void FinalizeTree(INodePresenter root)
        {
            var assetNode = root as IAssetNodePresenter;
            if (assetNode != null)
            {
                FinalizeTree(assetNode);
            }
        }

        protected virtual void UpdateNode([NotNull] IAssetNodePresenter node)
        {

        }

        protected virtual void FinalizeTree([NotNull] IAssetNodePresenter root)
        {

        }
    }
}
