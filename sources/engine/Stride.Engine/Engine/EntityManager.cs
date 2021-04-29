// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Core.ReferenceCounting;
using Stride.Core.Reflection;
using Stride.Engine.Design;
using Stride.Games;
using Stride.Rendering;

namespace Stride.Engine
{
    /// <summary>
    ///   Represents a class that manages a collection of <see cref="Entity"/>.
    /// </summary>
    public abstract class EntityManager : ComponentBase, Core.Collections.IReadOnlySet<Entity>
    {
        // TODO: Make this class threadsafe (current locks aren't sufficient)

        public ExecutionMode ExecutionMode { get; protected set; } = ExecutionMode.Runtime;

        // Set of all the contained Entities
        private readonly HashSet<Entity> entities;

        // Collection of Entity Processors currently registered
        private readonly TrackingEntityProcessorCollection processors;
        // Ordered list of Entity Processors to make sure they are added in the correct order as much as possible
        private readonly EntityProcessorCollection pendingProcessors;
        // Dictionary of Entity Processors per EntityComponent final type
        internal readonly Dictionary<TypeInfo, EntityProcessorCollectionPerComponentType> MapComponentTypeToProcessors;

        private readonly List<EntityProcessor> currentDependentProcessors;
        private readonly HashSet<TypeInfo> componentTypes;
        private int addEntityLevel = 0;

        /// <summary>
        ///   Occurs when an <see cref="Entity"/> is added.
        /// </summary>
        public event EventHandler<Entity> EntityAdded;
        /// <summary>
        ///   Occurs when an <see cref="Entity"/> is removed.
        /// </summary>
        public event EventHandler<Entity> EntityRemoved;

        /// <summary>
        ///   Occurs when the transform hierarchy of an entity is changed.
        /// </summary>
        /// <seealso cref="TransformComponent.Parent"/>
        /// <seealso cref="TransformComponent.Children"/>
        public event EventHandler<Entity> HierarchyChanged;

        /// <summary>
        ///   Occurs when a new <see cref="EntityComponent"/> type is added.
        /// </summary>
        public event EventHandler<TypeInfo> ComponentTypeAdded;

        /// <summary>
        ///   Occurs when a <see cref="EntityComponent"/> has been added or removed from an <see cref="Entity"/>.
        /// </summary>
        public event EventHandler<EntityComponentEventArgs> ComponentChanged;

        /// <summary>
        ///   Initializes a new instance of the <see cref="EntityManager"/> class.
        /// </summary>
        /// <param name="registry">The registry of game services.</param>
        /// <exception cref="ArgumentNullException"><paramref name="registry"/> is a <c>null</c> reference.</exception>
        protected EntityManager(IServiceRegistry registry)
        {
            if (registry is null)
                throw new ArgumentNullException(nameof(registry));

            Services = registry;

            entities = new HashSet<Entity>();
            processors = new TrackingEntityProcessorCollection(this);
            pendingProcessors = new EntityProcessorCollection();

            componentTypes = new HashSet<TypeInfo>();
            MapComponentTypeToProcessors = new Dictionary<TypeInfo, EntityProcessorCollectionPerComponentType>();

            currentDependentProcessors = new List<EntityProcessor>(10);
        }

        /// <summary>
        ///   Gets the registry of the services of the game.
        /// </summary>
        public IServiceRegistry Services { get; private set; }

        /// <summary>
        ///   Gets a list of the registered <see cref="EntityProcessor"/>s.
        /// </summary>
        public EntityProcessorCollection Processors => processors;

        /// <summary>
        ///   Gets the number of entities.
        /// </summary>
        public int Count => entities.Count;

        /// <summary>
        ///   Gets a list of the <see cref="EntityComponent"/> types from the entities.
        /// </summary>
        /// <value>The registered component types.</value>
        public IEnumerable<TypeInfo> ComponentTypes => componentTypes;

        public virtual void Update(GameTime gameTime)
        {
            foreach (var processor in processors)
            {
                if (processor.Enabled)
                {
                    using (processor.UpdateProfilingState = Profiler.Begin(processor.UpdateProfilingKey, "Entities: {0}", entities.Count))
                    {
                        processor.Update(gameTime);
                    }
                }
            }
        }

