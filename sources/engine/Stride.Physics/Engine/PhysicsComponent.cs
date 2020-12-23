// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Collections;
using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Core.MicroThreading;
using Stride.Engine.Design;
using Stride.Physics;
using Stride.Physics.Engine;
using Stride.Rendering;

namespace Stride.Engine
{
    public enum CollisionState
    {
        Ignore,
        Detect
    }

    [DataContract("PhysicsComponent", Inherited = true)]
    [Display("Physics", Expand = ExpandRule.Once)]
    [DefaultEntityComponentProcessor(typeof(PhysicsProcessor))]
    [AllowMultipleComponents]
    [ComponentOrder(3000)]
    [ComponentCategory("Physics")]
    public abstract class PhysicsComponent : ActivableEntityComponent
    {
        protected static Logger logger = GlobalLogger.GetLogger("PhysicsComponent");

        protected PhysicsComponent()
        {
            CanScaleShape = true;

            ColliderShapes = new ColliderShapeCollection(this);

            NewPairChannel = new Channel<Collision> { Preference = ChannelPreference.PreferSender };
            PairEndedChannel = new Channel<Collision> { Preference = ChannelPreference.PreferSender };
        }

        [DataMemberIgnore]
        internal BulletSharp.CollisionObject NativeCollisionObject;

        /// <userdoc>
        ///   Gets the collider shapes that compose this element.
        /// </userdoc>
        [DataMember(200)]
        [Category]
        [MemberCollection(NotNullItems = true)]
        public ColliderShapeCollection ColliderShapes { get; }

        /// <summary>
        ///   Gets or sets the collision group.
        /// </summary>
        /// <value>
        ///   The collision group the Entity belongs to. The default value is <see cref="CollisionFilterGroups.DefaultFilter"/>.
        /// </value>
        /// <userdoc>
        ///   Which collision group the component belongs to. This can't be changed at runtime. The default is DefaultFilter.
        /// </userdoc>
        /// <remarks>
        ///   The collider will still produce events, to allow non trigger rigidbodies or static colliders to act as a trigger
        ///   if required for certain filtering groups.
        /// </remarks>
        [DataMember(30)]
        [Display("Collision group")]
        [DefaultValue(CollisionFilterGroups.DefaultFilter)]
        public CollisionFilterGroups CollisionGroup { get; set; } = CollisionFilterGroups.DefaultFilter;

        /// <summary>
        ///   Gets or sets which collider groups this component collides with.
        /// </summary>
        /// <value>
        ///   The collider groups this component collides with. The default value is <see cref="CollisionFilterGroupFlags.AllFilter"/>.
        /// </value>
        /// <userdoc>
        ///   Which collider groups this component collides with. With nothing selected, it collides with all groups. This can't be changed at runtime.
        /// </userdoc>
        /// <remarks>
        ///   The collider will still produce events, to allow non trigger rigidbodies or static colliders to act as
        ///   a trigger if required for certain filtering groups.
        /// </remarks>
        [DataMember(40)]
        [Display("Collides with...")]
        [DefaultValue(CollisionFilterGroupFlags.AllFilter)]
        public CollisionFilterGroupFlags CanCollideWith { get; set; } = CollisionFilterGroupFlags.AllFilter;

        /// <summary>
        ///   Gets or sets a value indicating if this element will store collisions to allow scripts to access them
        ///   in case of collision events.
        /// </summary>
        /// <value>
        ///   Value indicating if this element will store collisions.
        /// </value>
        /// <userdoc>
        ///   You can use collision events in scripts. If you have no scripts using collision events for this component,
        ///   disable this option to save CPU. It has no effect on physics.
        /// </userdoc>
        [Display("Record collision events")]
        [DataMemberIgnore]
        public bool ProcessCollisions { get; set; } = false;

