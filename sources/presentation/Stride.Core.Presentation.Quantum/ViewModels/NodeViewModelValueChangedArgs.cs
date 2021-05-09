// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Stride.Core.Presentation.Quantum.ViewModels
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
