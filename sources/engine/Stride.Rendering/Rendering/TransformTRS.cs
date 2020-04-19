// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Core.Serialization;

namespace Xenko.Rendering
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
