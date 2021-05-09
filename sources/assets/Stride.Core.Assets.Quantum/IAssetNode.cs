// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Quantum;

namespace Stride.Core.Assets.Quantum
{
    /// <summary>
    /// Base interface for <see cref="IGraphNode"/> that represents asset properties.
    /// </summary>
    public interface IAssetNode : IGraphNode
    {
        /// <summary>
        /// Gets the <see cref="AssetPropertyGraph"/> of the related asset.
        /// </summary>
        AssetPropertyGraph PropertyGraph { get; }

        /// <summary>
        /// Gets the base node from which the value inherits, in case it inherits from an Archetype or from part composition.
        /// </summary>
        IGraphNode BaseNode { get; }

        /// <summary>
        /// Attaches a specific node to this node.
        /// </summary>
        /// <param name="key">The key representing the type of the node to attach.</param>
        /// <param name="node">The node to attach.</param>
        void SetContent(string key, IGraphNode node);

        /// <summary>
        /// Retrieves an attached node.
        /// </summary>
        /// <param name="key">The key representing the type of attached node.</param>
        /// <returns>The attached node corresponding to the given key if available, null otherwise.</returns>
        IGraphNode GetContent(string key);

        event EventHandler<EventArgs> OverrideChanging;

        event EventHandler<EventArgs> OverrideChanged;

        /// <summary>
        /// Resets all the overrides attached to this node and its descendants, recursively.
        /// </summary>
        void ResetOverrideRecursively();
    }
}