        /// <summary>
        ///   Determines whether this instance contains the specified <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The entity to look for.</param>
        /// <returns><c>true</c> if this instance contains the specified <see cref="Entity"/>; otherwise, <c>false</c>.</returns>
        public bool Contains(Entity entity)
        {
            return entities.Contains(entity);
        }

        /// <summary>
        ///   Gets an enumeration object with which you can loop over the entities in iteration.
        /// </summary>
        /// <returns>An enumerator over the entities in this instance.</returns>
        public HashSet<Entity>.Enumerator GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        /// <summary>
        ///   Gets the first <see cref="EntityProcessor"/> of the provided type.
        /// </summary>
        /// <typeparam name="TProcessor">Type of the processor.</typeparam>
        /// <returns>
        ///   The first processor of the provided type <typeparamref name="TProcessor"/>;
        ///   or <c>null</c> if not found.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TProcessor GetProcessor<TProcessor>() where TProcessor : EntityProcessor
        {
            return Processors.Get<TProcessor>();
        }

        /// <summary>
        ///   Removes the specified <see cref="Entity"/> from this <see cref="EntityManager"/>.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <remarks>
        ///   This method removes the entity whether it has a parent or not.
        ///   In conjonction with <see cref="HierarchicalProcessor" />, it will remove child entities as well.
        /// </remarks>
        public void Remove(Entity entity)
        {
            InternalRemoveEntity(entity, removeParent: true);
        }

        /// <summary>
        ///   Removes all the entities from this <see cref="EntityManager"/>.
        /// </summary>
        protected internal virtual void Reset()
        {
            var entitiesToRemove = entities.ToList();
            foreach (var entity in entitiesToRemove)
            {
                InternalRemoveEntity(entity, true);
            }

            entities.Clear();
            componentTypes.Clear();
            MapComponentTypeToProcessors.Clear();
            pendingProcessors.Clear();
            processors.Clear();
        }

        /// <summary>
        ///   Draws the entities in this instance through all the enabled <see cref="EntityProcessor"/>s.
        /// </summary>
        /// <param name="context">The render context.</param>
        public virtual void Draw(RenderContext context)
        {
            foreach (var processor in processors)
            {
                if (processor.Enabled)
                {
                    using (processor.DrawProfilingState = Profiler.Begin(processor.DrawProfilingKey))
                    {
                        processor.Draw(context);
                    }
                }
            }
        }

        /// <summary>
        ///   Adds an <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <exception cref="ArgumentException">Entity shouldn't have a parent.;entity</exception>
        /// <remarks>
        ///   If the provided <see cref="Entity" /> has a parent, its parent should also be added (or <see cref="TransformComponent.Children" />)
        ///   should be used.
        /// </remarks>
        internal void Add(Entity entity)
        {
            // Entity can't be a root because it already has a parent?
            if (entity.Transform is not null && entity.Transform.Parent is not null)
                throw new ArgumentException("Entity shouldn't have a parent.", nameof(entity));

            InternalAddEntity(entity);
        }

        /// <summary>
        ///   Adds the specified <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        internal void InternalAddEntity(Entity entity)
        {
            // Already added?
            if (entities.Contains(entity))
                return;

            if (entity.EntityManager != null)
                throw new InvalidOperationException("Cannot add an Entity to this EntityManager when it is already used by another one.");

            // Add this entity to our internal hashset
            entity.EntityManager = this;
            entities.Add(entity);
            entity.AddReferenceInternal();

            // Because a processor can add entities, we want to make sure that
            // the RegisterPendingProcessors is called only at the top level
            {
                addEntityLevel++;

                // Check which exiting processor are working with the components of this entity
                // and grab the list of new processors to registers
                CheckEntityWithProcessors(entity, forceRemove: false, collectComponentTypesAndProcessors: true);

                addEntityLevel--;
            }

            // Register all new processors
            RegisterPendingProcessors();

            OnEntityAdded(entity);
        }

        private void RegisterPendingProcessors()
        {
            // Auto-register all new processors
            if (addEntityLevel == 0 && pendingProcessors.Count > 0)
            {
                // Add all new processors
                foreach (var newProcessor in pendingProcessors)
                {
                    processors.Add(newProcessor);
                }
                pendingProcessors.Clear();
            }
        }

