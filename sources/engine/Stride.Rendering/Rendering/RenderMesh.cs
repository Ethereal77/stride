// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Rendering.Materials;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents a <see cref="RenderObject"/> used by <see cref="MeshRenderFeature"/> to render a <see cref="Rendering.Mesh"/>.
    /// </summary>
    public class RenderMesh : RenderObject
    {
        public MeshDraw ActiveMeshDraw;

        public RenderModel RenderModel;

        /// <summary>
        ///   The underlying mesh.
        /// </summary>
        /// <remarks>
        ///   The mesh can be accessed only during <see cref="RenderFeature.Extract"/> phase.
        /// </remarks>
        public Mesh Mesh;

        // Material
        // TODO: Extract with MaterialRenderFeature
        public MaterialPass MaterialPass;

        // TODO: GRAPHICS REFACTOR store that in RenderData (StaticObjectNode?)
        internal MaterialRenderFeature.MaterialInfo MaterialInfo;

        public bool IsShadowCaster;

        public bool IsScalingNegative;
        public bool IsPreviousScalingNegative;

        public Matrix World = Matrix.Identity;

        public Matrix[] BlendMatrices;

        public int InstanceCount;
    }
}
