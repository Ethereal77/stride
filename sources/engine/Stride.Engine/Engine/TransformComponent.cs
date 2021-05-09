// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.ComponentModel;

using Stride.Core;
using Stride.Core.Collections;
using Stride.Core.Mathematics;
using Stride.Core.Serialization;
using Stride.Engine.Design;
using Stride.Engine.Processors;

namespace Stride.Engine
{
    /// <summary>
    ///   Represents a component of an <see cref="Entity"/> that defines its position, rotation and scale.
    /// </summary>
    [DataContract("TransformComponent")]
    [DataSerializerGlobal(null, typeof(FastCollection<TransformComponent>))]
    [DefaultEntityComponentProcessor(typeof(TransformProcessor))]
    [Display("Transform", Expand = ExpandRule.Once)]
    [ComponentOrder(0)]
    public sealed class TransformComponent : EntityComponent //, IEnumerable<TransformComponent> Check why this is not working
    {
        private static readonly TransformOperation[] EmptyTransformOperations = Array.Empty<TransformOperation>();

        private TransformComponent parent;

        internal bool IsMovingInsideRootScene;

        /// <summary>
        ///   List of operations registered for custom work to be done after the world matrix has been computed,
        ///   such as updating model node hierarchy or physics for local node.
        /// </summary>
        [DataMemberIgnore]
        public FastListStruct<TransformOperation> PostOperations = new(EmptyTransformOperations);

        /// <summary>
        ///   The world transform matrix.
        /// </summary>
        /// <remarks>
        ///   The world matrix defines the transform in the global space.
        ///   <para/>
        ///   This value is automatically recomputed each frame from the local matrix and the matrices from
        ///   the transform hierarchy (parents).
        ///   <para/>
        ///   You can use <see cref="UpdateWorldMatrix"/> to force the update to happen before next frame.
        /// </remarks>
        [DataMemberIgnore]
        public Matrix WorldMatrix = Matrix.Identity;

        /// <summary>
        ///   The local transform matrix.
        /// </summary>
        /// <remarks>
        ///   This value is automatically recomputed each frame from the <see cref="Position"/>, <see cref="Rotation"/>
        ///   and <see cref="Scale"/>.
        ///   <para/>
        ///   You can use <see cref="UpdateLocalMatrix"/> to force the update to happen before next frame.
        /// </remarks>
        [DataMemberIgnore]
        public Matrix LocalMatrix = Matrix.Identity;

        /// <summary>
        ///   The position in local space, meaning the translation relative to the parent transformation.
        /// </summary>
        /// <userdoc>The translation of the entity relative to its parent.</userdoc>
        [DataMember(10)]
        public Vector3 Position;

        /// <summary>
        ///   The rotation in local space, meaning relative to the parent transformation.
        /// </summary>
        /// <userdoc>The rotation of the entity relative to its parent.</userdoc>
        [DataMember(20)]
        public Quaternion Rotation;

        /// <summary>
        ///   The scaling in local space, meaning relative to the parent transformation.
        /// </summary>
        /// <userdoc>The scale of the entity relative to its parent.</userdoc>
        [DataMember(30)]
        public Vector3 Scale;

        /// <summary>
        ///   A link representing how to compute the parent transform.
        /// </summary>
        /// <remarks>
        ///   If <c>null</c>, the transform hierarchy will be taken into account for computing the world matrix, starting
        ///   with the <see cref="Parent"/> Entity.
        ///   <para/>
        ///   If not <c>null</c>, the <see cref="Engine.TransformLink"/> will take that responsibility.
        /// </remarks>
        [DataMemberIgnore]
        public TransformLink TransformLink;


        /// <summary>
        ///   Initializes a new instance of the <see cref="TransformComponent" /> class.
        /// </summary>
        public TransformComponent()
        {
            Children = new TransformChildrenCollection(this);

            UseTRS = true;
            Scale = Vector3.One;
            Rotation = Quaternion.Identity;
        }


        // Use Translate+Rotate+Scale:
        //   false --> Transformation should be computed in TransformProcessor (no dependencies).
        //   true  --> Transformation is computed later by another system.
        //
        //   This is useful for scenarios such as binding a node to a bone, where it first need to run TransformProcessor for
        //   the hierarchy, run MeshProcessor to update ModelViewHierarchy, copy Node/Bone transformation to another Entity with
        //   special root and then update its children transformations.

        /// <summary>
        ///   Gets or sets a value indicating whether to compute the transform matrix using the
        ///   <see cref="Position"/> / <see cref="Rotation"/> / <see cref="Scale"/>.
        /// </summary>
        /// <value><c>true</c> to use TRS when computing the transform matrix; otherwise, <c>false</c>.</value>
        [DataMemberIgnore]
        [Display(Browsable = false)]
        [DefaultValue(true)]
        public bool UseTRS { get; set; } = true;

        /// <summary>
        ///   Gets the children of this <see cref="TransformComponent"/>.
        /// </summary>
        public FastCollection<TransformComponent> Children { get; }