        /// <summary>
        ///   Removes the specified <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <param name="removeParent">A value indicating if the entity should be removed from its parent.</param>
        internal void InternalRemoveEntity(Entity entity, bool removeParent)
        {
            // Entity wasn't already added
            if (!entities.Contains(entity))
                return;

            entities.Remove(entity);

            if (removeParent && entity.Transform != null)
            {
                // Force parent to be null, so that it is removed even if it is not a root node
                entity.Transform.Parent = null;
            }

            // Notify Processors this entity has been removed
            CheckEntityWithProcessors(entity, forceRemove: true, collectComponentTypesAndProcessors: false);

            entity.ReleaseInternal();

            entity.EntityManager = null;

            OnEntityRemoved(entity);
        }

        private void CollectNewProcessorsByComponentType(TypeInfo componentType)
        {
            if (componentTypes.Contains(componentType))
                return;

            componentTypes.Add(componentType);
            OnComponentTypeAdded(componentType);

            // Automatically collect processors that are used by this component
            var processorAttributes = componentType.GetCustomAttributes<DefaultEntityComponentProcessorAttribute>();
            foreach (var processorAttributeType in processorAttributes)
            {
                var processorType = AssemblyRegistry.GetType(processorAttributeType.TypeName);
                if (processorType is null || !typeof(EntityProcessor).GetTypeInfo().IsAssignableFrom(processorType.GetTypeInfo()))
                {
                    // TODO: Log an error if type is not of EntityProcessor
                    continue;
                }

                // Filter using ExecutionMode
                if ((ExecutionMode & processorAttributeType.ExecutionMode) != 0)
                {
                    // Make sure that we are adding a processor of the specified type only if it is not already in the list or pending

                    // 1) Check in the list of existing processors
                    var addNewProcessor = true;
                    for (int i = 0; i < processors.Count; i++)
                    {
                        if (processorType == processors[i].GetType())
                        {
                            addNewProcessor = false;
                            break;
                        }
                    }
                    if (addNewProcessor)
                    {
                        // 2) Check in the list of pending processors
                        for (int i = 0; i < pendingProcessors.Count; i++)
                        {
                            if (processorType == pendingProcessors[i].GetType())
                            {
                                addNewProcessor = false;
                                break;
                            }
                        }

                        // If not found, we can add this processor
                        if (addNewProcessor)
                        {
                            var processor = (EntityProcessor) Activator.CreateInstance(processorType);
                            pendingProcessors.Add(processor);

                            // Collect dependencies
                            foreach (var subComponentType in processor.RequiredTypes)
                            {
                                CollectNewProcessorsByComponentType(subComponentType);
                            }
                        }
                    }
                }
            }
        }

        //
        // Called when a new Entity Processor has been added to the Entity Manager.
        //
        private void OnProcessorAdded(EntityProcessor processor)
        {
            processor.EntityManager = this;
            processor.Services = Services;

            processor.OnSystemAdd();

            // Update processor per types and dependencies
            foreach (var componentTypeAndProcessors in MapComponentTypeToProcessors)
            {
                var componentType = componentTypeAndProcessors.Key;
                var processorList = componentTypeAndProcessors.Value;

                if (processor.Accept(componentType))
                    componentTypeAndProcessors.Value.Add(processor);

                // Add dependent component
                if (processor.IsDependentOnComponentType(componentType))
                {
                    if (processorList.Dependencies is null)
                        processorList.Dependencies = new List<EntityProcessor>();

                    processorList.Dependencies.Add(processor);
                }
            }

            // NOTE: It is important to perform a ToList() as the TransformProcessor adds children Entities and modifies the current list of Entities
            foreach (var entity in entities.ToList())
            {
                CheckEntityWithNewProcessor(entity, processor);
            }
        }

        //
        // Called when an Entity Processor has been removed from the Entity Manager.
        //
        private void OnProcessorRemoved(EntityProcessor processor)
        {
            // Remove the Processor from any list
            foreach (var componentTypeAndProcessors in MapComponentTypeToProcessors)
            {
                var processorList = componentTypeAndProcessors.Value;

                processorList.Remove(processor);

                if (processorList.Dependencies is not null)
                    processorList.Dependencies.Remove(processor);
            }

            processor.RemoveAllEntities();

            processor.OnSystemRemove();
            processor.Services = null;
            processor.EntityManager = null;
        }

