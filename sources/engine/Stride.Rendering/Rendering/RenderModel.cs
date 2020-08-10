// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents the information needed for the <see cref="RenderSystem"/> that a <see cref="RenderMesh"/> needs to access
    ///   from a <see cref="Rendering.Model"/>.
    /// </summary>
    public class RenderModel
    {
        public Model Model;
        public RenderMesh[] Meshes;
        public MaterialInfo[] Materials;

        /// <summary>
        ///   The number of unique <see cref="Mesh"/>es generated in <see cref="Meshes"/>.
        /// </summary>
        /// <remarks>
        ///   A single mesh may be split into multiple <see cref="RenderMesh"/>es due to multiple material passes.
        /// </remarks>
        public int UniqueMeshCount;

        public struct MaterialInfo
        {
            public Material Material;
            public int MeshStartIndex;
            public int MeshCount;
        }
    }
}
