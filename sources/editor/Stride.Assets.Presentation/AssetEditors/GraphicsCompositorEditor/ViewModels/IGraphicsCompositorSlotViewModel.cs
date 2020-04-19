// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Presentation.Collections;

namespace Xenko.Assets.Presentation.AssetEditors.GraphicsCompositorEditor.ViewModels
{
    public interface IGraphicsCompositorSlotViewModel
    {
        string Name { get; }

        IGraphicsCompositorBlockViewModel Block { get; }

        IObservableCollection<IGraphicsCompositorLinkViewModel> Links { get; }

        bool CanLinkTo(GraphicsCompositorSlotViewModel target);

        void LinkTo(IGraphicsCompositorSlotViewModel target);

        void ClearLink();
    }
}