        /// <summary>
        ///   Gets or sets the Euler rotation, with XYZ order.
        /// </summary>
        /// <value>
        ///   The Euler rotation around each of the XYZ axes.
        /// </value>
        /// <remarks>
        ///   Note that this is not stable. Setting the value and getting it again might return a different value
        ///   as it is internally encoded as a <see cref="Quaternion"/> in <see cref="Rotation"/>.
        /// </remarks>
        [DataMemberIgnore]
        public Vector3 RotationEulerXYZ
        {
            // Unfortunately it is not possible to factorize the following code with Quaternion.RotationYawPitchRoll because Z axis direction is inversed
            get
            {
                var rotation = Rotation;
                Vector3 rotationEuler;

                // Equivalent to:
                //  Matrix.Rotation(ref cachedRotation, out Matrix rotationMatrix);
                //  rotationMatrix.DecomposeXYZ(out rotationEuler);

                float xx = rotation.X * rotation.X;
                float yy = rotation.Y * rotation.Y;
                float zz = rotation.Z * rotation.Z;
                float xy = rotation.X * rotation.Y;
                float zw = rotation.Z * rotation.W;
                float zx = rotation.Z * rotation.X;
                float yw = rotation.Y * rotation.W;
                float yz = rotation.Y * rotation.Z;
                float xw = rotation.X * rotation.W;

                rotationEuler.Y = (float) Math.Asin(2.0f * (yw - zx));
                double test = Math.Cos(rotationEuler.Y);
                if (test > 1e-6f)
                {
                    rotationEuler.Z = (float) Math.Atan2(2.0f * (xy + zw), 1.0f - (2.0f * (yy + zz)));
                    rotationEuler.X = (float) Math.Atan2(2.0f * (yz + xw), 1.0f - (2.0f * (yy + xx)));
                }
                else
                {
                    rotationEuler.Z = (float) Math.Atan2(2.0f * (zw - xy), 2.0f * (zx + yw));
                    rotationEuler.X = 0.0f;
                }
                return rotationEuler;
            }
            set
            {
                // Equilvalent to:
                //  Quaternion quatX, quatY, quatZ;
                //
                //  Quaternion.RotationX(value.X, out quatX);
                //  Quaternion.RotationY(value.Y, out quatY);
                //  Quaternion.RotationZ(value.Z, out quatZ);
                //
                //  rotation = quatX * quatY * quatZ;

                var halfAngles = value * 0.5f;

                var sinX = (float) Math.Sin(halfAngles.X);
                var cosX = (float) Math.Cos(halfAngles.X);
                var sinY = (float) Math.Sin(halfAngles.Y);
                var cosY = (float) Math.Cos(halfAngles.Y);
                var sinZ = (float) Math.Sin(halfAngles.Z);
                var cosZ = (float) Math.Cos(halfAngles.Z);

                var cosXY = cosX * cosY;
                var sinXY = sinX * sinY;

                Rotation.X = sinX * cosY * cosZ - sinZ * sinY * cosX;
                Rotation.Y = sinY * cosX * cosZ + sinZ * sinX * cosY;
                Rotation.Z = sinZ * cosXY - sinXY * cosZ;
                Rotation.W = cosZ * cosXY + sinXY * sinZ;
            }
        }

        /// <summary>
        ///   Gets or sets the parent of this <see cref="TransformComponent"/>.
        /// </summary>
        /// <value>
        ///   The parent of this <see cref="TransformComponent"/> in the transform hierarchy.
        /// </value>
        [DataMemberIgnore]
        public TransformComponent Parent
        {
            get => parent;

            set
            {
                var oldParent = Parent;
                if (oldParent == value)
                    return;

                // SceneValue must be null if we have a parent
                if (Entity.SceneValue is not null)
                    Entity.Scene = null;

                var previousScene = oldParent?.Entity?.Scene;
                var newScene = value?.Entity?.Scene;

                // Get to root Scene
                while (previousScene?.Parent is not null)
                    previousScene = previousScene.Parent;
                while (newScene?.Parent is not null)
                    newScene = newScene.Parent;

                // Check if root Scene didn't change
                bool isMoving = (newScene is not null && newScene == previousScene);
                if (isMoving)
                    IsMovingInsideRootScene = true;

                // Add / Remove
                oldParent?.Children.Remove(this);
                value?.Children.Add(this);

                if (isMoving)
                    IsMovingInsideRootScene = false;
            }
        }


        /// <summary>
        ///   Updates the local matrix.
        /// </summary>
        /// <remarks>
        ///   If <see cref="UseTRS"/> is <c>trie</c>, <see cref="LocalMatrix"/> will be updated from <see cref="Position"/>,
        ///   <see cref="Rotation"/> and <see cref="Scale"/>.
        /// </remarks>
        public void UpdateLocalMatrix()
        {
            if (UseTRS)
            {
                Matrix.Transformation(ref Scale, ref Rotation, ref Position, out LocalMatrix);
            }
        }

