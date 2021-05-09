// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Presentation.Graph.ViewModel;
using Stride.Assets.Scripts;

namespace Stride.Assets.Presentation.AssetEditors.VisualScriptEditor
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
