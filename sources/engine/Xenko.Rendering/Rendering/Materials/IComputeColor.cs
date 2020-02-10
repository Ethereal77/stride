// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Annotations;

namespace Xenko.Rendering.Materials
{
    /// <summary>
    /// Base interface for vector/color based compute color nodes.
    /// </summary>
    [InlineProperty]
    public interface IComputeColor : IComputeNode
    {
        /// <summary>
        /// Indicates if the IComputeColor has changed since the last time it was checked, which might require recompilation of the shader mixins.
        /// Once polled, it will reset all cached states and revert to false until other changes have been triggered.
        /// </summary>
        bool HasChanged { get; }        
    }
}
