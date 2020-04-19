// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Quantum;
using Xenko.Core.Annotations;

namespace Xenko.Core.Assets.Editor.ViewModel
{
    public abstract class AssetCompositeViewModel<TAsset> : AssetViewModel<TAsset> where TAsset : AssetComposite
    {
        public AssetCompositePropertyGraph AssetCompositePropertyGraph => (AssetCompositePropertyGraph)PropertyGraph;

        protected AssetCompositeViewModel([NotNull] AssetViewModelConstructionParameters parameters) : base(parameters)
        {
        }
    }
}
