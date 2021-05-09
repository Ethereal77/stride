// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Presentation.Collections;

namespace Stride.Assets.Presentation.AssetEditors.GraphicsCompositorEditor.ViewModels
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
