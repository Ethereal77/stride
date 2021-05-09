// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Physics
{
    /// <summary>
    /// Defines the different possible orientations of a shape.
    /// </summary>
    public enum ShapeOrientation
    {
        /// <summary>
        /// The shape is aligned with the Ox axis.
        /// </summary>
        /// <userdoc>The top of shape is aligned with the Ox axis.</userdoc>
        UpX,

        /// <summary>
        /// The shape is aligned with the Oy axis.
        /// </summary>
        /// <userdoc>The top shape is aligned with the Oy axis.</userdoc>
        UpY,

        /// <summary>
        /// The shape is aligned with the Oz axis.
        /// </summary>
        /// <userdoc>The top shape is aligned with the Oz axis.</userdoc>
        UpZ,
    }
}
