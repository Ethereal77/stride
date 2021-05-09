// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;

namespace Stride.Rendering.Lights
{
    /// <summary>
    /// Base implementation of <see cref="IDirectLight"/>.
    /// </summary>
    [DataContract]
    public abstract class DirectLightBase : ColorLightBase, IDirectLight
    {
        /// <summary>
        /// Gets or sets the shadow.
        /// </summary>
        /// <value>The shadow.</value>
        /// <userdoc>The settings of the light shadow</userdoc>
        [DataMember(200)]
        public LightShadowMap Shadow { get; protected set; }

        public abstract bool HasBoundingBox { get; }

        public abstract BoundingBox ComputeBounds(Vector3 position, Vector3 direction);

        public abstract float ComputeScreenCoverage(RenderView renderView, Vector3 position, Vector3 direction);
    }
}