        /// <summary>
        ///   Gets or sets a value indicating if this element is enabled in the physics engine.
        /// </summary>
        /// <value>
        ///   Value indicating if this element is enabled in the physics engine.
        /// </value>
        /// <userdoc>
        ///   If this element is enabled in the physics engine.
        /// </userdoc>
        [DataMember(-10)]
        [DefaultValue(true)]
        public override bool Enabled
        {
            get => base.Enabled;

            set
            {
                base.Enabled = value;

                if (NativeCollisionObject is null)
                    return;

                if (value)
                {
                    // Enabled: Allow collisions
                    if (NativeCollisionObject.CollisionFlags.HasFlag(BulletSharp.CollisionFlags.NoContactResponse))
                    {
                        NativeCollisionObject.CollisionFlags ^= BulletSharp.CollisionFlags.NoContactResponse;
                    }

                    // Enabled: Allow simulation
                    NativeCollisionObject.ForceActivationState(canSleep ?
                        BulletSharp.ActivationState.ActiveTag :
                        BulletSharp.ActivationState.DisableDeactivation);
                }
                else
                {
                    // Disabled: Prevent collisions
                    NativeCollisionObject.CollisionFlags |= BulletSharp.CollisionFlags.NoContactResponse;

                    // Disabled: Prevent simulation
                    NativeCollisionObject.ForceActivationState(BulletSharp.ActivationState.DisableSimulation);
                }

                DebugEntity?.EnableAll(enabled: value, applyOnChildren: true);
            }
        }

        private bool canSleep;

