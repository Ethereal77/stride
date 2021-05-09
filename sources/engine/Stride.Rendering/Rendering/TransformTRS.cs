// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Core.Serialization;

namespace Stride.Rendering
{
    /// <summary>
    /// Stores transformation in a TRS format (Position, Rotation and Scale).
    /// </summary>
    /// <remarks>
    /// It first applies scaling, then rotation, then translation.
    /// Rotation is stored in a Quaternion so that animation system can provides smooth rotation interpolations and blending.
    /// </remarks>
    [DataContract]
    public struct TransformTRS
    {
        /// <summary>
        /// The translation.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The rotation.
        /// </summary>
        public Quaternion Rotation;

        /// <summary>
        /// The scaling
        /// </summary>
        public Vector3 Scale;
    }
}
