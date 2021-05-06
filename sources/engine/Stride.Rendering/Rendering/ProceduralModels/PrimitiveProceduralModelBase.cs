// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.Graphics.Data;

using Buffer = Stride.Graphics.Buffer;

namespace Stride.Rendering.ProceduralModels
{
    /// <summary>
    ///   Base class for the primitive procedural models.
    /// </summary>
    [DataContract]
    public abstract class PrimitiveProceduralModelBase : IProceduralModel
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="PrimitiveProceduralModelBase"/> class.
        /// </summary>
        protected PrimitiveProceduralModelBase()
        {
            MaterialInstance = new MaterialInstance();
            UvScale = Vector2.One;
        }


        /// <summary>
        ///   Sets the material to use when rendering the procedural model.
        /// </summary>
        /// <param name="name">Name of the material slot to set.</param>
        /// <param name="material">The material.</param>
        public void SetMaterial(string name, Material material)
        {
            if (name == "Material")
                MaterialInstance.Material = material;
        }

        /// <summary>
        ///   Scale of the model's geometry scaling factors.
        /// </summary>
        /// <value>The scaling factors to apply when generating the procedural model.</value>
        [DataMember(510)]
        public Vector3 Scale { get; set; } = Vector3.One;

        /// <summary>
        ///   Gets or sets the texture-space UV scaling factors.
        /// </summary>
        /// <value>The scale to apply to the UV coordinates of the shape. This can be used to tile a texture on it.</value>
        /// <userdoc>The scale to apply to the UV coordinates of the shape. This can be used to tile a texture on it.</userdoc>
        [DataMember(520)]
        [Display("UV Scale")]
        public Vector2 UvScale { get; set; }

        /// <summary>
        ///   Gets or sets the local offset that will be applied to the procedural model's vertices.
        /// </summary>
        [DataMember(530)]
        public Vector3 LocalOffset { get; set; }

        /// <summary>
        ///   Gets or sets the number of texture coordinate channels to generate.
        /// </summary>
        /// <value>
        ///   The number of texure coordinate channels. A value between 1 and 10, inclusive.
        /// </value>
        [DataMember(540)]
        [DataMemberRange(1, 10)]
        [Display("Number of texture coordinate channels")]
        public int NumberOfTextureCoordinates  { get; set; } = 10;

        /// <summary>
        ///   Gets the material instance to use for the model.
        /// </summary>
        /// <userdoc>The reference material asset to use with this model.</userdoc>
        [DataMember(600)]
        [NotNull]
        [Display("Material")]
        public MaterialInstance MaterialInstance { get; private set; }

        /// <inheritdoc/>
        [DataMemberIgnore]
        public IEnumerable<KeyValuePair<string, MaterialInstance>> MaterialInstances { get { yield return new KeyValuePair<string, MaterialInstance>("Material", MaterialInstance); } }


