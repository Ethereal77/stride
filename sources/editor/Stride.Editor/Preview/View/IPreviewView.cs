// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Editor.Preview.View
{
    /// <summary>
    /// An interface that represents the view of a preview.
    /// </summary>
    public interface IPreviewView
    {
        /// <summary>
        /// Initializes the view with the given parameters.
        /// </summary>
        /// <param name="previewBuilder">The preview builder used to build the preview.</param>
        /// <param name="assetPreview">The asset preview to display in the view.</param>
        void InitializeView(IPreviewBuilder previewBuilder, IAssetPreview assetPreview);

        /// <summary>
        /// Updates the view, usually after regenerating a new <see cref="IAssetPreview"/> instance.
        /// </summary>
        /// <param name="assetPreview">The asset preview to display in the view.</param>
        void UpdateView(IAssetPreview assetPreview);
    }
}
