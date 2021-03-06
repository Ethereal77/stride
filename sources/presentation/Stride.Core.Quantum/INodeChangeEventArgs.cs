// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;

namespace Stride.Core.Quantum
{
    /// <summary>
    /// A global interface representing any kind of change in a node.
    /// </summary>
    public interface INodeChangeEventArgs
    {
        /// <summary>
        /// The node that has changed.
        /// </summary>
        [NotNull]
        IGraphNode Node { get; }

        /// <summary>
        /// The type of change.
        /// </summary>
        ContentChangeType ChangeType { get; }

        /// <summary>
        /// The old value of the node or the item of the node that has changed.
        /// </summary>
        object OldValue { get; }

        /// <summary>
        /// The new value of the node or the item of the node that has changed.
        /// </summary>
        object NewValue { get; }
    }
}
