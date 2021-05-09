// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Extensions;
using Stride.Core.Presentation.ViewModel;
using Stride.Editor.Preview;
using Stride.Editor.Preview.ViewModel;

namespace Stride.Assets.Presentation.ViewModel.Preview
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
