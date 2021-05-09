// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;
using Stride.Core.Presentation.Collections;

namespace Stride.Core.Assets.Editor.ViewModel
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