        /// <inheritdoc/>
        public void Generate(IServiceRegistry services, Model model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            var needsTempDevice = false;
            var graphicsDevice = services?.GetSafeServiceAs<IGraphicsDeviceService>().GraphicsDevice;
            if (graphicsDevice is null)
            {
                graphicsDevice = GraphicsDevice.New();
                needsTempDevice = true;
            }

            var data = CreatePrimitiveMeshData();

            if (data.Vertices.Length == 0)
                throw new InvalidOperationException("Invalid GeometricPrimitive [{0}]. Expecting non-zero Vertices array.");

            // Translate if necessary
            if (LocalOffset != Vector3.Zero)
                for (var index = 0; index < data.Vertices.Length; index++)
                    data.Vertices[index].Position += LocalOffset;

            // Scale if necessary
            if (Scale != Vector3.One)
            {
                var inverseMatrix = Matrix.Scaling(Scale);
                inverseMatrix.Invert();

                for (var index = 0; index < data.Vertices.Length; index++)
                {
                    data.Vertices[index].Position *= Scale;
                    // TODO: Shouldn't be TransformNormal?
                    Vector3.TransformCoordinate(ref data.Vertices[index].Normal, ref inverseMatrix, out data.Vertices[index].Normal);
                }
            }

            var boundingBox = BoundingBox.Empty;
            for (int i = 0; i < data.Vertices.Length; i++)
                BoundingBox.Merge(ref boundingBox, ref data.Vertices[i].Position, out boundingBox);

            BoundingSphere boundingSphere;
            unsafe
            {
                fixed (void* verticesPtr = data.Vertices)
                    BoundingSphere.FromPoints((IntPtr)verticesPtr, 0, data.Vertices.Length, VertexPositionNormalTexture.Size, out boundingSphere);
            }

            var originalLayout = data.Vertices[0].GetLayout();

            // Generate Tangent/BiNormal vectors
            var resultWithTangentBiNormal = VertexHelper.GenerateTangentBinormal(originalLayout, data.Vertices, data.Indices);

            // Generate Multi texturing coords
            var maxTexCoords = MathUtil.Clamp(NumberOfTextureCoordinates, 1, 10) - 1;
            var result = VertexHelper.GenerateMultiTextureCoordinates(resultWithTangentBiNormal, vertexStride: 0, maxTexCoords);

            var meshDraw = new MeshDraw();

            var layout = result.Layout;
            var vertexBuffer = result.VertexBuffer;
            var indices = data.Indices;

            if (indices.Length < 0xFFFF)
            {
                // 16-bit indices
                var indicesShort = new ushort[indices.Length];
                for (int i = 0; i < indicesShort.Length; i++)
                    indicesShort[i] = (ushort) indices[i];

                var indexBuffer = Buffer.Index.New(graphicsDevice, indicesShort)
                                              .RecreateWith(indicesShort);

                meshDraw.IndexBuffer = new IndexBufferBinding(indexBuffer, is32Bit: false, indices.Length);

                if (needsTempDevice)
                {
                    var indexData = BufferData.New(BufferFlags.IndexBuffer, indicesShort);
                    meshDraw.IndexBuffer = new IndexBufferBinding(indexData.ToSerializableVersion(), is32Bit: false, indices.Length);
                }
            }
            else
            {
                // 32-bit indices
                var indexBuffer = Buffer.Index.New(graphicsDevice, indices)
                                              .RecreateWith(indices);

                meshDraw.IndexBuffer = new IndexBufferBinding(indexBuffer, is32Bit: true, indices.Length);

                if (needsTempDevice)
                {
                    var indexData = BufferData.New(BufferFlags.IndexBuffer, indices);
                    meshDraw.IndexBuffer = new IndexBufferBinding(indexData.ToSerializableVersion(), is32Bit: true, indices.Length);
                }
            }

            var geometryBuffer = Buffer.New(graphicsDevice, vertexBuffer, BufferFlags.VertexBuffer)
                                       .RecreateWith(vertexBuffer);

            meshDraw.VertexBuffers = new[] { new VertexBufferBinding(geometryBuffer, layout, data.Vertices.Length) };

            if (needsTempDevice)
            {
                var vertexData = BufferData.New(BufferFlags.VertexBuffer, vertexBuffer);
                meshDraw.VertexBuffers = new[] { new VertexBufferBinding(vertexData.ToSerializableVersion(), layout, data.Vertices.Length) };
            }

            meshDraw.DrawCount = indices.Length;
            meshDraw.PrimitiveType = PrimitiveType.TriangleList;

            var mesh = new Mesh
            {
                Draw = meshDraw,
                BoundingBox = boundingBox,
                BoundingSphere = boundingSphere
            };

            model.BoundingBox = boundingBox;
            model.BoundingSphere = boundingSphere;
            model.Add(mesh);

            if (MaterialInstance?.Material is not null)
            {
                model.Materials.Add(MaterialInstance);
            }

            if (needsTempDevice)
            {
                graphicsDevice.Dispose();
            }
        }

        protected abstract GeometricMeshData<VertexPositionNormalTexture> CreatePrimitiveMeshData();
    }
}
