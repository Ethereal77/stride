// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    /// Indicates triangles facing a particular direction are not drawn.
    /// </summary>
    /// <remarks>
    /// This enumeration is part of a rasterizer-state object description (see <see cref="RasterizerState"/>). 
    /// </remarks>
    [DataContract]
    public enum CullMode 
    {
        /// <summary>
        /// Always draw all triangles. 
        /// </summary>
        None = 1,

        /// <summary>
        /// Do not draw triangles that are front-facing. 
        /// </summary>
        Front = 2,

        /// <summary>
        /// Do not draw triangles that are back-facing. 
        /// </summary>
        Back = 3,
    }
}