        //
        // Called when a Component has been added / removed from an Entity.
        //
        internal void NotifyComponentChanged(Entity entity, int index, EntityComponent oldComponent, EntityComponent newComponent)
        {
            // No real update
            if (oldComponent == newComponent)
                return;

            // If we have a new Component we can try to collect Processors for it
            if (newComponent is not null)
            {
                CollectNewProcessorsByComponentType(newComponent.GetType().GetTypeInfo());
                RegisterPendingProcessors();
            }

            // Remove previous Component from Processors
            currentDependentProcessors.Clear();
            if (oldComponent is not null)
            {
                CheckEntityComponentWithProcessors(entity, oldComponent, forceRemove: true, currentDependentProcessors);
            }

            // Add new Component to Processors
            if (newComponent is not null)
            {
                CheckEntityComponentWithProcessors(entity, newComponent, forceRemove: false, currentDependentProcessors);
            }

            // Update all dependencies
            if (currentDependentProcessors.Count > 0)
            {
                UpdateDependentProcessors(entity, oldComponent, newComponent);
                currentDependentProcessors.Clear();
            }

            // Notify Component changes
            OnComponentChanged(entity, index, oldComponent, newComponent);
        }

        //
        // Updates the Processors that depend on Components other than the ones added / removed so they check whether
        // they have all their dependencies covered.
        //
        private void UpdateDependentProcessors(Entity entity, EntityComponent oldComponent, EntityComponent newComponent)
        {
            var components = entity.Components;

            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];

                if (component == oldComponent || component == newComponent)
                    continue;

                var componentType = component.GetType().GetTypeInfo();
                var processorsForComponent = MapComponentTypeToProcessors[componentType];

