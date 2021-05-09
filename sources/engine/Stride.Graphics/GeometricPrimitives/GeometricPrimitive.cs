// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;
using Stride.Rendering;

namespace Stride.Graphics.GeometricPrimitives
{
    /// <summary>
    /// A geometric primitive used to draw a simple model built from a set of vertices and indices.
    /// </summary>
    public class GeometricPrimitive<T> : ComponentBase where T : struct, IVertex
    {
        /// <summary>
        /// The pipeline state.
        /// </summary>
        public readonly MutablePipelineState PipelineState;

        /// <summary>
        /// The index buffer used by this geometric primitive.
        /// </summary>
        public readonly Buffer IndexBuffer;

        /// <summary>
        /// The vertex buffer used by this geometric primitive.
        /// </summary>
        public readonly Buffer VertexBuffer;

        /// <summary>
        /// The default graphics device.
        /// </summary>
        protected readonly GraphicsDevice GraphicsDevice;

        /// <summary>
        /// The input layout used by this geometric primitive (shared for all geometric primitive).
        /// </summary>
        protected readonly VertexBufferBinding VertexBufferBinding;

        /// <summary>
        /// True if the index buffer is a 32 bit index buffer.
        /// </summary>
        public readonly bool IsIndex32Bits;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricPrimitive{T}"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="geometryMesh">The geometry mesh.</param>
        /// <exception cref="System.InvalidOperationException">Cannot generate more than 65535 indices on feature level HW <= 9.3</exception>
        public GeometricPrimitive(GraphicsDevice graphicsDevice, GeometricMeshData<T> geometryMesh)
        {
            GraphicsDevice = graphicsDevice;
            PipelineState = new MutablePipelineState(graphicsDevice);

            var vertices = geometryMesh.Vertices;
            var indices = geometryMesh.Indices;

            if (geometryMesh.IsLeftHanded)
                ReverseWinding(vertices, indices);

            if (indices.Length < 0xFFFF)
            {
                var indicesShort = new ushort[indices.Length];
                for (int i = 0; i < indicesShort.Length; i++)
                {
                    indicesShort[i] = (ushort)indices[i];
                }
                IndexBuffer = Buffer.Index.New(graphicsDevice, indicesShort).RecreateWith(indicesShort).DisposeBy(this);
            }
            else
            {
                IndexBuffer = Buffer.Index.New(graphicsDevice, indices).RecreateWith(indices).DisposeBy(this);
                IsIndex32Bits = true;
            }

            // For now it will keep buffers for recreation.
            // TODO: A better alternative would be to store recreation parameters so that we can reuse procedural code.
            VertexBuffer = Buffer.Vertex.New(graphicsDevice, vertices).RecreateWith(vertices).DisposeBy(this);
            VertexBufferBinding = new VertexBufferBinding(VertexBuffer, new T().GetLayout(), vertices.Length);

            PipelineState.State.SetDefaults();
            PipelineState.State.InputElements = VertexBufferBinding.Declaration.CreateInputElements();
            PipelineState.State.PrimitiveType = PrimitiveQuad.PrimitiveType;
        }

        /// <summary>
        /// Draws this <see cref="GeometricPrimitive" />.
        /// </summary>
        /// <param name="commandList">The command list.</param>
        public void Draw(GraphicsContext graphicsContext, EffectInstance effectInstance)
        {
            var commandList = graphicsContext.CommandList;

            // Update pipeline state
            PipelineState.State.RootSignature = effectInstance.RootSignature;
            PipelineState.State.EffectBytecode = effectInstance.Effect.Bytecode;
            PipelineState.State.Output.CaptureState(commandList);
            PipelineState.Update();
            commandList.SetPipelineState(PipelineState.CurrentState);

            effectInstance.Apply(graphicsContext);

            // Setup the Vertex Buffer
            commandList.SetIndexBuffer(IndexBuffer, 0, IsIndex32Bits);
            commandList.SetVertexBuffer(0, VertexBuffer, 0, VertexBufferBinding.Stride);

            // Finally Draw this mesh
            commandList.DrawIndexed(IndexBuffer.ElementCount);
        }

        /// <summary>
        /// Helper for flipping winding of geometric primitives for LH vs. RH coordinates
        /// </summary>
        /// <typeparam name="TIndex">The type of the T index.</typeparam>
        /// <param name="vertices">The vertices.</param>
        /// <param name="indices">The indices.</param>
        private void ReverseWinding<TIndex>(T[] vertices, TIndex[] indices)
        {
            for (int i = 0; i < indices.Length; i += 3)
            {
                Utilities.Swap(ref indices[i], ref indices[i + 2]);
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].FlipWinding();
            }
        }
    }

    /// <summary>
    /// A geometric primitive. Use <see cref="Cube"/>, <see cref="Cylinder"/>, <see cref="GeoSphere"/>, <see cref="Plane"/>, <see cref="Sphere"/>, <see cref="Teapot"/>, <see cref="Torus"/>. See <see cref="Draw+vertices"/> to learn how to use it.
    /// </summary>
    public partial class GeometricPrimitive : GeometricPrimitive<VertexPositionNormalTexture>
    {
        public GeometricPrimitive(GraphicsDevice graphicsDevice, GeometricMeshData<VertexPositionNormalTexture> geometryMesh) : base(graphicsDevice, geometryMesh)
        {
        }
    }
}
