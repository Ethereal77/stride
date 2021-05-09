// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;

namespace Stride.Animations
{
    public interface IComputeCurve
    {
        /// <summary>
        /// Updates any optimizations in the curve if data has changed.
        /// </summary>
        /// <returns><c>true</c> there were changes since the last time; otherwise, <c>false</c>.</returns>
        bool UpdateChanges();
    }

    /// <summary>
    /// Base interface for curve based compute value nodes.
    /// </summary>
    [InlineProperty]
    public interface IComputeCurve<out T> : IComputeCurve where T : struct
    {
        /// <summary>
        /// Evaluates the compute curve's value at the specified location, usually in the [0 .. 1] range
        /// </summary>
        /// <param name="location">Location to sample at</param>
        /// <returns>Sampled value</returns>
        T Evaluate(float location);
    }
}
