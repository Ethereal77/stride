// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Graphics
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
