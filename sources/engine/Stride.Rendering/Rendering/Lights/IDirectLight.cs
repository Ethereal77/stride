// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Engine;

namespace Stride.Rendering.Lights
{
    /// <summary>
    /// Base interface for all direct lights.
    /// </summary>
    public interface IDirectLight : IColorLight
    {
        /// <summary>
        /// Gets or sets the shadow.
        /// </summary>
        /// <value>The shadow.</value>
        LightShadowMap Shadow { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has a bounding box.
        /// </summary>
        /// <value><c>true</c> if this instance has a bounding box; otherwise, <c>false</c>.</value>
        bool HasBoundingBox { get; }

        /// <summary>
        /// Computes the bounds of this light..
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>BoundingBox.</returns>
        BoundingBox ComputeBounds(Vector3 position, Vector3 direction);

        /// <summary>
        /// Computes the screen coverage of this light in pixel.
        /// </summary>
        /// <param name="renderView">The render view.</param>
        /// <param name="position">The position of the light in world space.</param>
        /// <param name="direction">The direction of the light in world space.</param>
        /// <returns>The largest screen coverage width or height size in pixels of this light.</returns>
        float ComputeScreenCoverage(RenderView renderView, Vector3 position, Vector3 direction);
    }
}
