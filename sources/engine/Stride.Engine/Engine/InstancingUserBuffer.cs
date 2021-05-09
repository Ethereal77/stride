// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2020 Tebjan Halm
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Engine
{
    [DataContract("InstancingUserBuffer")]
    [Display("UserBuffer")]
    public class InstancingUserBuffer : IInstancing
    {
        [DataMember(10)]
        [Display("Model Transformation Usage")]
        public virtual ModelTransformUsage ModelTransformUsage { get; set; }

        /// <summary>
        /// The instance count
        /// </summary>
        [DataMemberIgnore]
        public virtual int InstanceCount { get; set; }

        /// <summary>
        /// The bounding box of the world matrices, updated automatically by the <see cref="InstancingProcessor"/>.
        /// </summary>
        [DataMemberIgnore]
        public virtual BoundingBox BoundingBox { get; set; } = BoundingBox.Empty;

        [DataMemberIgnore]
        public Buffer InstanceWorldBuffer;

        [DataMemberIgnore]
        public Buffer InstanceWorldInverseBuffer;

        public void Update()
        {
            // No op, Assumes the user has done everything.
        }
    }
}