        /// <summary>
        ///   Gets or sets a value indicating if this element can enter sleep state.
        /// </summary>
        /// <value>
        ///   Value indicating if this element can enter sleep state.
        /// </value>
        /// <userdoc>
        ///   Don't process this physics component when it's not moving. This saves CPU.
        /// </userdoc>
        [DataMember(55)]
        [Display("Can sleep")]
        public bool CanSleep
        {
            get => canSleep;

            set
            {
                canSleep = value;

                if (NativeCollisionObject is null)
                    return;

                if (Enabled)
                {
                    NativeCollisionObject.ActivationState = value ?
                        BulletSharp.ActivationState.ActiveTag :
                        BulletSharp.ActivationState.DisableDeactivation;
                }
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is active (awake).
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive => NativeCollisionObject?.IsActive ?? false;

        /// <summary>
        ///   Attempts to awake the collider.
        /// </summary>
        /// <param name="forceActivation">Value indicating whether to forcefully activate the collider.</param>
        public void Activate(bool forceActivation = false)
        {
            NativeCollisionObject?.Activate(forceActivation);
        }

        private float restitution;

        /// <summary>
        ///   Gets or sets the restitution of this element, that is, the amount of kinetic energy lost or gained
        ///   after a collision.
        /// </summary>
        /// <value>
        ///   The restitution of this element. A typical value is between 0 and 1.
        /// </value>
        /// <userdoc>
        ///   The amount of kinetic energy lost or gained after a collision. If the restitution of colliding
        ///   entities is 0, the entities lose all energy and stop moving immediately on impact. If the restitution
        ///   is 1, they lose no energy and rebound with the same velocity they collided at. Use this to change the
        ///   component "bounciness". A typical value is between 0 and 1.
        /// </userdoc>
        [DataMember(60)]
        public float Restitution
        {
            get => restitution;

            set
            {
                restitution = value;

                if (NativeCollisionObject != null)
                {
                    NativeCollisionObject.Restitution = restitution;
                }
            }
        }

        private float friction = 0.5f;

        /// <summary>
        ///   Gets or sets the friction of this element
        /// </summary>
        /// <value>
        ///   The friction of this element.
        /// </value>
        /// <userdoc>
        ///   The friction.
        /// </userdoc>
        /// <remarks>
        ///   It's important to realise that friction and restitution are not values of any particular surface, but
        ///   rather a value of the interaction of two surfaces.
        ///   So why is it defined for each object? In order to determine the overall friction and restitution
        ///   between any two surfaces in a collision.
        /// </remarks>
        [DataMember(65)]
        public float Friction
        {
            get => friction;

            set
            {
                friction = value;

                if (NativeCollisionObject != null)
                {
                    NativeCollisionObject.Friction = friction;
                }
            }
        }

        private float rollingFriction;

        /// <summary>
        ///   Gets or sets the rolling friction of this element.
        /// </summary>
        /// <value>
        ///   The rolling friction of this element.
        /// </value>
        /// <userdoc>
        ///   The rolling friction.
        /// </userdoc>
        [DataMember(66)]
        public float RollingFriction
        {
            get => rollingFriction;

            set
            {
                rollingFriction = value;

                if (NativeCollisionObject != null)
                {
                    NativeCollisionObject.RollingFriction = rollingFriction;
                }
            }
        }

        private float ccdMotionThreshold;

        [DataMember(67)]
        public float CcdMotionThreshold
        {
            get => ccdMotionThreshold;

            set
            {
                ccdMotionThreshold = value;

                if (NativeCollisionObject != null)
                {
                    NativeCollisionObject.CcdMotionThreshold = ccdMotionThreshold;
                }
            }
        }

        private float ccdSweptSphereRadius;

        [DataMember(68)]
        public float CcdSweptSphereRadius
        {
            get => ccdSweptSphereRadius;

            set
            {
                ccdSweptSphereRadius = value;

                if (NativeCollisionObject != null)
                {
                    NativeCollisionObject.CcdSweptSphereRadius = ccdSweptSphereRadius;
                }
            }
        }


        private Dictionary<PhysicsComponent, CollisionState> ignoreCollisionBuffer;

        #region Ignore or Private/Internal

        [DataMemberIgnore]
        public TrackingHashSet<Collision> Collisions { get; } = new TrackingHashSet<Collision>();

        [DataMemberIgnore]
        internal Channel<Collision> NewPairChannel;

        public ChannelMicroThreadAwaiter<Collision> NewCollision()
        {
            return NewPairChannel.Receive();
        }

        [DataMemberIgnore]
        internal Channel<Collision> PairEndedChannel;

        public ChannelMicroThreadAwaiter<Collision> CollisionEnded()
        {
            return PairEndedChannel.Receive();
        }

        [DataMemberIgnore]
        public Simulation Simulation { get; internal set; }

        [DataMemberIgnore]
        internal PhysicsShapesRenderingService DebugShapeRendering;

        [DataMemberIgnore]
        public bool ColliderShapeChanged { get; private set; }

        [DataMemberIgnore]
        protected ColliderShape colliderShape;

        [DataMemberIgnore]
        public virtual ColliderShape ColliderShape
        {
            get => colliderShape;

            set
            {
                colliderShape = value;

                if (value is null)
                    return;

                if (NativeCollisionObject != null)
                    NativeCollisionObject.CollisionShape = value.InternalShape;
            }
        }

        [DataMemberIgnore]
        public bool CanScaleShape { get; set; }

        [DataMemberIgnore]
        public Matrix PhysicsWorldTransform
        {
            get => NativeCollisionObject.WorldTransform;
            set => NativeCollisionObject.WorldTransform = value;
        }

        /// <summary>
        ///   Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        [DataMemberIgnore]
        public string Tag { get; set; }

        [DataMemberIgnore]
        public Matrix BoneWorldMatrix;

        [DataMemberIgnore]
        public Matrix BoneWorldMatrixOut;

        [DataMemberIgnore]
        public int BoneIndex = -1;

        [DataMemberIgnore]
        protected PhysicsProcessor.AssociatedData Data { get; set; }

        [DataMemberIgnore]
        public Entity DebugEntity { get; set; }

        public void AddDebugEntity(Scene scene, RenderGroup renderGroup = RenderGroup.Group0, bool alwaysAddOffset = false)
        {
            if (DebugEntity != null)
                return;

            var entity = Data?.PhysicsComponent?.DebugShapeRendering?.CreateDebugEntity(this, renderGroup, alwaysAddOffset);
            DebugEntity = entity;

            if (DebugEntity is null)
                return;

            scene.Entities.Add(entity);
        }

        public void RemoveDebugEntity(Scene scene)
        {
            if (DebugEntity is null)
                return;

            scene.Entities.Remove(DebugEntity);
            DebugEntity = null;
        }

        #endregion Ignore or Private/Internal

        #region Utility

        /// <summary>
        ///   Computes the physics transformation from the TransformComponent values.
        /// </summary>
        internal void DerivePhysicsTransformation(out Matrix derivedTransformation)
        {
            Entity.Transform.WorldMatrix.Decompose(out Vector3 scale, out Matrix rotation, out Vector3 translation);

            var translationMatrix = Matrix.Translation(translation);
            Matrix.Multiply(ref rotation, ref translationMatrix, out derivedTransformation);

            // Handle dynamic scaling if allowed (aka not using assets)
            if (CanScaleShape)
            {
                if (ColliderShape.Scaling != scale)
                    ColliderShape.Scaling = scale;
            }

            // Handle collider shape offset
            if (ColliderShape.LocalOffset != Vector3.Zero ||
                ColliderShape.LocalRotation != Quaternion.Identity)
            {
                derivedTransformation = Matrix.Multiply(ColliderShape.PositiveCenterMatrix, derivedTransformation);
            }

            if (DebugEntity is null)
                return;

            derivedTransformation.Decompose(out _, out rotation, out translation);
            DebugEntity.Transform.Position = translation;
            DebugEntity.Transform.Rotation = Quaternion.RotationMatrix(rotation);
        }

        /// <summary>
        ///   Computes the physics transformation from the bone matrices.
        /// </summary>
        /// <returns></returns>
        internal void DeriveBonePhysicsTransformation(out Matrix derivedTransformation)
        {
            BoneWorldMatrix.Decompose(out Vector3 scale, out Matrix rotation, out Vector3 translation);

            var translationMatrix = Matrix.Translation(translation);
            Matrix.Multiply(ref rotation, ref translationMatrix, out derivedTransformation);

            // Handle dynamic scaling if allowed (aka not using assets)
            if (CanScaleShape)
            {
                if (ColliderShape.Scaling != scale)
                    ColliderShape.Scaling = scale;
            }

            // Handle collider shape offset
            if (ColliderShape.LocalOffset != Vector3.Zero ||
                ColliderShape.LocalRotation != Quaternion.Identity)
            {
                derivedTransformation = Matrix.Multiply(ColliderShape.PositiveCenterMatrix, derivedTransformation);
            }

            if (DebugEntity is null)
                return;

            derivedTransformation.Decompose(out _, out rotation, out translation);
            DebugEntity.Transform.Position = translation;
            DebugEntity.Transform.Rotation = Quaternion.RotationMatrix(rotation);
        }

        /// <summary>
        ///   Updates the graphics transformation from the given physics transformation.
        /// </summary>
        /// <param name="physicsTransform">By reference. Physics transformation matrix.</param>
        internal void UpdateTransformationComponent(ref Matrix physicsTransform)
        {
            var entity = Entity;

            if (ColliderShape.LocalOffset != Vector3.Zero ||
                ColliderShape.LocalRotation != Quaternion.Identity)
            {
                physicsTransform = Matrix.Multiply(ColliderShape.NegativeCenterMatrix, physicsTransform);
            }

            // We need to extract scale only..
            entity.Transform.WorldMatrix.Decompose(out Vector3 scale, out Matrix _, out _);

            var scaling = Matrix.Scaling(scale);
            Matrix.Multiply(ref scaling, ref physicsTransform, out entity.Transform.WorldMatrix);

            entity.Transform.UpdateLocalFromWorld();

            entity.Transform.LocalMatrix.Decompose(out _, out Quaternion rotQuat, out Vector3 translation);
            entity.Transform.Position = translation;
            entity.Transform.Rotation = rotQuat;

            if (DebugEntity is null)
                return;

            if (ColliderShape.LocalOffset != Vector3.Zero ||
                ColliderShape.LocalRotation != Quaternion.Identity)
            {
                physicsTransform = Matrix.Multiply(ColliderShape.PositiveCenterMatrix, physicsTransform);
            }

            physicsTransform.Decompose(out _, out Matrix rotation, out translation);
            DebugEntity.Transform.Position = translation;
            DebugEntity.Transform.Rotation = Quaternion.RotationMatrix(rotation);
        }

        /// <summary>
        ///   Updates the graphics transformation from the given physics transformation.
        /// </summary>
        /// <param name="physicsTransform"></param>
        internal void UpdateBoneTransformation(ref Matrix physicsTransform)
        {
            if (ColliderShape.LocalOffset != Vector3.Zero ||
                ColliderShape.LocalRotation != Quaternion.Identity)
            {
                physicsTransform = Matrix.Multiply(ColliderShape.NegativeCenterMatrix, physicsTransform);
            }

            //we need to extract scale only..
            BoneWorldMatrix.Decompose(out Vector3 scale, out Matrix _, out _);

            var scaling = Matrix.Scaling(scale);
            Matrix.Multiply(ref scaling, ref physicsTransform, out BoneWorldMatrixOut);

            // TODO: Propagate to other bones? need to review this.

            if (DebugEntity is null)
                return;

            if (ColliderShape.LocalOffset != Vector3.Zero ||
                ColliderShape.LocalRotation != Quaternion.Identity)
            {
                physicsTransform = Matrix.Multiply(ColliderShape.PositiveCenterMatrix, physicsTransform);
            }

            physicsTransform.Decompose(out _, out Matrix rotation, out Vector3 translation);
            DebugEntity.Transform.Position = translation;
            DebugEntity.Transform.Rotation = Quaternion.RotationMatrix(rotation);
        }

        /// <summary>
        ///   Forces an update from the TransformComponent to the Collider.PhysicsWorldTransform.
        ///   Useful to manually force movements.
        ///   In the case of dynamic rigidbodies a velocity reset should be applied first.
        /// </summary>
        public void UpdatePhysicsTransformation()
        {
            Matrix transform;
            if (BoneIndex == -1)
                DerivePhysicsTransformation(out transform);
            else
                DeriveBonePhysicsTransformation(out transform);

            // Finally copy back to bullet
            PhysicsWorldTransform = transform;
        }

        public void ComposeShape()
        {
            ColliderShapeChanged = false;

            if (ColliderShape != null)
            {
                if (!ColliderShape.IsPartOfAsset)
                {
                    ColliderShape.Dispose();
                    ColliderShape = null;
                }
                else
                {
                    ColliderShape = null;
                }
            }

            CanScaleShape = true;

            // Single shape case
            if (ColliderShapes.Count == 1)
            {
                if (ColliderShapes[0] is null)
                    return;

                if (ColliderShapes[0] is ColliderShapeAssetDesc)
                {
                    CanScaleShape = false;
                }

                ColliderShape = PhysicsColliderShape.CreateShape(ColliderShapes[0]);
            }
            // Need a compound shape in this case
            else if (ColliderShapes.Count > 1)
            {
                var compound = new CompoundColliderShape();
                foreach (var desc in ColliderShapes)
                {
                    if (desc is null)
                        continue;
                    if (desc is ColliderShapeAssetDesc)
                    {
                        CanScaleShape = false;
                    }

                    var subShape = PhysicsColliderShape.CreateShape(desc);
                    if (subShape != null)
                        compound.AddChildShape(subShape);
                }

                ColliderShape = compound;
            }

            if (ColliderShape != null)
            {
                // Force update internal shape and gizmo scaling
                ColliderShape.Scaling = ColliderShape.Scaling;
            }
        }

        #endregion Utility

        internal void Attach(PhysicsProcessor.AssociatedData data)
        {
            Data = data;

            // This is mostly required for the Game Studio gizmos
            if (Simulation.DisableSimulation)
                return;

            // Not optimal as UpdateWorldMatrix will end up being called twice this frame.. but we need to ensure that we have valid data
            Entity.Transform.UpdateWorldMatrix();

            if (ColliderShapes.Count == 0 && ColliderShape is null)
            {
                // No shape, no purpose
                logger.Error($"Entity {Entity.Name} has a PhysicsComponent without any collider shape.");
                return;
            }
            else if (ColliderShape is null)
            {
                ComposeShape();
                if (ColliderShape is null)
                {
                    // No shape, no purpose
                    logger.Error($"Entity {Entity.Name}'s PhysicsComponent failed to compose its collider shape.");
                    return;
                }
            }

            BoneIndex = -1;

            OnAttach();

            if(ignoreCollisionBuffer != null && NativeCollisionObject != null)
            {
                foreach(var kvp in ignoreCollisionBuffer)
                {
                    IgnoreCollisionWith(kvp.Key, kvp.Value);
                }
                ignoreCollisionBuffer = null;
            }
        }

        internal void Detach()
        {
            Data = null;

            // This is mostly required for the Game Studio gizmos
            if (Simulation.DisableSimulation)
                return;

            // Actually call the detach
            OnDetach();

            if (ColliderShape != null && !ColliderShape.IsPartOfAsset)
            {
                ColliderShape.Dispose();
                ColliderShape = null;
            }
        }

        protected virtual void OnAttach()
        {
            // Set pre-set post deserialization properties
            Enabled = base.Enabled;
            CanSleep = canSleep;
            Restitution = restitution;
            Friction = friction;
            RollingFriction = rollingFriction;
            CcdMotionThreshold = ccdMotionThreshold;
            CcdSweptSphereRadius = ccdSweptSphereRadius;
        }

        protected virtual void OnDetach()
        {
            if (NativeCollisionObject is null)
                return;

            NativeCollisionObject.UserObject = null;
            NativeCollisionObject.Dispose();
            NativeCollisionObject = null;
        }

        internal void UpdateBones()
        {
            if (!Enabled)
                return;

            OnUpdateBones();
        }

        internal void UpdateDraw()
        {
            if (!Enabled)
                return;

            OnUpdateDraw();
        }

        protected internal virtual void OnUpdateDraw() { }

        protected virtual void OnUpdateBones()
        {
            // Read from ModelViewHierarchy
            var model = Data.ModelComponent;
            BoneWorldMatrix = model.Skeleton.NodeTransformations[BoneIndex].WorldMatrix;
        }

        public void IgnoreCollisionWith(PhysicsComponent other, CollisionState state)
        {
            var otherNative = other.NativeCollisionObject;
            if(NativeCollisionObject is null || other.NativeCollisionObject is null)
            {
                if(ignoreCollisionBuffer != null || other.ignoreCollisionBuffer is null)
                {
                    if(ignoreCollisionBuffer is null)
                        ignoreCollisionBuffer = new Dictionary<PhysicsComponent, CollisionState>();
                    if(ignoreCollisionBuffer.ContainsKey(other))
                        ignoreCollisionBuffer[other] = state;
                    else
                        ignoreCollisionBuffer.Add(other, state);
                }
                else
                {
                    other.IgnoreCollisionWith(this, state);
                }
                return;
            }

            switch(state)
            {
                // Note that we're calling 'SetIgnoreCollisionCheck' on both objects as bullet doesn't
                // do it itself ; One of the object in the pair will report that it doesn't ignore
                // collision with the other even though you set the other as ignoring the former.
                case CollisionState.Ignore:
                    // Bullet uses an array per collision object to store all of the objects to ignore,
                    // when calling this method it adds the referenced object without checking for duplicates,
                    // so if a user where to call 'Ignore' of this function on this object n-times he'll have to call it
                    // that same amount of time to re-detect them instead of just once.
                    // We're calling false here to remove a previous ignore if there was any and re-ignoring
                    // to force it to have only a single instance.
                    otherNative.SetIgnoreCollisionCheck(NativeCollisionObject, false);
                    NativeCollisionObject.SetIgnoreCollisionCheck(otherNative, false);
                    otherNative.SetIgnoreCollisionCheck(NativeCollisionObject, true);
                    NativeCollisionObject.SetIgnoreCollisionCheck(otherNative, true);
                    break;

                case CollisionState.Detect:
                    otherNative.SetIgnoreCollisionCheck(NativeCollisionObject, false);
                    NativeCollisionObject.SetIgnoreCollisionCheck(otherNative, false);
                    break;
            }
        }

        public bool IsIgnoringCollisionWith(PhysicsComponent other)
        {
            if(ignoreCollisionBuffer != null)
            {
                return ignoreCollisionBuffer.TryGetValue(other, out var state) && state == CollisionState.Ignore;
            }
            else if(other.ignoreCollisionBuffer != null)
            {
                return other.IsIgnoringCollisionWith(this);
            }
            else if(other.NativeCollisionObject is null || NativeCollisionObject is null)
            {
                return false;
            }
            else
            {
                return !NativeCollisionObject.CheckCollideWith(other.NativeCollisionObject);
            }
        }

        [DataContract]
        public class ColliderShapeCollection : FastCollection<IInlineColliderShapeDesc>
        {
            private readonly PhysicsComponent component;

            public ColliderShapeCollection(PhysicsComponent componentParam)
            {
                component = componentParam;
            }

            /// <inheritdoc/>
            protected override void InsertItem(int index, IInlineColliderShapeDesc item)
            {
                base.InsertItem(index, item);
                component.ColliderShapeChanged = true;
            }

            /// <inheritdoc/>
            protected override void RemoveItem(int index)
            {
                base.RemoveItem(index);
                component.ColliderShapeChanged = true;
            }

            /// <inheritdoc/>
            protected override void ClearItems()
            {
                base.ClearItems();
                component.ColliderShapeChanged = true;
            }

            /// <inheritdoc/>
            protected override void SetItem(int index, IInlineColliderShapeDesc item)
            {
                base.SetItem(index, item);
                component.ColliderShapeChanged = true;
            }
        }
    }
}
