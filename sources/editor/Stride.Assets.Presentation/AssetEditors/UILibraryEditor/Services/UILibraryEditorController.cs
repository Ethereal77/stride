// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Annotations;
using Stride.Assets.Presentation.AssetEditors.UIEditor.Services;
using Stride.Assets.Presentation.AssetEditors.UILibraryEditor.ViewModels;

namespace Stride.Assets.Presentation.AssetEditors.UILibraryEditor.Services
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
