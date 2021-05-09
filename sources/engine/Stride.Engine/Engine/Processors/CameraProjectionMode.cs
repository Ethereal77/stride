// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Engine.Processors
{
    /// <summary>
    ///   Defines the projection modes of a <see cref="CameraComponent"/>.
    /// </summary>
    [DataContract("CameraProjectionMode")]
    public enum CameraProjectionMode
    {
        /// <summary>
        ///   The camera defines a perspective projection.
        /// </summary>
        /// <userdoc>A perspective projection (usually used for 3D games).</userdoc>
        Perspective,

        /// <summary>
        ///   The camera defines an orthographic projection.
        /// </summary>
        /// <userdoc>An orthographic projection (usually used for 2D games).</userdoc>
        Orthographic
    }
}