        /// <summary>
        ///   Updates the local matrix based on the world matrix and the parent Entity's or containing Scene's world matrix.
        /// </summary>
        public void UpdateLocalFromWorld()
        {
            if (Parent is null)
            {
                var scene = Entity?.Scene;
                if (scene is not null)
                {
                    Matrix.Invert(ref scene.WorldMatrix, out var inverseSceneTransform);
                    Matrix.Multiply(ref WorldMatrix, ref inverseSceneTransform, out LocalMatrix);
                }
                else
                {
                    LocalMatrix = WorldMatrix;
                }
            }
            else
            {
                // We are not root so we need to derive the local matrix as well
                Matrix.Invert(ref Parent.WorldMatrix, out var inverseParent);
                Matrix.Multiply(ref WorldMatrix, ref inverseParent, out LocalMatrix);
            }
        }

        /// <summary>
        ///   Updates the world matrix.
        /// </summary>
        /// <remarks>
        ///   This will first call <see cref="UpdateLocalMatrix"/> on itself, and <see cref="UpdateWorldMatrix"/> on
        ///   its <see cref="Parent"/> if not <c>null</c>.
        ///   Then <see cref="WorldMatrix"/> will be updated by multiplying <see cref="LocalMatrix"/> and its parent's
        ///   <see cref="WorldMatrix"/> (if any).
        /// </remarks>
        public void UpdateWorldMatrix()
        {
            UpdateLocalMatrix();
            UpdateWorldMatrixInternal(recursive: true);
        }

        /// <summary>
        ///   Updates the world matrix by taking into account the Transform Link (if any), and the transform hierarchy of
        ///   Entities and Scenes.
        ///   Then it applies all the registered post-operations.
        /// </summary>
        /// <param name="recursive">A value indicating whether to update the transform recursively for the transform hierarchy.</param>
        internal void UpdateWorldMatrixInternal(bool recursive)
        {
            if (TransformLink is not null)
            {
                TransformLink.ComputeMatrix(recursive, out Matrix linkMatrix);
                Matrix.Multiply(ref LocalMatrix, ref linkMatrix, out WorldMatrix);
            }
            else if (Parent is not null)
            {
                if (recursive)
                    Parent.UpdateWorldMatrix();

                Matrix.Multiply(ref LocalMatrix, ref Parent.WorldMatrix, out WorldMatrix);
            }
            else
            {
                var scene = Entity?.Scene;
                if (scene is not null)
                {
                    if (recursive)
                        scene.UpdateWorldMatrix();

                    Matrix.Multiply(ref LocalMatrix, ref scene.WorldMatrix, out WorldMatrix);
                }
                else
                {
                    WorldMatrix = LocalMatrix;
                }
            }

            foreach (var transformOperation in PostOperations)
            {
                transformOperation.Process(this);
            }
        }


        /// <summary>
        ///   Represents a collection of the child <see cref="TransformComponent"/>s of a <see cref="TransformComponent"/>.
        /// </summary>
        [DataContract]
        public class TransformChildrenCollection : FastCollection<TransformComponent>
        {
            private readonly TransformComponent transform;

            private Entity Entity => transform.Entity;

            public TransformChildrenCollection(TransformComponent transform)
            {
                this.transform = transform;
            }

            //
            // Called when a new TransformComponent is added to the collection.
            //
            private void OnTransformAdded(TransformComponent item)
            {
                if (item.Parent is not null)
                    throw new InvalidOperationException("This TransformComponent already has a Parent. Detach it first.");

                item.parent = transform;

                Entity?.EntityManager?.OnHierarchyChanged(item.Entity);
                Entity?.EntityManager?.GetProcessor<TransformProcessor>().NotifyChildrenCollectionChanged(item, true);
            }

            //
            // Called when a new TransformComponent is removed from the collection.
            //
            private void OnTransformRemoved(TransformComponent item)
            {
                if (item.Parent != transform)
                    throw new InvalidOperationException("This TransformComponent's Parent is not the expected value.");

                item.parent = null;

                Entity?.EntityManager?.OnHierarchyChanged(item.Entity);
                Entity?.EntityManager?.GetProcessor<TransformProcessor>().NotifyChildrenCollectionChanged(item, false);
            }

            /// <inheritdoc/>
            protected override void InsertItem(int index, TransformComponent item)
            {
                base.InsertItem(index, item);

                OnTransformAdded(item);
            }

            /// <inheritdoc/>
            protected override void RemoveItem(int index)
            {
                OnTransformRemoved(this[index]);

                base.RemoveItem(index);
            }

            /// <inheritdoc/>
            protected override void ClearItems()
            {
                for (var i = Count - 1; i >= 0; --i)
                    OnTransformRemoved(this[i]);

                base.ClearItems();
            }

            /// <inheritdoc/>
            protected override void SetItem(int index, TransformComponent item)
            {
                OnTransformRemoved(this[index]);

                base.SetItem(index, item);

                OnTransformAdded(this[index]);
            }
        }
    }
}
