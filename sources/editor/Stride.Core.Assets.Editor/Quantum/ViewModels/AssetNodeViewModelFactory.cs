// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.Presenters;
using Stride.Core.Presentation.Quantum.ViewModels;

namespace Stride.Core.Assets.Editor.Quantum.ViewModels
{
    public class AssetNodeViewModelFactory : NodeViewModelFactory
    {
        protected override NodeViewModel CreateNodeViewModel(GraphViewModel owner, NodeViewModel parent, Type nodeType, List<INodePresenter> nodePresenters, bool isRootNode = false)
        {
            // TODO: properly compute the name
            var viewModel = new AssetNodeViewModel(owner, parent, nodePresenters.First().Name, nodeType, nodePresenters);
            if (isRootNode)
            {
                owner.RootNode = viewModel;
            }
            GenerateChildren(owner, viewModel, nodePresenters);
            viewModel.FinishInitialization();
            return viewModel;
        }
    }
}
