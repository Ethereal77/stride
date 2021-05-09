// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Lights;
using Stride.Rendering.Voxels;

using Buffer = Stride.Graphics.Buffer;

namespace Stride.Assets.Presentation.AssetEditors.Gizmos
{
    /// <summary>
    ///   A gizmo to display the bounding boxes for voxel volumes inside the editor as a gizmo. 
    ///   This gizmo uses volume scale as the extent of the bounding box and is not affected by rotation.
    /// </summary>
    [GizmoComponent(typeof(VoxelVolumeComponent), false)]
    public class VoxelVolumeGizmo : EntityGizmo<VoxelVolumeComponent>
    {
        private BoxMesh box;
        private Material material;
        private Entity debugRootEntity;
        private Entity debugEntity;

        public VoxelVolumeGizmo(EntityComponent component) : base(component)
        {
        }

        protected override Entity Create()
        {
            debugRootEntity = new Entity($"Voxel volume of {Component.Entity.Name}");

            material = GizmoUniformColorMaterial.Create(GraphicsDevice, Color.CornflowerBlue);

            box = new BoxMesh(GraphicsDevice);
            box.Build();

            debugEntity = new Entity($"Voxel volume mesh of {Component.Entity.Name}")
            {
                new ModelComponent
                {
                    Model = new Model
                    {
                        material,
                        new Mesh { Draw = box.MeshDraw },
                    },
                    RenderGroup = RenderGroup,
                }
            };

            return debugRootEntity;
        }

        public override void Update()
        {
            if (ContentEntity == null || GizmoRootEntity == null)
                return;

            // Calculate the world matrix of the gizmo so that it is positioned exactly as the corresponding scene entity
            // except the scale that is re-adjusted to the gizmo desired size
            // (gizmos are inserted at scene root so LocalMatrix = WorldMatrix)
            ContentEntity.Transform.WorldMatrix.Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation);

            // Translation and Scale but no Rotation on bounding boxes
            GizmoRootEntity.Transform.Position = translation;
            GizmoRootEntity.Transform.Scale = new Vector3(Component.VoxelVolumeSize * 0.5f);
            GizmoRootEntity.Transform.UpdateWorldMatrix();
        }

        public override bool IsSelected
        {
            set
            {
                bool hasChanged = IsSelected != value;
                base.IsSelected = value;

                if (hasChanged)
                {
                    if (IsSelected)
                        GizmoRootEntity.AddChild(debugEntity);
                    else
                        GizmoRootEntity.RemoveChild(debugEntity);
                }
            }
        }

        private class BoxMesh
        {
            public MeshDraw MeshDraw;

            private Buffer vertexBuffer;

            private readonly GraphicsDevice graphicsDevice;

            public BoxMesh(GraphicsDevice graphicsDevice)
            {
                this.graphicsDevice = graphicsDevice;
            }

            public void Build()
            {
                var indices = new int[12 * 2];
                var vertices = new VertexPositionNormalTexture[8];

                vertices[0] = new VertexPositionNormalTexture(new Vector3(-1, 1, -1), Vector3.UnitY, Vector2.Zero);
                vertices[1] = new VertexPositionNormalTexture(new Vector3(-1, 1, 1), Vector3.UnitY, Vector2.Zero);
                vertices[2] = new VertexPositionNormalTexture(new Vector3(1, 1, 1), Vector3.UnitY, Vector2.Zero);
                vertices[3] = new VertexPositionNormalTexture(new Vector3(1, 1, -1), Vector3.UnitY, Vector2.Zero);

                int indexOffset = 0;
                // Top sides
                for (int i = 0; i < 4; i++)
                {
                    indices[indexOffset++] = i;
                    indices[indexOffset++] = (i + 1) % 4;
                }

                // Duplicate vertices and indices to bottom part
                for (int i = 0; i < 4; i++)
                {
                    vertices[i + 4] = vertices[i];
                    vertices[i + 4].Position.Y = -vertices[i + 4].Position.Y;

                    indices[indexOffset++] = indices[i * 2] + 4;
                    indices[indexOffset++] = indices[i * 2 + 1] + 4;
                }

                // Sides
                for (int i = 0; i < 4; i++)
                {
                    indices[indexOffset++] = i;
                    indices[indexOffset++] = i + 4;
                }

                vertexBuffer = Buffer.Vertex.New(graphicsDevice, vertices);
                MeshDraw = new MeshDraw
                {
                    PrimitiveType = PrimitiveType.LineList,
                    DrawCount = indices.Length,
                    IndexBuffer = new IndexBufferBinding(Buffer.Index.New(graphicsDevice, indices), true, indices.Length),
                    VertexBuffers = new[] { new VertexBufferBinding(vertexBuffer, VertexPositionNormalTexture.Layout, vertexBuffer.ElementCount) },
                };
            }
        }
    }
}
