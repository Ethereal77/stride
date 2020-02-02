// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Xenko.Core.Annotations;
using Xenko.Core.Presentation.Quantum.Presenters;

namespace Xenko.Core.Presentation.Quantum.ViewModels
{
    public interface INodeViewModelFactory
    {
        [NotNull]
        NodeViewModel CreateGraph([NotNull] GraphViewModel owner, [NotNull] Type rootType, [NotNull] IEnumerable<INodePresenter> rootNodes);

        void GenerateChildren([NotNull] GraphViewModel owner, NodeViewModel parent, [NotNull] List<INodePresenter> nodePresenters);
    }
}
