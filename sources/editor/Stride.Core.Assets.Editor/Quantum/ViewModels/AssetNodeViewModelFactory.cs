// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Xenko.Core.Presentation.Quantum;
using Xenko.Core.Presentation.Quantum.Presenters;
using Xenko.Core.Presentation.Quantum.ViewModels;

namespace Xenko.Core.Assets.Editor.Quantum.ViewModels
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
