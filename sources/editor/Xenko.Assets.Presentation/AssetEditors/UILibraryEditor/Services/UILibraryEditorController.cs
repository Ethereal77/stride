// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Core.Annotations;
using Xenko.Assets.Presentation.AssetEditors.UIEditor.Services;
using Xenko.Assets.Presentation.AssetEditors.UILibraryEditor.ViewModels;

namespace Xenko.Assets.Presentation.AssetEditors.UILibraryEditor.Services
{
    /// <summary>
    /// Game controller for the UI library editor.
    /// </summary>
    public sealed class UILibraryEditorController : UIEditorController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIEditorController"/> class.
        /// </summary>
        /// <param name="asset">The UI library associated with this instance.</param>
        /// <param name="editor">The editor associated with this instance.</param>
        public UILibraryEditorController([NotNull] AssetViewModel asset, [NotNull] UILibraryEditorViewModel editor)
            : base(asset, editor)
        {
        }
    }
}
