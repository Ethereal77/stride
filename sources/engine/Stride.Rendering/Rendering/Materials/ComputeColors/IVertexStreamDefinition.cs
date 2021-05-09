// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering.Materials.ComputeColors
{
    /// <summary>
    /// Definition of a stream. e.g COLOR0, COLOR1...etc.
    /// </summary>
    public interface IVertexStreamDefinition
    {
        /// <summary>
        /// Gets the name of the semantic.
        /// </summary>
        /// <returns>A string with the name of the stream semantic.</returns>
        string GetSemanticName();

        /// <summary>
        /// Gets the hash code of the semantic name.
        /// </summary>
        /// <returns>An int with the hash code of the semantic name.</returns>
        int GetSemanticNameHash();
    }
}
