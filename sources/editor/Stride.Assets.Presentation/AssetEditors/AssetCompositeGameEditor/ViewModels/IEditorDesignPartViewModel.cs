// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Assets;
using Stride.Core;
using Stride.Core.Annotations;

namespace Stride.Assets.Presentation.AssetEditors.AssetCompositeGameEditor.ViewModels
{
    /// <summary>
    /// An interface for view models that contain a design part in an editor of <see cref="AssetComposite"/>.
    /// </summary>
    public interface IEditorDesignPartViewModel<out TAssetPartDesign, TAssetPart>
        where TAssetPartDesign : IAssetPartDesign<TAssetPart>
        where TAssetPart : IIdentifiable
    {
        /// <summary>
        /// Gets the part design object associated to the asset-side part.
        /// </summary>
        [NotNull]
        TAssetPartDesign PartDesign { get; }
    }
}
