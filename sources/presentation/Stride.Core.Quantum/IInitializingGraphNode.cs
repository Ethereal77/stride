// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Quantum
{
    /// <summary>
    /// An interface representing an <see cref="IGraphNode"/> during its initialization phase.
    /// </summary>
    public interface IInitializingGraphNode : IGraphNode
    {
        /// <summary>
        /// Seal the node, indicating its construction is finished and that no more children will be added.
        /// </summary>
        void Seal();
    }
}
