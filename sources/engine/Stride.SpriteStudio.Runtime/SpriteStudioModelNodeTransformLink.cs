// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Rendering;

namespace Stride.SpriteStudio.Runtime
{
    public class SpriteStudioNodeTransformLink : TransformLink
    {
        private readonly SpriteStudioComponent parentModelComponent;
        private SpriteStudioSheet sheet;
        private readonly string nodeName;
        private int nodeIndex = int.MaxValue;

        public SpriteStudioNodeTransformLink(SpriteStudioComponent parentModelComponent, string nodeName)
        {
            this.parentModelComponent = parentModelComponent;
            this.nodeName = nodeName;
            sheet = parentModelComponent.Sheet;

            for (var index = 0; index < parentModelComponent.Nodes.Count; index++)
            {
                var node = parentModelComponent.Nodes[index];
                if (node.BaseNode.Name == nodeName)
                {
                    nodeIndex = index;
                    break;
                }
            }
        }

        public TransformTRS Transform;

        /// <inheritdoc/>
        public override void ComputeMatrix(bool recursive, out Matrix matrix)
        {
            // Updated? (rare slow path)
            if (sheet != parentModelComponent.Sheet)
            {
                for (var index = 0; index < parentModelComponent.Nodes.Count; index++)
                {
                    var node = parentModelComponent.Nodes[index];
                    if (node.BaseNode.Name != nodeName) continue;
                    nodeIndex = index;
                    break;
                }

                sheet = parentModelComponent.Sheet;
            }

            var nodes = parentModelComponent.Nodes;
            if (nodeIndex >= nodes.Count)
            {
                // Out of bound: fallback to TransformComponent
                matrix = parentModelComponent.Entity.Transform.WorldMatrix;
                return;
            }

            // Compute using ModelTransform
            Matrix.Multiply(ref nodes[nodeIndex].ModelTransform, ref parentModelComponent.Entity.Transform.WorldMatrix, out matrix);
        }

        public bool NeedsRecreate(Entity parentEntity, string targetNodeName)
        {
            return parentModelComponent.Entity != parentEntity
                || !object.ReferenceEquals(nodeName, targetNodeName); // note: supposed to use same string instance so no need to compare content
        }
    }
}
