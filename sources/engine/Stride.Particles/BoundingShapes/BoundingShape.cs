// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.ComponentModel;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Particles.DebugDraw;

namespace Stride.Particles.BoundingShapes
{
    [DataContract("BoundingShape")]
    public abstract class BoundingShape
    {
        [DataMemberIgnore]
        public bool Dirty { get; set; } = true;

        // ReSharper disable once InconsistentNaming
        public abstract BoundingBox GetAABB(Vector3 translation, Quaternion rotation, float scale);

        /// <summary>
        /// Should the Bounding shape's bounds be displayed as a debug draw
        /// </summary>
        /// <userdoc>
        /// Display the Bounding shape's boinds as a wireframe debug shape. Temporary feature (will be removed later)!
        /// </userdoc>
        [DataMember(-1)]
        [DefaultValue(false)]
        public bool DebugDraw { get; set; } = false;

        public virtual bool TryGetDebugDrawShape(out DebugDrawShape debugDrawShape, out Vector3 translation, out Quaternion rotation, out Vector3 scale)
        {
            debugDrawShape = DebugDrawShape.None;
            scale = Vector3.One;
            translation = Vector3.Zero;
            rotation = Quaternion.Identity;
            return false;
        }
    }
}
