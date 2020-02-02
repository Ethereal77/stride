// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Linq;

using Xenko.Core.Extensions;
using Xenko.Core.Presentation.Collections;
using Xenko.Core.Presentation.ViewModel;

namespace Xenko.Assets.Presentation.AssetEditors.GraphicsCompositorEditor.ViewModels
{
    public abstract class GraphicsCompositorSlotViewModel : DispatcherViewModel, IGraphicsCompositorSlotViewModel
    {
        protected GraphicsCompositorSlotViewModel(GraphicsCompositorBlockViewModel block, string name)
            : base(block.SafeArgument(nameof(block)).ServiceProvider)
        {
            Block = block;
            Name = name;
        }

        public string Name { get; }

        public IGraphicsCompositorBlockViewModel Block { get; }

        public IObservableCollection<IGraphicsCompositorLinkViewModel> Links { get; } = new ObservableList<IGraphicsCompositorLinkViewModel>();

        public abstract void UpdateLink();

        public abstract bool CanLinkTo(GraphicsCompositorSlotViewModel target);

        public abstract void LinkTo(IGraphicsCompositorSlotViewModel target);

        public abstract void ClearLink();

        public override void Destroy()
        {
            base.Destroy();
            foreach (var link in Links.Cast<GraphicsCompositorLinkViewModel>().ToList())
            {
                link.Destroy();
                link.SourceSlot.Links.Remove(link);
                link.TargetSlot.Links.Remove(link);
            }
            Links.Clear();
        }
    }
}
