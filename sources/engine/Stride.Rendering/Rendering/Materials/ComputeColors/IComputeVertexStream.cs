// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering.Materials.ComputeColors
{
    /// <summary>
    /// A compute node that retrieve values from the stream.
    /// </summary>
    public interface IComputeVertexStream : IComputeNode
    {
        /// <summary>
        /// Gets or sets the stream.
        /// </summary>
        /// <value>The stream.</value>
        IVertexStreamDefinition Stream { get; set; }
    }
}
