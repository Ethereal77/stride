// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.ComponentModel;

using Stride.Core;
using Stride.Graphics;
using Stride.Graphics.GeometricPrimitives;

namespace Stride.Rendering.ProceduralModels
{
    /// <summary>
    /// A teapot procedural model.
    /// </summary>
    [DataContract("TeapotProceduralModel")]
    [Display("Teapot")]
    public class TeapotProceduralModel : PrimitiveProceduralModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeapotProceduralModel"/> class.
        /// </summary>
        public TeapotProceduralModel()
        {
        }

        /// <summary>
        /// Gets or sets the size of this teapot.
        /// </summary>
        /// <value>The size.</value>
        /// <userdoc>The size of the teapot.</userdoc>
        [DataMember(10)]
        [DefaultValue(1.0f)]
        public float Size { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets the tessellation factor (default: 3.0)
        /// </summary>
        /// <value>The tessellation of the teapot. That is the number of polygons composing it.</value>
        [DataMember(20)]
        [DefaultValue(8)]
        public int Tessellation { get; set; } = 8;

        protected override GeometricMeshData<VertexPositionNormalTexture> CreatePrimitiveMeshData()
        {
            return GeometricPrimitive.Teapot.New(Size, Tessellation, UvScale.X, UvScale.Y);
        }
    }
}
