// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.Annotations;

namespace Xenko.Core.Assets.Editor.ViewModel.CopyPasteProcessors
{
    public abstract class AssetPostPasteProcessorBase<TAsset> : IAssetPostPasteProcessor
        where TAsset : Asset
    {
        /// <inheritdoc cref="IAssetPostPasteProcessor.PostPasteDeserialization"/>
        protected abstract void PostPasteDeserialization([NotNull] TAsset asset);

        /// <inheritdoc />
        bool IAssetPostPasteProcessor.Accept(Type assetType)
        {
            return typeof(TAsset).IsAssignableFrom(assetType);
        }

        /// <inheritdoc />
        void IAssetPostPasteProcessor.PostPasteDeserialization(Asset asset)
        {
            if (!(asset is TAsset)) throw new ArgumentException("Incompatible type of asset", nameof(asset));
            PostPasteDeserialization((TAsset)asset);
        }
    }
}
