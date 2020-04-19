// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Annotations;
using Xenko.Core.Reflection;
using Xenko.Core.Quantum.References;

namespace Xenko.Core.Quantum
{
    public interface IMemberNode : IGraphNode, INotifyNodeValueChange
    {
        /// <summary>
        /// Gets the member name.
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="IObjectNode"/> containing this member node.
        /// </summary>
        [NotNull]
        IObjectNode Parent { get; }

        ObjectReference TargetReference { get; }

        /// <summary>
        /// Gets the target of this node, if this node contains a reference to another node.
        /// </summary>
        /// <exception cref="InvalidOperationException">The node does not contain a reference to another node.</exception>
        [CanBeNull]
        IObjectNode Target { get; }

        /// <summary>
        /// Gets the member descriptor corresponding to this member node.
        /// </summary>
        [NotNull]
        IMemberDescriptor MemberDescriptor { get; }

        /// <summary>
        /// Updates the value of this content with the given value.
        /// </summary>
        /// <param name="newValue">The new value to set.</param>
        void Update(object newValue);
    }
}
