// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Stride.Core.Collections;
using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Rendering;

namespace Stride.Physics
{
    public class Simulation : IDisposable
    {
        const CollisionFilterGroups DefaultGroup = (CollisionFilterGroups) BulletSharp.CollisionFilterGroups.DefaultFilter;
        const CollisionFilterGroupFlags DefaultFlags = (CollisionFilterGroupFlags) BulletSharp.CollisionFilterGroups.AllFilter;

        private readonly PhysicsProcessor processor;

        private readonly BulletSharp.DiscreteDynamicsWorld discreteDynamicsWorld;
        private readonly BulletSharp.CollisionWorld collisionWorld;

        private readonly BulletSharp.CollisionDispatcher dispatcher;
        private readonly BulletSharp.CollisionConfiguration collisionConfiguration;
        private readonly BulletSharp.DbvtBroadphase broadphase;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly BulletSharp.ContactSolverInfo solverInfo;

        private readonly BulletSharp.DispatcherInfo dispatchInfo;

        internal readonly bool CanCcd;

#if DEBUG
        private static readonly Logger Log = GlobalLogger.GetLogger(typeof(Simulation).FullName);
#endif

        public bool ContinuousCollisionDetection
        {
            get
            {
                if (!CanCcd)
                    throw new Exception("ContinuousCollisionDetection must be enabled at physics engine initialization using the proper flag.");

                return dispatchInfo.UseContinuous;
            }
            set
            {
                if (!CanCcd)
                    throw new Exception("ContinuousCollisionDetection must be enabled at physics engine initialization using the proper flag.");

                dispatchInfo.UseContinuous = value;
            }
        }

        /// <summary>
        ///   Value indicating whether to disable the physics simulation completely.
        /// </summary>
        public static bool DisableSimulation = false;

        public delegate PhysicsEngineFlags OnSimulationCreationDelegate();

        /// <summary>
        ///   Called to configure physics engine flags upon initialization.
        /// </summary>
        // NOTE: Temporary solution to inject engine flags
        public static OnSimulationCreationDelegate OnSimulationCreation;

        /// <summary>
        ///   Initializes the Physics engine using the specified flags.
        /// </summary>
        /// <param name="processor">The physics entities processor.</param>
        /// <param name="configuration">Physics engine settings.</param>
        /// <exception cref="NotImplementedException">SoftBody processing is not yet available.</exception>
        internal Simulation(PhysicsProcessor processor, PhysicsSettings configuration)
        {
            this.processor = processor;

            if (configuration.Flags == PhysicsEngineFlags.None)
            {
                configuration.Flags = OnSimulationCreation?.Invoke() ?? configuration.Flags;
            }

            MaxSubSteps = configuration.MaxSubSteps;
            FixedTimeStep = configuration.FixedTimeStep;

            collisionConfiguration = new BulletSharp.DefaultCollisionConfiguration();
            dispatcher = new BulletSharp.CollisionDispatcher(collisionConfiguration);
            broadphase = new BulletSharp.DbvtBroadphase();

            // Allows characters to have proper physics behavior
            broadphase.OverlappingPairCache.SetInternalGhostPairCallback(new BulletSharp.GhostPairCallback());

            // 2D pipeline
            var simplex = new BulletSharp.VoronoiSimplexSolver();
            var pdSolver = new BulletSharp.MinkowskiPenetrationDepthSolver();
            var convexAlgo = new BulletSharp.Convex2DConvex2DAlgorithm.CreateFunc(simplex, pdSolver);

            dispatcher.RegisterCollisionCreateFunc(BulletSharp.BroadphaseNativeType.Convex2DShape, BulletSharp.BroadphaseNativeType.Convex2DShape, convexAlgo);
            //dispatcher.RegisterCollisionCreateFunc(BulletSharp.BroadphaseNativeType.Box2DShape, BulletSharp.BroadphaseNativeType.Convex2DShape, convexAlgo);
            //dispatcher.RegisterCollisionCreateFunc(BulletSharp.BroadphaseNativeType.Convex2DShape, BulletSharp.BroadphaseNativeType.Box2DShape, convexAlgo);
            //dispatcher.RegisterCollisionCreateFunc(BulletSharp.BroadphaseNativeType.Box2DShape, BulletSharp.BroadphaseNativeType.Box2DShape, new BulletSharp.Box2DBox2DCollisionAlgorithm.CreateFunc());
            //~2D pipeline

            // Default solver
            var solver = new BulletSharp.SequentialImpulseConstraintSolver();

            if (configuration.Flags.HasFlag(PhysicsEngineFlags.CollisionsOnly))
            {
                collisionWorld = new BulletSharp.CollisionWorld(dispatcher, broadphase, collisionConfiguration);
            }
            else if (configuration.Flags.HasFlag(PhysicsEngineFlags.SoftBodySupport))
            {
                //mSoftRigidDynamicsWorld = new BulletSharp.SoftBody.SoftRigidDynamicsWorld(mDispatcher, mBroadphase, solver, mCollisionConf);
                //mDiscreteDynamicsWorld = mSoftRigidDynamicsWorld;
                //mCollisionWorld = mSoftRigidDynamicsWorld;
                throw new NotImplementedException("SoftBody processing is not yet available");
            }
            else
            {
                discreteDynamicsWorld = new BulletSharp.DiscreteDynamicsWorld(dispatcher, broadphase, solver, collisionConfiguration);
                collisionWorld = discreteDynamicsWorld;
            }

            if (discreteDynamicsWorld != null)
            {
                // We are required to keep this reference, or the GC will mess up
                solverInfo = discreteDynamicsWorld.SolverInfo;
                dispatchInfo = discreteDynamicsWorld.DispatchInfo;

                // TODO: Test if helps with performance or not
                solverInfo.SolverMode |= BulletSharp.SolverModes.CacheFriendly;

                if (configuration.Flags.HasFlag(PhysicsEngineFlags.ContinuousCollisionDetection))
                {
                    CanCcd = true;
                    solverInfo.SolverMode |= BulletSharp.SolverModes.Use2FrictionDirections | BulletSharp.SolverModes.RandomizeOrder;
                    dispatchInfo.UseContinuous = true;
                }
            }
        }

