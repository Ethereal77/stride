// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering
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