                for (int j = 0; j < processorsForComponent.Count; j++)
                {
                    var processor = processorsForComponent[j];
                    if (currentDependentProcessors.Contains(processor))
                    {
                        processor.ProcessEntityComponent(entity, component, forceRemove: false);
                    }
                }
            }
        }

        //
        // Checks if the registered Processors accept an Entity and track its Components.
        //
        private void CheckEntityWithProcessors(Entity entity, bool forceRemove, bool collectComponentTypesAndProcessors)
        {
            var components = entity.Components;

            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];

                CheckEntityComponentWithProcessors(entity, component, forceRemove, dependentProcessors: null);

                if (collectComponentTypesAndProcessors)
                    CollectNewProcessorsByComponentType(component.GetType().GetTypeInfo());
            }
        }

        //
        // Checks whether a newly added Processor accepts an Entity based on its Components (and tracks it internally).
        //
        private void CheckEntityWithNewProcessor(Entity entity, EntityProcessor processor)
        {
            var components = entity.Components;

            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];

                if (processor.Accept(component.GetType().GetTypeInfo()))
                    processor.ProcessEntityComponent(entity, component, forceRemove: false);
            }
        }

        //
        // Checks whether the registered Processors accept (and track) an Entity Component, resolve the dependent Components
        // and Processors, and cache the results.
        //
        private void CheckEntityComponentWithProcessors(Entity entity, EntityComponent component, bool forceRemove, List<EntityProcessor> dependentProcessors)
        {
            var componentType = component.GetType().GetTypeInfo();

            if (MapComponentTypeToProcessors.TryGetValue(componentType, out EntityProcessorCollectionPerComponentType processorsForComponent))
            {
                for (int i = 0; i < processorsForComponent.Count; i++)
                    processorsForComponent[i].ProcessEntityComponent(entity, component, forceRemove);
            }
            else
            {
                processorsForComponent = new EntityProcessorCollectionPerComponentType();
                for (int j = 0; j < processors.Count; j++)
                {
                    var processor = processors[j];

                    if (processor.Accept(componentType))
                    {
                        processorsForComponent.Add(processor);
                        processor.ProcessEntityComponent(entity, component, forceRemove);
                    }

                    if (processor.IsDependentOnComponentType(componentType))
                    {
                        if (processorsForComponent.Dependencies is null)
                            processorsForComponent.Dependencies = new List<EntityProcessor>();

                        processorsForComponent.Dependencies.Add(processor);
                    }
                }
                MapComponentTypeToProcessors.Add(componentType, processorsForComponent);
            }

            // Collect dependent Processors
            var processorsForComponentDependencies = processorsForComponent.Dependencies;
            if (dependentProcessors is not null &&
                processorsForComponentDependencies is not null)
            {
                for (int i = 0; i < processorsForComponentDependencies.Count; i++)
                {
                    var processor = processorsForComponentDependencies[i];

                    if (!dependentProcessors.Contains(processor))
                        dependentProcessors.Add(processor);
                }
            }
        }

        IEnumerator<Entity> IEnumerable<Entity>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///   Method called when a new type of <see cref="EntityComponent"/> has been added to the <see cref="EntityManager"/>.
        ///   Raises the event <see cref="ComponentTypeAdded"/>.
        ///   Override in a derived class to implement a custom behavior for when a Component type has been added.
        /// </summary>
        /// <param name="componentType">The type of the Component.</param>
        protected virtual void OnComponentTypeAdded(TypeInfo componentType) => ComponentTypeAdded?.Invoke(this, componentType);

        /// <summary>
        ///   Method called when an Entity has been added to the <see cref="EntityManager"/>.
        ///   Raises the event <see cref="EntityAdded"/>.
        ///   Override in a derived class to implement a custom behavior for when an Entity has been added.
        /// </summary>
        /// <param name="e">The added Entity.</param>
        protected virtual void OnEntityAdded(Entity e) => EntityAdded?.Invoke(this, e);

        /// <summary>
        ///   Method called when an Entity has been removed from the <see cref="EntityManager"/>.
        ///   Raises the event <see cref="EntityRemoved"/>.
        ///   Override in a derived class to implement a custom behavior for when an Entity has been removed.
        /// </summary>
        /// <param name="e">The removed Entity.</param>
        protected virtual void OnEntityRemoved(Entity e) => EntityRemoved?.Invoke(this, e);

        /// <summary>
        ///   Method called when a Component has been added / removed in an Entity.
        ///   Raises the event <see cref="ComponentChanged"/>.
        ///   Override in a derived class to implement a custom behavior for when an Component has changed.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <param name="index">The index of the Component in the collection of Components.</param>
        /// <param name="newComponent">The newly added Component, or <c>null</c> if no Component was added.</param>
        /// <param name="previousComponent">The removed Component, or <c>null</c> if no Component was removed.</param>
        protected virtual void OnComponentChanged(Entity entity, int index, EntityComponent previousComponent, EntityComponent newComponent)
        {
            ComponentChanged?.Invoke(this, new EntityComponentEventArgs(entity, index, previousComponent, newComponent));
        }

        /// <summary>
        ///   Method called when the hierarchy of Entities has changed.
        ///   Raises the event <see cref="HierarchyChanged"/>.
        /// </summary>
        /// <param name="entity">The Entity that has been added / removed from the hierarchy.</param>
        internal void OnHierarchyChanged(Entity entity) => HierarchyChanged?.Invoke(this, entity);


        /// <summary>
        ///   Represents a collection of <see cref="EntityProcessor"/>s for a particular <see cref="EntityComponent"/> type.
        /// </summary>
        internal class EntityProcessorCollectionPerComponentType : EntityProcessorCollection
        {
            /// <summary>
            ///   The processors that are depending on the component type
            /// </summary>
            public List<EntityProcessor> Dependencies;
        }

        /// <summary>
        ///   Represents a collection of <see cref="EntityProcessor"/>s.
        /// </summary>
        private class TrackingEntityProcessorCollection : EntityProcessorCollection
        {
            private readonly EntityManager manager;

            public TrackingEntityProcessorCollection(EntityManager manager)
            {
                if (manager is null)
                    throw new ArgumentNullException(nameof(manager));

                this.manager = manager;
            }

            protected override void ClearItems()
            {
                for (int i = 0; i < Count; i++)
                    manager.OnProcessorRemoved(this[i]);

                base.ClearItems();
            }

            protected override void AddItem(EntityProcessor processor)
            {
                if (processor is null)
                    throw new ArgumentNullException(nameof(processor));

                if (!Contains(processor))
                {
                    base.AddItem(processor);
                    manager.OnProcessorAdded(processor);
                }
            }

            protected override void RemoveItem(int index)
            {
                var processor = this[index];

                base.RemoveItem(index);
                manager.OnProcessorRemoved(processor);
            }
        }
    }
}