        private readonly List<Collision> newCollisionsCache = new List<Collision>();
        private readonly List<Collision> removedCollisionsCache = new List<Collision>();
        private readonly List<ContactPoint> newContactsFastCache = new List<ContactPoint>();
        private readonly List<ContactPoint> updatedContactsCache = new List<ContactPoint>();
        private readonly List<ContactPoint> removedContactsCache = new List<ContactPoint>();

        //private ProfilingState contactsProfilingState;

        private readonly Dictionary<ContactPoint, Collision> contactToCollision = new Dictionary<ContactPoint, Collision>(ContactPointEqualityComparer.Default);

        internal void SendEvents()
        {
            foreach (var collision in newCollisionsCache)
            {
                while (collision.ColliderA.NewPairChannel.Balance < 0)
                {
                    collision.ColliderA.NewPairChannel.Send(collision);
                }

                while (collision.ColliderB.NewPairChannel.Balance < 0)
                {
                    collision.ColliderB.NewPairChannel.Send(collision);
                }
            }

            foreach (var collision in removedCollisionsCache)
            {
                while (collision.ColliderA.PairEndedChannel.Balance < 0)
                {
                    collision.ColliderA.PairEndedChannel.Send(collision);
                }

                while (collision.ColliderB.PairEndedChannel.Balance < 0)
                {
                    collision.ColliderB.PairEndedChannel.Send(collision);
                }
            }

            foreach (var contactPoint in newContactsFastCache)
            {
                if (contactToCollision.TryGetValue(contactPoint, out Collision collision))
                {
                    while (collision.NewContactChannel.Balance < 0)
                    {
                        collision.NewContactChannel.Send(contactPoint);
                    }
                }
            }

            foreach (var contactPoint in updatedContactsCache)
            {
                if (contactToCollision.TryGetValue(contactPoint, out Collision collision))
                {
                    while (collision.ContactUpdateChannel.Balance < 0)
                    {
                        collision.ContactUpdateChannel.Send(contactPoint);
                    }
                }
            }

            foreach (var contactPoint in removedContactsCache)
            {
                if (contactToCollision.TryGetValue(contactPoint, out Collision collision))
                {
                    while (collision.ContactEndedChannel.Balance < 0)
                    {
                        collision.ContactEndedChannel.Send(contactPoint);
                    }
                }
            }

            //contactsProfilingState.End("Contacts: {0}", currentFrameContacts.Count);
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        ///   resources.
        /// </summary>
        public void Dispose()
        {
            //if (mSoftRigidDynamicsWorld != null) mSoftRigidDynamicsWorld.Dispose();
            if (discreteDynamicsWorld != null)
            {
                discreteDynamicsWorld.Dispose();
            }
            else
            {
                collisionWorld?.Dispose();
            }

            broadphase?.Dispose();
            dispatcher?.Dispose();
            collisionConfiguration?.Dispose();
        }

        /// <summary>
        ///   Enables or disables the rendering of collider shapes.
        /// </summary>
        public bool ColliderShapesRendering
        {
            set => processor.RenderColliderShapes(value);
        }

        public RenderGroup ColliderShapesRenderGroup { get; set; } = RenderGroup.Group0;

        internal void AddCollider(PhysicsComponent component, CollisionFilterGroupFlags group, CollisionFilterGroupFlags mask)
        {
            collisionWorld.AddCollisionObject(component.NativeCollisionObject, (BulletSharp.CollisionFilterGroups)group, (BulletSharp.CollisionFilterGroups)mask);
        }

        internal void RemoveCollider(PhysicsComponent component)
        {
            collisionWorld.RemoveCollisionObject(component.NativeCollisionObject);
        }

        internal void AddRigidBody(RigidbodyComponent rigidBody, CollisionFilterGroupFlags group, CollisionFilterGroupFlags mask)
        {
            if (discreteDynamicsWorld is null)
                throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

            discreteDynamicsWorld.AddRigidBody(rigidBody.InternalRigidBody, (short) group, (short) mask);
        }

        internal void RemoveRigidBody(RigidbodyComponent rigidBody)
        {
            if (discreteDynamicsWorld is null)
                throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

            discreteDynamicsWorld.RemoveRigidBody(rigidBody.InternalRigidBody);
        }

        internal void AddCharacter(CharacterComponent character, CollisionFilterGroupFlags group, CollisionFilterGroupFlags mask)
        {
            if (discreteDynamicsWorld is null)
                throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

            var collider = character.NativeCollisionObject;
            var action = character.KinematicCharacter;
            discreteDynamicsWorld.AddCollisionObject(collider, (BulletSharp.CollisionFilterGroups)group, (BulletSharp.CollisionFilterGroups)mask);
            discreteDynamicsWorld.AddAction(action);

            character.Simulation = this;
        }

        internal void RemoveCharacter(CharacterComponent character)
        {
            if (discreteDynamicsWorld is null)
                throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

            var collider = character.NativeCollisionObject;
            var action = character.KinematicCharacter;
            discreteDynamicsWorld.RemoveCollisionObject(collider);
            discreteDynamicsWorld.RemoveAction(action);

            character.Simulation = null;
        }

        /// <summary>
        ///   Creates a constraint.
        /// </summary>
        /// <param name="type">The type of constraint to create.</param>
        /// <param name="rigidBody">The rigid body.</param>
        /// <param name="frame">The frame of reference of the rigid body.</param>
        /// <param name="useReferenceFrame">Value indicating whether to use the reference frame.</param>
        /// <returns>The created constraint.</returns>
        /// <exception cref="Exception">
        ///   <list type="bullet">
        ///     <item>Cannot perform this action when the physics engine is set to process only collisions.</item>
        ///     <item>Both RigidBodies must be valid.</item>
        ///     <item>A gear constraint always needs two rigidbodies to be created.</item>
        ///   </list>
        /// </exception>
        public static Constraint CreateConstraint(ConstraintTypes type, RigidbodyComponent rigidBody, Matrix frame, bool useReferenceFrame = false)
        {
            return CreateConstraintInternal(type, rigidBody, frame, useReferenceFrameA:useReferenceFrame);
        }

        /// <summary>
        ///   Creates a constraint between two rigid bodies.
        /// </summary>
        /// <param name="type">The type of constraint to create.</param>
        /// <param name="rigidBodyA">The rigid body A.</param>
        /// <param name="rigidBodyB">The rigid body B.</param>
        /// <param name="frameA">The frame of reference of rigid body A.</param>
        /// <param name="frameB">The frame of reference of rigid body B.</param>
        /// <param name="useReferenceFrameA">Value indicating whether to use the reference frame of <paramref name="rigidBodyA"/>.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        ///   <list type="bullet">
        ///     <item>Cannot perform this action when the physics engine is set to process only collisions.</item>
        ///     <item>Both RigidBodies must be valid.</item>
        ///   </list>
        /// </exception>
        public static Constraint CreateConstraint(ConstraintTypes type, RigidbodyComponent rigidBodyA, RigidbodyComponent rigidBodyB, Matrix frameA, Matrix frameB, bool useReferenceFrameA = false)
        {
            if (rigidBodyA is null || rigidBodyB is null)
                throw new Exception("Both RigidBodies must be valid.");

            return CreateConstraintInternal(type, rigidBodyA, frameA, rigidBodyB, frameB, useReferenceFrameA);
        }

        static Constraint CreateConstraintInternal(ConstraintTypes type, RigidbodyComponent rigidBodyA, Matrix frameA, RigidbodyComponent rigidBodyB = null, Matrix frameB = default, bool useReferenceFrameA = false)
        {
            if (rigidBodyA is null)
                throw new Exception($"{nameof(rigidBodyA)} must be valid.");
            if (rigidBodyB != null && rigidBodyB.Simulation != rigidBodyA.Simulation)
                throw new Exception("Both RigidBodies must be on the same simulation.");

            Constraint constraintBase;
            var rbA = rigidBodyA.InternalRigidBody;
            var rbB = rigidBodyB?.InternalRigidBody;
            switch (type)
            {
                case ConstraintTypes.Point2Point:
                {
                    var constraint = new Point2PointConstraint
                    {
                        InternalPoint2PointConstraint =
                            rigidBodyB is null ?
                            new BulletSharp.Point2PointConstraint(rbA, frameA.TranslationVector ) :
                            new BulletSharp.Point2PointConstraint(rbA, rbB, frameA.TranslationVector, frameB.TranslationVector),
                    };
                    constraintBase = constraint;

                    constraint.InternalConstraint = constraint.InternalPoint2PointConstraint;
                    break;
                }
                case ConstraintTypes.Hinge:
                {
                    var constraint = new HingeConstraint
                    {
                        InternalHingeConstraint =
                            rigidBodyB is null ?
                                new BulletSharp.HingeConstraint(rbA, frameA ) :
                                new BulletSharp.HingeConstraint(rbA, rbB, frameA, frameB, useReferenceFrameA),
                    };
                    constraintBase = constraint;

                    constraint.InternalConstraint = constraint.InternalHingeConstraint;
                    break;
                }
                case ConstraintTypes.Slider:
                {
                    var constraint = new SliderConstraint
                    {
                        InternalSliderConstraint =
                            rigidBodyB is null ?
                                new BulletSharp.SliderConstraint(rbA, frameA, useReferenceFrameA ) :
                                new BulletSharp.SliderConstraint(rbA, rbB, frameA, frameB, useReferenceFrameA),
                    };
                    constraintBase = constraint;

                    constraint.InternalConstraint = constraint.InternalSliderConstraint;
                    break;
                }
                case ConstraintTypes.ConeTwist:
                {
                    var constraint = new ConeTwistConstraint
                    {
                        InternalConeTwistConstraint =
                            rigidBodyB is null ?
                                new BulletSharp.ConeTwistConstraint(rbA, frameA) :
                                new BulletSharp.ConeTwistConstraint(rbA, rbB, frameA, frameB),
                    };
                    constraintBase = constraint;

                    constraint.InternalConstraint = constraint.InternalConeTwistConstraint;
                    break;
                }
                case ConstraintTypes.Generic6DoF:
                {
                    var constraint = new Generic6DoFConstraint
                    {
                        InternalGeneric6DofConstraint =
                            rigidBodyB is null ?
                                new BulletSharp.Generic6DofConstraint(rbA, frameA, useReferenceFrameA) :
                                new BulletSharp.Generic6DofConstraint(rbA, rbB, frameA, frameB, useReferenceFrameA),
                    };
                    constraintBase = constraint;

                    constraint.InternalConstraint = constraint.InternalGeneric6DofConstraint;
                    break;
                }
                case ConstraintTypes.Generic6DoFSpring:
                {
                    var constraint = new Generic6DoFSpringConstraint
                    {
                        InternalGeneric6DofSpringConstraint =
                            rigidBodyB is null ?
                                new BulletSharp.Generic6DofSpringConstraint(rbA, frameA, useReferenceFrameA) :
                                new BulletSharp.Generic6DofSpringConstraint(rbA, rbB, frameA, frameB, useReferenceFrameA),
                    };
                    constraintBase = constraint;

                    constraint.InternalConstraint = constraint.InternalGeneric6DofConstraint = constraint.InternalGeneric6DofSpringConstraint;
                    break;
                }
                case ConstraintTypes.Gear:
                {
                    var constraint = new GearConstraint
                    {
                        InternalGearConstraint =
                            rigidBodyB is null ?
                                throw new Exception("A Gear constraint always needs two rigidbodies to be created.") :
                                new BulletSharp.GearConstraint(rbA, rbB, frameA.TranslationVector, frameB.TranslationVector),
                    };
                    constraintBase = constraint;

                    constraint.InternalConstraint = constraint.InternalGearConstraint;
                    break;
                }
                default:
                    throw new ArgumentException(type.ToString());
            }

            if(rigidBodyB != null)
            {
                constraintBase.RigidBodyB = rigidBodyB;
                rigidBodyB.LinkedConstraints.Add(constraintBase);
            }
            rigidBodyA.LinkedConstraints.Add(constraintBase);

            return constraintBase;
        }

        /// <summary>
        ///   Adds the constraint to the engine processing pipeline.
        /// </summary>
        /// <param name="constraint">The constraint.</param>
        /// <exception cref="Exception">Cannot perform this action when the physics engine is set to CollisionsOnly.</exception>
        public void AddConstraint(Constraint constraint)
        {
            if (discreteDynamicsWorld is null)
                throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

            discreteDynamicsWorld.AddConstraint(constraint.InternalConstraint);
            constraint.Simulation = this;
        }

        /// <summary>
        ///   Adds the constraint to the engine processing pipeline.
        /// </summary>
        /// <param name="constraint">The constraint.</param>
        /// <param name="disableCollisionsBetweenLinkedBodies">Value indicating whether to disable collisions between linked bodies.</param>
        /// <exception cref="Exception">Cannot perform this action when the physics engine is set to CollisionsOnly.</exception>
        public void AddConstraint(Constraint constraint, bool disableCollisionsBetweenLinkedBodies)
        {
            if (discreteDynamicsWorld is null)
                throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

            discreteDynamicsWorld.AddConstraint(constraint.InternalConstraint, disableCollisionsBetweenLinkedBodies);
            constraint.Simulation = this;
        }

        /// <summary>
        ///   Removes a constraint from the engine processing pipeline.
        /// </summary>
        /// <param name="constraint">The constraint to remove.</param>
        /// <exception cref="Exception">Cannot perform this action when the physics engine is set to CollisionsOnly</exception>
        public void RemoveConstraint(Constraint constraint)
        {
            if (discreteDynamicsWorld is null)
                throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

            discreteDynamicsWorld.RemoveConstraint(constraint.InternalConstraint);
            constraint.Simulation = null;
        }

        /// <summary>
        ///   Traces a ray through the scene and returns the closest hit.
        /// </summary>
        /// <param name="from">Starting point of the ray.</param>
        /// <param name="to">Ending point of the ray.</param>
        /// <param name="filterGroup">The collision group of this raycast.</param>
        /// <param name="filterFlags">The collision group the ray can collide with.</param>
        /// <returns>The hit results.</returns>
        public HitResult Raycast(Vector3 from, Vector3 to, CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterFlags = DefaultFlags)
        {
            var callback = StrideClosestRayResultCallback.Shared(ref from, ref to, filterGroup, filterFlags);
            collisionWorld.RayTest(from, to, callback);
            return callback.Result;
        }

        /// <summary>
        ///   Traces a ray through the scene and determines if it hits something.
        /// </summary>
        /// <param name="from">Starting point of the ray.</param>
        /// <param name="to">Ending point of the ray.</param>
        /// <param name="result">When returning from this method, contains the closest hit.</param>
        /// <param name="filterGroup">The collision group of this raycast.</param>
        /// <param name="filterFlags">The collision group the ray can collide with.</param>
        /// <returns><c>true</c> if the ray has intersected with something in the scene; otherwise, <c>false</c>.</returns>
        public bool Raycast(Vector3 from, Vector3 to, out HitResult result, CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterFlags = DefaultFlags)
        {
            var callback = StrideClosestRayResultCallback.Shared(ref from, ref to, filterGroup, filterFlags);
            collisionWorld.RayTest(from, to, callback);
            result = callback.Result;
            return result.Succeeded;
        }

        /// <summary>
        ///   Traces a ray through the scene and returns any shape it intersects with.
        /// </summary>
        /// <param name="from">Starting point of the ray.</param>
        /// <param name="to">Ending point of the ray.</param>
        /// <param name="resultsOutput">When returning from this method, contains the list of hits.</param>
        /// <param name="filterGroup">The collision group of this raycast.</param>
        /// <param name="filterFlags">The collision group the ray can collide with.</param>
        /// <returns><c>true</c> if the ray has intersected with something in the scene; otherwise, <c>false</c>.</returns>
        public void RaycastPenetrating(Vector3 from, Vector3 to, IList<HitResult> resultsOutput, CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterFlags = DefaultFlags)
        {
            var callback = StrideAllHitsRayResultCallback.Shared(ref from, ref to, resultsOutput, filterGroup, filterFlags);
            collisionWorld.RayTest(from, to, callback);
        }

        /// <summary>
        ///   Performs a sweep test using a collider shape and returns the closest hit.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="from">Starting point of the ray.</param>
        /// <param name="to">Ending point of the ray.</param>
        /// <param name="filterGroup">The collision group of this raycast.</param>
        /// <param name="filterFlags">The collision group the ray can collide with.</param>
        /// <returns>The hit results.</returns>
        /// <exception cref="Exception"><paramref name="shape"/> cannot be used for a shape sweep.</exception>
        public HitResult ShapeSweep(ColliderShape shape, Matrix from, Matrix to, CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterFlags = DefaultFlags)
        {
            var sh = shape.InternalShape as BulletSharp.ConvexShape;
            if (sh is null)
                throw new Exception("This kind of shape cannot be used for a ShapeSweep.");

            var callback = StrideClosestConvexResultCallback.Shared(filterGroup, filterFlags);
            collisionWorld.ConvexSweepTest(sh, from, to, callback);
            return callback.Result;
        }

        /// <summary>
        ///   Performs a sweep test using a collider shape and returns any shape it intersects with.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="from">Starting point of the ray.</param>
        /// <param name="to">Ending point of the ray.</param>
        /// <param name="resultsOutput">When returning from this method, contains the list of hits.</param>
        /// <param name="filterGroup">The collision group of this raycast.</param>
        /// <param name="filterFlags">The collision group the ray can collide with.</param>
        /// <exception cref="Exception"><paramref name="shape"/> cannot be used for a shape sweep.</exception>
        public void ShapeSweepPenetrating(ColliderShape shape, Matrix from, Matrix to, IList<HitResult> resultsOutput, CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterFlags = DefaultFlags)
        {
            if (!(shape.InternalShape is BulletSharp.ConvexShape convexShape))
                throw new Exception("This kind of shape cannot be used for a ShapeSweep.");

            var rcb = StrideAllHitsConvexResultCallback.Shared(resultsOutput, filterGroup, filterFlags);
            collisionWorld.ConvexSweepTest(convexShape, from, to, rcb);
        }

        /// <summary>
        ///   Gets or sets the gravity.
        /// </summary>
        /// <value>The gravity vector.</value>
        /// <exception cref="Exception">Cannot perform this action when the physics engine is set to CollisionsOnly</exception>
        public Vector3 Gravity
        {
            get
            {
                if (discreteDynamicsWorld is null)
                    throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

                return discreteDynamicsWorld.Gravity;
            }
            set
            {
                if (discreteDynamicsWorld is null)
                    throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

                discreteDynamicsWorld.Gravity = value;
            }
        }

        /// <summary>
        ///   Gets or sets the maximum number of steps that the physics simulation is allowed to take each tick.
        /// </summary>
        /// <remarks>
        ///   If the engine is running slow (large deltaTime), then you must increase this number to compensate,
        ///   otherwise your simulation is “losing” time.<br/>
        ///   It's important that frame delta time is always less than <c>MaxSubSteps * FixedTimeStep</c>, otherwise
        ///   you are losing time.
        /// </remarks>
        public int MaxSubSteps { get; set; }

        /// <summary>
        ///   Gets or sets the time step of the simulation.
        /// </summary>
        /// <value>
        ///   Time step of the simulation. Default value is 1/60 or 60 Hz.
        /// </value>
        /// <remarks>
        ///   By decreasing the size of the fixed time step, you are increasing the “resolution” of the simulation.
        /// </remarks>
        public float FixedTimeStep { get; set; }

        public void ClearForces()
        {
            if (discreteDynamicsWorld is null)
                throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

            discreteDynamicsWorld.ClearForces();
        }

        public bool SpeculativeContactRestitution
        {
            get
            {
                if (discreteDynamicsWorld is null)
                    throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

                return discreteDynamicsWorld.ApplySpeculativeContactRestitution;
            }
            set
            {
                if (discreteDynamicsWorld is null)
                    throw new Exception("Cannot perform this action when the physics engine is set to CollisionsOnly.");

                discreteDynamicsWorld.ApplySpeculativeContactRestitution = value;
            }
        }

        public class SimulationArgs : EventArgs
        {
            public float DeltaTime;
        }

        /// <summary>
        ///   Raised right before the physics simulation starts.
        /// </summary>
        /// <remarks>
        ///   This event might not be fired by the main thread.
        /// </remarks>
        public event EventHandler<SimulationArgs> SimulationBegin;

        protected virtual void OnSimulationBegin(SimulationArgs e)
        {
            var handler = SimulationBegin;
            handler?.Invoke(this, e);
        }

        internal int UpdatedRigidbodies;

        private readonly SimulationArgs simulationArgs = new SimulationArgs();

        internal ProfilingState SimulationProfiler;

        internal void Simulate(float deltaTime)
        {
            if (collisionWorld is null)
                return;

            simulationArgs.DeltaTime = deltaTime;

            UpdatedRigidbodies = 0;

            OnSimulationBegin(simulationArgs);

            SimulationProfiler = Profiler.Begin(PhysicsProfilingKeys.SimulationProfilingKey);

            if (discreteDynamicsWorld != null)
                discreteDynamicsWorld.StepSimulation(deltaTime, MaxSubSteps, FixedTimeStep);
            else
                collisionWorld.PerformDiscreteCollisionDetection();

            SimulationProfiler.End("Alive rigidbodies: {0}", UpdatedRigidbodies);

            OnSimulationEnd(simulationArgs);
        }

        /// <summary>
        ///   Raised right after the physics simulation ends.
        /// </summary>
        /// <remarks>
        ///   This event might not be fired by the main thread.
        /// </remarks>
        public event EventHandler<SimulationArgs> SimulationEnd;

        protected virtual void OnSimulationEnd(SimulationArgs e)
        {
            var handler = SimulationEnd;
            handler?.Invoke(this, e);
        }

        private readonly FastList<ContactPoint> newContacts = new FastList<ContactPoint>();
        private readonly FastList<ContactPoint> updatedContacts = new FastList<ContactPoint>();
        private readonly FastList<ContactPoint> removedContacts = new FastList<ContactPoint>();

        private readonly Queue<Collision> collisionsPool = new Queue<Collision>();

        internal void BeginContactTesting()
        {
            // Remove previous frame removed collisions
            foreach (var collision in removedCollisionsCache)
            {
                collision.Destroy();
                collisionsPool.Enqueue(collision);
            }

            // Clean caches
            newCollisionsCache.Clear();
            removedCollisionsCache.Clear();
            newContactsFastCache.Clear();
            updatedContactsCache.Clear();
            removedContactsCache.Clear();

            // Swap the lists
            var previous = currentFrameContacts;
            currentFrameContacts = previousFrameContacts;
            currentFrameContacts.Clear();
            previousFrameContacts = previous;
        }

        private void ContactRemoval(ContactPoint contact, PhysicsComponent component0, PhysicsComponent component1)
        {
            Collision existingPair = null;
            foreach (var x in component0.Collisions)
            {
                if (x.InternalEquals(component0, component1))
                {
                    existingPair = x;
                    break;
                }
            }
            if (existingPair is null)
            {
#if DEBUG
                // Should not happen?
                Log.Warning("Pair not present.");
#endif
                return;
            }

            if (existingPair.Contacts.Contains(contact))
            {
                existingPair.Contacts.Remove(contact);
                removedContactsCache.Add(contact);

                contactToCollision.Remove(contact);

                if (existingPair.Contacts.Count == 0)
                {
                    component0.Collisions.Remove(existingPair);
                    component1.Collisions.Remove(existingPair);
                    removedCollisionsCache.Add(existingPair);
                }
            }
            else
            {
#if DEBUG
                // Should not happen?
                Log.Warning("Contact not in pair.");
#endif
            }
        }

        internal void EndContactTesting()
        {
            newContacts.Clear(fastClear: true);
            updatedContacts.Clear(fastClear: true);
            removedContacts.Clear(fastClear: true);

            foreach (var currentFrameContact in currentFrameContacts)
            {
                if (!previousFrameContacts.Contains(currentFrameContact))
                {
                    newContacts.Add(currentFrameContact);
                }
                else
                {
                    updatedContacts.Add(currentFrameContact);
                }
            }

            foreach (var previousFrameContact in previousFrameContacts)
            {
                if (!currentFrameContacts.Contains(previousFrameContact))
                {
                    removedContacts.Add(previousFrameContact);
                }
            }

            foreach (var contact in newContacts)
            {
                var component0 = contact.ColliderA;
                var component1 = contact.ColliderB;

                Collision existingPair = null;
                foreach (var x in component0.Collisions)
                {
                    if (x.InternalEquals(component0, component1))
                    {
                        existingPair = x;
                        break;
                    }
                }
                if (existingPair != null)
                {
                    if (existingPair.Contacts.Contains(contact))
                    {
#if DEBUG
                        // Should not happen?
                        Log.Warning("Contact already added.");
#endif
                        continue;
                    }

                    existingPair.Contacts.Add(contact);
                }
                else
                {
                    var newPair = collisionsPool.Count == 0 ? new Collision() : collisionsPool.Dequeue();
                    newPair.Initialize(component0, component1);
                    newPair.Contacts.Add(contact);
                    component0.Collisions.Add(newPair);
                    component1.Collisions.Add(newPair);

                    contactToCollision.Add(contact, newPair);

                    newCollisionsCache.Add(newPair);
                    newContactsFastCache.Add(contact);
                }
            }

            foreach (var contact in updatedContacts)
            {
                var component0 = contact.ColliderA;
                var component1 = contact.ColliderB;

                Collision existingPair = null;
                foreach (var x in component0.Collisions)
                {
                    if (x.InternalEquals(component0, component1))
                    {
                        existingPair = x;
                        break;
                    }
                }
                if (existingPair != null)
                {
                    if (existingPair.Contacts.Contains(contact))
                    {
                        // Update data values (since comparison is only at pointer level internally)
                        existingPair.Contacts.Remove(contact);
                        existingPair.Contacts.Add(contact);
                        updatedContactsCache.Add(contact);
                    }
                    else
                    {
#if DEBUG
                        // Should not happen?
                        Log.Warning("Contact not in pair.");
#endif
                    }
                }
                else
                {
#if DEBUG
                    // Should not happen?
                    Log.Warning("Pair not present.");
#endif
                }
            }

            foreach (var contact in removedContacts)
            {
                var component0 = contact.ColliderA;
                var component1 = contact.ColliderB;

                ContactRemoval(contact, component0, component1);
            }
        }

        private DefaultContactResultCallback currentFrameContacts = new DefaultContactResultCallback();
        private DefaultContactResultCallback previousFrameContacts = new DefaultContactResultCallback();

        class DefaultContactResultCallback : BulletSharp.ContactResultCallback
        {
            readonly HashSet<ContactPoint> contacts = new HashSet<ContactPoint>(ContactPointEqualityComparer.Default);

            public override float AddSingleResult(BulletSharp.ManifoldPoint contact, BulletSharp.CollisionObjectWrapper obj0, int partId0, int index0, BulletSharp.CollisionObjectWrapper obj1, int partId1, int index1)
            {
                var component0 = (PhysicsComponent) obj0.CollisionObject.UserObject;
                var component1 = (PhysicsComponent) obj1.CollisionObject.UserObject;

                // Disable static-static
                if ((component0 is StaticColliderComponent && component1 is StaticColliderComponent) ||
                    !component0.Enabled || !component1.Enabled)
                    return 0;

                contacts.Add(new ContactPoint
                {
                    ColliderA = component0,
                    ColliderB = component1,
                    Distance = contact.m_distance1,
                    Normal = contact.m_normalWorldOnB,
                    PositionOnA = contact.m_positionWorldOnA,
                    PositionOnB = contact.m_positionWorldOnB,
                });
                return 0f;
            }

            public void Remove(ContactPoint contact) => contacts.Remove(contact);
            public bool Contains(ContactPoint contact) => contacts.Contains(contact);
            public void Clear() => contacts.Clear();
            public HashSet<ContactPoint>.Enumerator GetEnumerator() => contacts.GetEnumerator();
        }

        internal unsafe void ContactTest(PhysicsComponent component)
        {
            currentFrameContacts.CollisionFilterMask = (int) component.CanCollideWith;
            currentFrameContacts.CollisionFilterGroup = (int) component.CollisionGroup;
            collisionWorld.ContactTest( component.NativeCollisionObject, currentFrameContacts );
        }

        private readonly FastList<ContactPoint> currentToRemove = new FastList<ContactPoint>();
        private readonly HashSet<Collision> collisionsRemovedDueToComponentRemoval = new HashSet<Collision>();

        internal void CleanContacts(PhysicsComponent component)
        {
            currentToRemove.Clear(true);
            collisionsRemovedDueToComponentRemoval.Clear();

            foreach (var currentFrameContact in currentFrameContacts)
            {
                var component0 = currentFrameContact.ColliderA;
                var component1 = currentFrameContact.ColliderB;
                if (component == component0 || component == component1)
                {
                    currentToRemove.Add(currentFrameContact);
                    collisionsRemovedDueToComponentRemoval.Add(contactToCollision[currentFrameContact]);
                    ContactRemoval(currentFrameContact, component0, component1);
                }
            }

            foreach (var contactPoint in currentToRemove)
            {
                currentFrameContacts.Remove(contactPoint);
            }

            foreach (var collision in collisionsRemovedDueToComponentRemoval)
            {
                collision.HasEndedFromComponentRemoval = true;
                while (collision.ColliderA.PairEndedChannel.Balance < 0)
                {
                    collision.ColliderA.PairEndedChannel.Send(collision);
                }

                while (collision.ColliderB.PairEndedChannel.Balance < 0)
                {
                    collision.ColliderB.PairEndedChannel.Send(collision);
                }
            }
        }

        private class StrideAllHitsConvexResultCallback : StrideReusableConvexResultCallback
        {
            [ThreadStatic]
            private static StrideAllHitsConvexResultCallback shared;

            public IList<HitResult> ResultsList { get; set; }

            public StrideAllHitsConvexResultCallback(IList<HitResult> results)
            {
                ResultsList = results;
            }

            public override float AddSingleResult(ref BulletSharp.LocalConvexResult convexResult, bool normalInWorldSpace)
            {
                ResultsList.Add(ComputeHitResult(ref convexResult, normalInWorldSpace));
                return convexResult.m_hitFraction;
            }

            public static StrideAllHitsConvexResultCallback Shared(IList<HitResult> buffer, CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterMask = DefaultFlags)
            {
                if (shared is null)
                    shared = new StrideAllHitsConvexResultCallback(buffer);

                shared.ResultsList = buffer;
                shared.Recycle(filterGroup, filterMask);
                return shared;
            }
        }

        private class StrideClosestConvexResultCallback : StrideReusableConvexResultCallback
        {
            [ThreadStatic]
            static StrideClosestConvexResultCallback shared;

            BulletSharp.LocalConvexResult closestHit;
            bool normalInWorldSpace;
            float? closestFraction;

            public HitResult Result => ComputeHitResult(ref closestHit, normalInWorldSpace);

            public override float AddSingleResult(ref BulletSharp.LocalConvexResult convexResult, bool normalInWorldSpaceParam)
            {
                float fraction = convexResult.m_hitFraction;

                // First hit or closest hit yet
                if (closestFraction is null || closestFraction > fraction)
                {
                    closestHit = convexResult;
                    closestFraction = fraction;
                    normalInWorldSpace = normalInWorldSpaceParam;
                }
                return fraction;
            }

            public override void Recycle(CollisionFilterGroups filterGroup = CollisionFilterGroups.DefaultFilter, CollisionFilterGroupFlags filterMask = (CollisionFilterGroupFlags)(-1))
            {
                base.Recycle(filterGroup, filterMask);
                closestFraction = null;
                closestHit = default;
            }

            public static StrideClosestConvexResultCallback Shared(CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterMask = DefaultFlags)
            {
                if (shared is null)
                    shared = new StrideClosestConvexResultCallback();

                shared.Recycle(filterGroup, filterMask);
                return shared;
            }
        }

        private class StrideAllHitsRayResultCallback : StrideReusableRayResultCallback
        {
            [ThreadStatic]
            private static StrideAllHitsRayResultCallback shared;

            public IList<HitResult> ResultsList { get; set; }

            public StrideAllHitsRayResultCallback(ref Vector3 from, ref Vector3 to, IList<HitResult> results) : base(ref from, ref to)
            {
                ResultsList = results;
            }

            public override float AddSingleResult(ref BulletSharp.LocalRayResult rayResult, bool normalInWorldSpace)
            {
                ResultsList.Add(ComputeHitResult(ref rayResult, normalInWorldSpace));
                return rayResult.m_hitFraction;
            }

            public static StrideAllHitsRayResultCallback Shared(ref Vector3 from, ref Vector3 to, IList<HitResult> buffer, CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterMask = DefaultFlags)
            {
                if (shared is null)
                    shared = new StrideAllHitsRayResultCallback(ref from, ref to, buffer);

                shared.ResultsList = buffer;
                shared.Recycle(ref from, ref to, filterGroup, filterMask);
                return shared;
            }
        }

        private class StrideClosestRayResultCallback : StrideReusableRayResultCallback
        {
            [ThreadStatic]
            private static StrideClosestRayResultCallback shared;

            BulletSharp.LocalRayResult closestHit;
            bool normalInWorldSpace;
            float? closestFraction;

            public HitResult Result => ComputeHitResult(ref closestHit, normalInWorldSpace);

            public StrideClosestRayResultCallback(ref Vector3 from, ref Vector3 to) : base(ref from, ref to)
            {
            }

            public override float AddSingleResult(ref BulletSharp.LocalRayResult rayResult, bool normalInWorldSpaceParam)
            {
                float fraction = rayResult.m_hitFraction;

                // First hit or closest hit yet
                if (closestFraction is null || closestFraction > fraction)
                {
                    closestHit = rayResult;
                    closestFraction = fraction;
                    normalInWorldSpace = normalInWorldSpaceParam;
                    CollisionObject = rayResult.CollisionObject;
                    ClosestHitFraction = fraction;
                }
                return fraction;
            }

            public override void Recycle(ref Vector3 from, ref Vector3 to, CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterMask = DefaultFlags)
            {
                base.Recycle(ref from, ref to, filterGroup, filterMask);
                closestFraction = null;
                closestHit = default;
            }

            public static StrideClosestRayResultCallback Shared(ref Vector3 from, ref Vector3 to, CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterMask = DefaultFlags)
            {
                if (shared is null)
                    shared = new StrideClosestRayResultCallback(ref from, ref to);

                shared.Recycle(ref from, ref to, filterGroup, filterMask);
                return shared;
            }
        }

        private abstract class StrideReusableRayResultCallback : BulletSharp.RayResultCallback
        {
            public Vector3 RayFromWorld { get; protected set; }
            public Vector3 RayToWorld { get; protected set; }

            public StrideReusableRayResultCallback(ref Vector3 from, ref Vector3 to) : base()
            {
                RayFromWorld = from;
                RayToWorld = to;
            }

            public HitResult ComputeHitResult(ref BulletSharp.LocalRayResult rayResult, bool normalInWorldSpace)
            {
                var obj = rayResult.CollisionObject;
                if (obj is null)
                    return new HitResult() { Succeeded = false };

                Vector3 normal = rayResult.m_hitNormalLocal;
                if (!normalInWorldSpace)
                {
                    normal = Vector3.TransformNormal(normal, obj.WorldTransform.Basis);
                }

                return new HitResult
                {
                    Succeeded = true,
                    Collider = obj.UserObject as PhysicsComponent,
                    Point = Vector3.Lerp(RayFromWorld, RayToWorld, rayResult.m_hitFraction),
                    Normal = normal,
                    HitFraction = rayResult.m_hitFraction,
                };
            }

            public virtual void Recycle(ref Vector3 from, ref Vector3 to, CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterMask = DefaultFlags)
            {
                RayFromWorld = from;
                RayToWorld = to;
                ClosestHitFraction = float.PositiveInfinity;
                CollisionObject = null;
                Flags = 0;
                CollisionFilterGroup = (int) filterGroup;
                CollisionFilterMask = (int) filterMask;
            }
        }

        private abstract class StrideReusableConvexResultCallback : BulletSharp.ConvexResultCallback
        {
            public HitResult ComputeHitResult(ref BulletSharp.LocalConvexResult convexResult, bool normalInWorldSpace)
            {
                var obj = convexResult.HitCollisionObject;
                if (obj is null)
                    return new HitResult() { Succeeded = false };

                Vector3 normal = convexResult.m_hitNormalLocal;
                if (!normalInWorldSpace)
                {
                    normal = Vector3.TransformNormal(normal, obj.WorldTransform.Basis);
                }

                return new HitResult
                {
                    Succeeded = true,
                    Collider = obj.UserObject as PhysicsComponent,
                    Point = convexResult.m_hitPointLocal,
                    Normal = normal,
                    HitFraction = convexResult.m_hitFraction,
                };
            }

            public virtual void Recycle(CollisionFilterGroups filterGroup = DefaultGroup, CollisionFilterGroupFlags filterMask = DefaultFlags)
            {
                ClosestHitFraction = float.PositiveInfinity;
                CollisionFilterGroup = (int)filterGroup;
                CollisionFilterMask = (int)filterMask;
            }
        }
    }
}
