// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core.Presentation.Quantum.ViewModels;

namespace Xenko.Core.Assets.Editor.ViewModel
{
    public abstract class AddReferenceViewModel : IAddReferenceViewModel
    {
        protected NodeViewModel TargetNode;

        public void SetTargetNode(NodeViewModel node)
        {
            TargetNode = node;
        }

        public abstract bool CanAddChildren(IReadOnlyCollection<object> children, AddChildModifiers modifiers, out string message);

        public abstract void AddChildren(IReadOnlyCollection<object> children, AddChildModifiers modifiers);
    }
}
