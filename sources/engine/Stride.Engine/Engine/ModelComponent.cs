// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Collections;
using Stride.Core.Mathematics;
using Stride.Engine.Design;
using Stride.Engine.Processors;
using Stride.Rendering;
using Stride.Updater;

namespace Stride.Engine
{
    /// <summary>
    ///   Represents an <see cref="EntityComponent"/> to add a <see cref="Rendering.Model"/> to an <see cref="Entity"/>, that will be used during rendering.
    /// </summary>
    [DataContract("ModelComponent")]
    [Display("Model", Expand = ExpandRule.Once)]
    [DefaultEntityComponentProcessor(typeof(ModelTransformProcessor))]
    [DefaultEntityComponentRenderer(typeof(ModelRenderProcessor))]
    [ComponentOrder(11000)]
    [ComponentCategory("Model")]
    // TODO: GRAPHICS REFACTOR
    public sealed class ModelComponent : ActivableEntityComponent, IModelInstance
    {
        private readonly List<MeshInfo> meshInfos = new List<MeshInfo>();
        private Model model;
        private SkeletonUpdater skeleton;
        private bool modelViewHierarchyDirty = true;

        /// <summary>
        ///   Represents the per-entity state of each individual mesh of a model.
        /// </summary>
        public class MeshInfo
        {
            /// <summary>
            ///   The current blend matrices of the skinned meshes, transforming from mesh space to world space, for each bone.
            /// </summary>
            public Matrix[] BlendMatrices;

            /// <summary>
            ///   The mesh's current bounding box in world space.
            /// </summary>
            public BoundingBox BoundingBox;
            /// <summary>
            ///   The mesh's current bounding sphere in world space.
            /// </summary>
            public BoundingSphere BoundingSphere;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ModelComponent"/> class.
        /// </summary>
        public ModelComponent() : this(null) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ModelComponent"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ModelComponent(Model model)
        {
            Model = model;
            IsShadowCaster = true;
        }

        /// <summary>
        ///   Gets or sets the model associated to the <see cref="Entity"/>.
        /// </summary>
        /// <value>The model.</value>
        /// <userdoc>The reference to the model asset to attach to this entity.</userdoc>
        [DataMemberCustomSerializer]
        [DataMember(10)]
        public Model Model
        {
            get => model;

            set
            {
                if (model != value)
                    modelViewHierarchyDirty = true;
                model = value;
            }
        }

        /// <summary>
        ///   Gets the list of materials to use for the <see cref="Rendering.Model"/>.
        /// </summary>
        /// <value>
        ///   The list of materials. The non-<c>null</c> ones will override the corresponding materials from
        ///   <see cref="Model.Materials"/> in the same slots.
        /// </value>
        /// <userdoc>The list of materials to use with the model. This list overrides the default materials of the model.</userdoc>
        [DataMember(40)]
        [Category]
        [MemberCollection(ReadOnly = true)]
        public IndexingDictionary<Material> Materials { get; } = new IndexingDictionary<Material>();

        [DataMemberIgnore, DataMemberUpdatable]
        [DataMember]
        public SkeletonUpdater Skeleton
        {
            get
            {
                CheckSkeleton();
                return skeleton;
            }
        }

        /// <summary>
        ///   Gets the current per-entity state for each mesh in the associated <see cref="Rendering.Model"/>.
        /// </summary>
        [DataMemberIgnore]
        public IReadOnlyList<MeshInfo> MeshInfos => meshInfos;

