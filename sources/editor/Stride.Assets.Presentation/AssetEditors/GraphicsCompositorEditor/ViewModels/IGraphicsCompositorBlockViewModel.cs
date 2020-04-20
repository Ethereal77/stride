// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Presentation.Collections;

namespace Stride.Assets.Presentation.AssetEditors.GraphicsCompositorEditor.ViewModels
{
    public interface IGraphicsCompositorBlockViewModel
    {
        IObservableList<IGraphicsCompositorSlotViewModel> InputSlots { get; }

        IObservableList<IGraphicsCompositorSlotViewModel> OutputSlots { get; }
    }
}
