// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.ViewModel;

namespace Xenko.Editor.Preview.ViewModel
{
    /// <summary>
    /// An interface that represents a view model that can be attached to an <see cref="IAssetPreview"/>.
    /// </summary>
    /// <remarks>Implementation should provide at least one constructor with a <see cref="SessionViewModel"/> as first argument.</remarks>
    public interface IAssetPreviewViewModel
    {
        SessionViewModel Session { get; }

        /// <summary>
        /// Attaches the given preview to this view model.
        /// </summary>
        /// <param name="preview">The preview to attach.</param>
        void AttachPreview(IAssetPreview preview);
    }
}
