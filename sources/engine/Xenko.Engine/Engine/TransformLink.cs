// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Mathematics;

namespace Xenko.Engine
{
    /// <summary>
    /// Gives the ability to control how parent matrix is computed in a <see cref="TransformComponent"/>.
    /// </summary>
    public abstract class TransformLink
    {
        /// <summary>
        /// Compute a world matrix this link represents.
        /// </summary>
        /// <param name="recursive"></param>
        /// <param name="matrix">The computed world matrix.</param>
        public abstract void ComputeMatrix(bool recursive, out Matrix matrix);
    }
}
