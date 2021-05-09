// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Annotations;

namespace Stride.Core.Assets.Editor.Services
{
    /// <summary>
    /// This interface represents the view of an asset editor.
    /// </summary>
    public interface IEditorView
    {
        /// <summary>
        /// Gets or sets the data context for an element when it participates in data binding.
        /// </summary>
        object DataContext { get; set; }

        /// <summary>
        /// Gets a task that is completed when the initialization is done.
        /// </summary>
        [NotNull]
        Task EditorInitialization { get; }

        /// <summary>
        /// Initializes the editor view with the given asset.
        /// </summary>
        /// <param name="asset">The asset for which to initialize the editor view.</param>
        /// <returns>A task that completes when the initialization is done and contains the <see cref="IAssetEditorViewModel"/> as result.</returns>
        [ItemCanBeNull]
        Task<IAssetEditorViewModel> InitializeEditor([NotNull] AssetViewModel asset);
    }
}
