// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Mathematics;

namespace Stride.Engine
{
    /// <summary>
    ///   Represents a link that gives the ability to control how the parent matrix is computed in a
    ///   <see cref="TransformComponent"/>.
    /// </summary>
    public abstract class TransformLink
    {
        /// <summary>
        ///   Computes a world matrix this link represents.
        /// </summary>
        /// <param name="recursive">A value indicating whether to recurse the transform hierarchy.</param>
        /// <param name="matrix">The output computed world matrix.</param>
        public abstract void ComputeMatrix(bool recursive, out Matrix matrix);
    }
}
