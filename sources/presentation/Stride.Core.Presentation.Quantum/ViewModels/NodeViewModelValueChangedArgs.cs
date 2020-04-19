// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Xenko.Core.Presentation.Quantum.ViewModels
{
    public class NodeViewModelValueChangedArgs : EventArgs
    {
        public NodeViewModelValueChangedArgs(GraphViewModel viewModel, NodeViewModel node)
        {
            ViewModel = viewModel;
            Node = node;
        }

        public GraphViewModel ViewModel { get; private set; }

        public NodeViewModel Node { get; private set; }
    }
}
