// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Core.Serialization;

namespace Xenko.Rendering
{
    /// <summary>
    /// Describes a bone cluster inside a <see cref="Mesh"/>.
    /// </summary>
    [DataContract]
    public struct MeshBoneDefinition
    {
        /// <summary>
        /// The node index in <see cref="SkeletonUpdater.NodeTransformations"/>.
        /// </summary>
        public int NodeIndex;
        
        /// <summary>
        /// The matrix to transform from mesh space to local space of this bone.
        /// </summary>
        public Matrix LinkToMeshMatrix;
    }
}
