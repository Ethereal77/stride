// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Graphics.GeometricPrimitives;

namespace Xenko.Rendering.ProceduralModels
{
    /// <summary>
    /// A cube procedural model
    /// </summary>
    [DataContract("CubeProceduralModel")]
    [Display("Cube")]
    public class CubeProceduralModel : PrimitiveProceduralModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CubeProceduralModel"/> class.
        /// </summary>
        public CubeProceduralModel()
        {
        }

        /// <summary>
        /// Gets or sets the size of the cube.
        /// </summary>
        /// <value>The size.</value>
        /// <userdoc>The size of the cube along the Ox, Oy and Oz axis.</userdoc>
        [DataMember(10)]
        public Vector3 Size { get; set; } = Vector3.One;

        protected override GeometricMeshData<VertexPositionNormalTexture> CreatePrimitiveMeshData()
        {
            return GeometricPrimitive.Cube.New(Size, UvScale.X, UvScale.Y);
        }
    }
}
