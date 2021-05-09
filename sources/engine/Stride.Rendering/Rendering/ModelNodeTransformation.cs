// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;

namespace Stride.Rendering
{
    [DataContract]
    public struct ModelNodeTransformation
    {
        public int ParentIndex;

        public TransformTRS Transform;

        public Matrix LocalMatrix;

        public Matrix WorldMatrix;

        public bool IsScalingNegative;

        /// <summary>
        /// The flags of this node.
        /// </summary>
        public ModelNodeFlags Flags;

        internal bool RenderingEnabledRecursive;
    }
}
