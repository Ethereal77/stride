// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Core.Extensions;
using Xenko.Core.Presentation.ViewModel;
using Xenko.Editor.Preview;
using Xenko.Editor.Preview.ViewModel;

namespace Xenko.Assets.Presentation.ViewModel.Preview
{
    /// <summary>
    /// Base implementation of <see cref="IAssetPreview"/>.
    /// </summary>
    public abstract class AssetPreviewViewModel : DispatcherViewModel, IAssetPreviewViewModel
    {
        public SessionViewModel Session { get; }

        protected AssetPreviewViewModel(SessionViewModel session)
            : base(session.SafeArgument(nameof(session)).ServiceProvider)
        {
            this.Session = session;
        }

        /// <inheritdoc/>
        public abstract void AttachPreview(IAssetPreview preview);
    }
}
