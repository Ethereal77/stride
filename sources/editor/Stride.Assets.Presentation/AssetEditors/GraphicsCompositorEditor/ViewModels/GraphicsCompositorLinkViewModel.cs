// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Annotations;
using Stride.Core.Presentation.ViewModel;

namespace Stride.Assets.Presentation.AssetEditors.GraphicsCompositorEditor.ViewModels
{
    public class GraphicsCompositorLinkViewModel : DispatcherViewModel, IGraphicsCompositorLinkViewModel
    {
        public GraphicsCompositorLinkViewModel([NotNull] IViewModelServiceProvider serviceProvider, [NotNull] GraphicsCompositorSlotViewModel source, [NotNull] GraphicsCompositorSlotViewModel target)
            : base(serviceProvider)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));
            SourceSlot = source;
            TargetSlot = target;
        }

        public IGraphicsCompositorSlotViewModel SourceSlot { get; }

        public IGraphicsCompositorSlotViewModel TargetSlot { get; }
    }
}
