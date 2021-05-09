// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering
{
    /// <summary>
    /// Culling mode of a <see cref="RenderView"/>.
    /// </summary>
    public enum CameraCullingMode
    {
        /// <summary>
        /// No culling is applied to meshes.
        /// </summary>
        /// <userdoc>No specific culling</userdoc>
        None,

        /// <summary>
        /// Meshes outside of the camera's view frustum will be culled.
        /// </summary>
        /// <userdoc>Skip all entities out of the camera frustum.</userdoc>
        Frustum,
    }
}