        private void CheckSkeleton()
        {
            if (modelViewHierarchyDirty)
            {
                ModelUpdated();
                modelViewHierarchyDirty = false;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the <see cref="Rendering.Model"/> should cast shadows in the environment.
        /// </summary>
        /// <value>A value indicating if the model should cast shadows.</value>
        /// <userdoc>Cast a shadow (when shadow maps are enabled).</userdoc>
        [DataMember(30)]
        [DefaultValue(true)]
        [Display("Cast shadows")]
        public bool IsShadowCaster { get; set; }

        /// <summary>
        ///   Gets or sets the render group for this <see cref="Rendering.Model"/>.
        /// </summary>
        [DataMember(20)]
        [DefaultValue(RenderGroup.Group0)]
        [Display("Render group")]
        public RenderGroup RenderGroup { get; set; }

        /// <summary>
        ///   The bounding box of the <see cref="Rendering.Model"/>, in world space.
        /// </summary>
        /// <value>The bounding box.</value>
        [DataMemberIgnore]
        public BoundingBox BoundingBox;

        /// <summary>
        ///   The bounding sphere of the <see cref="Rendering.Model"/>, in world space.
        /// </summary>
        /// <value>The bounding sphere.</value>
        [DataMemberIgnore]
        public BoundingSphere BoundingSphere;

        /// <summary>
        ///   Gets the material at the specified index. If the material is not overriden by this component, it will try to get it
        ///   from <see cref="Model.Materials"/>.
        /// </summary>
        /// <param name="index">The index of the material to get.</param>
        /// <returns>
        ///   The <see cref="Material"/> at the specified <paramref name="index"/>;
        ///   or <c>null</c> if not found.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> cannot be less than 0.</exception>
        public Material GetMaterial(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than 0.");

            if (Materials.TryGetValue(index, out Material material))
                return material;

            // TODO: If Model is null, shouldn't we always return null?
            if (Model != null && index < Model.Materials.Count)
            {
                material = Model.Materials[index].Material;
            }
            return material;
        }

        /// <summary>
        ///   Gets the number of materials of the <see cref="Rendering.Model"/> (computed from <see cref="Model.Materials"/>).
        /// </summary>
        /// <returns>Number of materials of the model.</returns>
        public int GetMaterialCount() => Model?.Materials.Count ?? 0;

        private void ModelUpdated()
        {
            if (model != null)
            {
                // Create mesh-per-entity state
                meshInfos.Clear();
                foreach (var mesh in model.Meshes)
                {
                    var meshData = new MeshInfo();
                    meshInfos.Add(meshData);

                    if (mesh.Skinning != null)
                        meshData.BlendMatrices = new Matrix[mesh.Skinning.Bones.Length];
                }

                if (skeleton != null)
                {
                    // Reuse previous ModelViewHierarchy
                    skeleton.Initialize(model.Skeleton);
                }
                else
                {
                    skeleton = new SkeletonUpdater(model.Skeleton);
                }
            }
        }

        /// <summary>
        ///   Updates the skeleton, skinning and bounding box with the associated <see cref="TransformComponent"/> of a <see cref="Rendering.Model"/>.
        /// </summary>
        /// <param name="transformComponent">The transform of the model.</param>
        internal void Update(TransformComponent transformComponent)
        {
            if (!Enabled || model is null)
                return;

            ref Matrix worldMatrix = ref transformComponent.WorldMatrix;

            // Check if scaling is negative
            var up = Vector3.Cross(worldMatrix.Right, worldMatrix.Forward);
            bool isScalingNegative = Vector3.Dot(worldMatrix.Up, up) < 0.0f;

            // Make sure skeleton is up to date
            CheckSkeleton();
            if (skeleton != null)
            {
                // Update model view hierarchy node matrices
                skeleton.NodeTransformations[0].LocalMatrix = worldMatrix;
                skeleton.NodeTransformations[0].IsScalingNegative = isScalingNegative;
                skeleton.UpdateMatrices();
            }

            // Update the bounding sphere / bounding box in world space
            BoundingSphere = BoundingSphere.Empty;
            BoundingBox = BoundingBox.Empty;
            bool modelHasBoundingBox = false;

            for (int meshIndex = 0; meshIndex < Model.Meshes.Count; meshIndex++)
            {
                var mesh = Model.Meshes[meshIndex];
                var meshInfo = meshInfos[meshIndex];
                meshInfo.BoundingSphere = BoundingSphere.Empty;
                meshInfo.BoundingBox = BoundingBox.Empty;

                if (mesh.Skinning != null && skeleton != null)
                {
                    bool meshHasBoundingBox = false;
                    var bones = mesh.Skinning.Bones;

                    // For skinned meshes, bounding box is union of the bounding boxes of the unskinned mesh, transformed by each affecting bone.
                    for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
                    {
                        var nodeIndex = bones[boneIndex].NodeIndex;
                        Matrix.Multiply(ref bones[boneIndex].LinkToMeshMatrix, ref skeleton.NodeTransformations[nodeIndex].WorldMatrix, out meshInfo.BlendMatrices[boneIndex]);

                        BoundingBox.Transform(ref mesh.BoundingBox, ref meshInfo.BlendMatrices[boneIndex], out BoundingBox skinnedBoundingBox);
                        BoundingSphere.Transform(ref mesh.BoundingSphere, ref meshInfo.BlendMatrices[boneIndex], out BoundingSphere skinnedBoundingSphere);

                        if (meshHasBoundingBox)
                        {
                            BoundingBox.Merge(ref meshInfo.BoundingBox, ref skinnedBoundingBox, out meshInfo.BoundingBox);
                            BoundingSphere.Merge(ref meshInfo.BoundingSphere, ref skinnedBoundingSphere, out meshInfo.BoundingSphere);
                        }
                        else
                        {
                            meshHasBoundingBox = true;
                            meshInfo.BoundingSphere = skinnedBoundingSphere;
                            meshInfo.BoundingBox = skinnedBoundingBox;
                        }
                    }
                }
                else
                {
                    // If there is a skeleton, use the corresponding node's transform. Otherwise, fall back to the model transform.
                    var transform = skeleton != null ? skeleton.NodeTransformations[mesh.NodeIndex].WorldMatrix : worldMatrix;
                    BoundingBox.Transform(ref mesh.BoundingBox, ref transform, out meshInfo.BoundingBox);
                    BoundingSphere.Transform(ref mesh.BoundingSphere, ref transform, out meshInfo.BoundingSphere);
                }

                if (modelHasBoundingBox)
                {
                    BoundingBox.Merge(ref BoundingBox, ref meshInfo.BoundingBox, out BoundingBox);
                    BoundingSphere.Merge(ref BoundingSphere, ref meshInfo.BoundingSphere, out BoundingSphere);
                }
                else
                {
                    BoundingBox = meshInfo.BoundingBox;
                    BoundingSphere = meshInfo.BoundingSphere;
                    modelHasBoundingBox = true;
                }
            }
        }
    }
}
