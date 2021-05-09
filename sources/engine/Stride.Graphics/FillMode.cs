// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    /// <p>Determines the fill mode to use when rendering triangles.</p>
    /// </summary>
    /// <remarks>
    /// <p>This enumeration is part of a rasterizer-state object description (see <strong><see cref="RasterizerStateDescription"/></strong>).</p>
    /// </remarks>
    [DataContract]
    public enum FillMode : int
    {
        /// <summary>
        /// <dd> <p>Draw lines connecting the vertices. Adjacent vertices are not drawn.</p> </dd>
        /// </summary>
        Wireframe = unchecked((int)2),

        /// <summary>
        /// <dd> <p>Fill the triangles formed by the vertices. Adjacent vertices are not drawn.</p> </dd>
        /// </summary>
        Solid = unchecked((int)3),
    }
}
