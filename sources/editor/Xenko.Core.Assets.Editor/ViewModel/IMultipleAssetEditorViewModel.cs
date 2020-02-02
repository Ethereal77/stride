// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Annotations;
using Xenko.Core.Presentation.Collections;

namespace Xenko.Core.Assets.Editor.ViewModel
{
    /// <summary>
    /// An interface that represents a the view model of an editor capable of editing more than one asset.
    /// </summary>
    public interface IMultipleAssetEditorViewModel : IAssetEditorViewModel
    {
        /// <summary>
        /// The list of assets opened in this editor.
        /// </summary>
        [ItemNotNull, NotNull]
        IReadOnlyObservableList<AssetViewModel> OpenedAssets { get; }
    }
}
