// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Presentation.Graph.ViewModel;
using Xenko.Assets.Scripts;

namespace Xenko.Assets.Presentation.AssetEditors.VisualScriptEditor
{
    public class LinkNodeEdge : NodeEdge
    {
        internal LinkNodeEdge(VisualScriptLinkViewModel viewModel, BlockNodeVertex source, BlockNodeVertex target) : base(source, target)
        {
            ViewModel = viewModel;
            SourceSlot = viewModel.SourceSlot;
            TargetSlot = viewModel.TargetSlot;
            if (SourceSlot == null || TargetSlot == null)
            {
                throw new InvalidOperationException("Could not find appropriate slots for this link");
            }
        }

        public VisualScriptLinkViewModel ViewModel { get; }
    }
}
