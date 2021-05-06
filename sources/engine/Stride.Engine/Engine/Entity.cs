// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Collections;
using Stride.Core.Mathematics;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;

namespace Stride.Engine
{
    /// <summary>
    ///   Represents a game entity. It usually aggregates multiple <see cref="EntityComponent"/>s.
    /// </summary>
    [DataContract("Entity")]
    [DataStyle(DataStyle.Normal)]
    [DataSerializer(typeof(EntitySerializer))]
    [ContentSerializer(typeof(EntityContentSerializer))]
    [DebuggerTypeProxy(typeof(EntityDebugView))]
    public sealed class Entity : ComponentBase, IEnumerable<EntityComponent>, IIdentifiable
    {
        internal TransformComponent TransformValue;
        internal Scene SceneValue;


        /// <summary>
        ///   Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity() : this(name: null) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Entity"/> class with the provided name.
        /// </summary>
        /// <param name="name">The name to give to the entity. It can be <c>null</c>.</param>
        public Entity(string name) : this(position: Vector3.Zero, name) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Entity"/> class with the provided name and initial position.
        /// </summary>
        /// <param name="position">The initial position of the entity</param>
        /// <param name="name">The name to give to the entity. It can be <c>null</c>. Default is no name (<c>null</c>).</param>
        public Entity(Vector3 position, string name = null)
            : this(name, false)
        {
            Id = Guid.NewGuid();
            TransformValue = new TransformComponent { Position = position };
            Components.Add(TransformValue);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Entity"/> class without any components
        ///   (used for binary deserialization).
        /// </summary>
        /// <param name="name">The name to give to the entity. It can be <c>null</c>.</param>
        /// <param name="notUsed">This parameter is not used</param>
        private Entity(string name, bool notUsed) : base(name)
        {
            Components = new EntityComponentCollection(this);
        }


        /// <summary>
        ///   Gets or sets the unique identifier of this entity.
        /// </summary>
        /// <value>A <see cref="Guid"/> that uniquely identifies this instance.</value>
        [DataMember(-10), Display(Browsable = false)]
        [NonOverridable]
        public Guid Id { get; set; }

        /// <summary>
        ///   Gets or sets the name of this entity.
        /// </summary>
        /// <value>The name given to this entity. It can be <c>null</c> it not specified.</value>
        [DataMember(0)]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        /// <summary>
        ///   Gets or sets the <see cref="Engine.Scene"/> this entity is in.
        /// </summary>
        /// <value>
        ///   The <see cref="Engine.Scene"/> this entity belongs to. Setting this to <c>null</c> will remove
        ///   the entity from the scene and detach it from its parent if it has one.
        /// </value>
        [DataMemberIgnore]
        public Scene Scene
        {
            get => this.FindRoot().SceneValue;
            set
            {
                if (Transform.Parent is not null)
                {
                    if (value is not null)
                        throw new InvalidOperationException("This entity is another entity's child. Detach it before changing its scene.");

                    Transform.Parent = null;
                    return;
                }

                var oldScene = SceneValue;
                if (oldScene == value)
                    return;

                oldScene?.Entities.Remove(this);
                value?.Entities.Add(this);
            }
        }

        /// <summary>
        ///   Gets the <see cref="Engine.EntityManager"/> this entity belongs to.
        /// </summary>
        /// <value>The <see cref="Engine.EntityManager"/> that processes this entity.</value>
        [DataMemberIgnore]
        public EntityManager EntityManager { get; internal set; }

        /// <summary>
        ///   Gets the <see cref="TransformComponent"/> associated to this entity.
        /// </summary>
        /// <value>The <see cref="TransformComponent"/> that defines the spatial properties of thie entity.</value>
        [DataMemberIgnore]
        public TransformComponent Transform => TransformValue;

        /// <summary>
        ///   Gets a collection of components attached to this entity.
        /// </summary>
        /// <value>A collection of the <see cref="EntityComponent"/>s of this entity.</value>
        [DataMember(100, DataMemberMode.Content)]
        [MemberCollection(CanReorderItems = true, NotNullItems = true)]
        public EntityComponentCollection Components { get; }


        /// <summary>
        ///   Gets or creates a component of this entity of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="EntityComponent"/>.</typeparam>
        /// <returns>A new or an existing instance of the component.</returns>
        public T GetOrCreate<T>() where T : EntityComponent, new()
        {
            var component = Components.Get<T>();
            if (component is null)
            {
                component = new T();
                Components.Add(component);
            }

            return component;
        }

        /// <summary>
        ///   Adds the specified component to the entity.
        /// </summary>
        /// <param name="component">The component to add.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="component"/> to add cannot be <c>null</c>.</exception>
        public void Add(EntityComponent component)
        {
            Components.Add(component);
        }

        /// <summary>
        ///   Gets the first component of the specified type or derived type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="EntityComponent"/>.</typeparam>
        /// <returns>
        ///   The first component of type <typeparamref name="T"/> found;
        ///   or <c>null</c> if no component of that type were found.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get<T>() where T : EntityComponent => Components.Get<T>();

        /// <summary>
        ///   Gets the <c>N</c>th component of the specified type or derived type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="EntityComponent"/>.</typeparam>
        /// <param name="index">Index of the component of the same type, starting from zero.</param>
        /// <returns>
        ///   The <paramref name="index"/>th component of type <typeparamref name="T"/> found;
        ///   or <c>null</c> if no component of that type were found or <paramref name="index"/> was out of range.
        /// </returns>
        /// <remarks>
        ///   <list type="bullet">
        ///     <item>A non-zero positive <paramref name="index"/> will search from the beginning of the collection to the end.</item>
        ///     <item>A non-zero negative <paramref name="index"/> will search starting from the end of the collection to the beginning.</item>
        ///     <item>An <paramref name="index"/> of 0 is equivalent to calling <see cref="Get{T}()"/>.</item>
        ///   </list>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get<T>(int index) where T : EntityComponent => Components.Get<T>(index);

        /// <summary>
        ///   Gets all the components of the specified type or derived type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="EntityComponent"/>.</typeparam>
        /// <returns>An enumeration of the components matching the specified type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<T> GetAll<T>() where T : EntityComponent => Components.GetAll<T>();

        /// <summary>
        ///   Removes the first component of the specified type or derived type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="EntityComponent"/>.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove<T>() where T : EntityComponent => Components.Remove<T>();

        /// <summary>
        ///   Removes the specifies component.
        /// </summary>
        /// <param name="component">The <see cref="EntityComponent"/> to remove.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(EntityComponent component) => Components.Remove(component);

        /// <summary>
        ///   Removes all components of the specified type or derived type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="EntityComponent"/>.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAll<T>() where T : EntityComponent => Components.RemoveAll<T>();

        internal void OnComponentChanged(int index, EntityComponent oldComponent, EntityComponent newComponent)
        {
            // Don't use events but directly call the Owner
            EntityManager?.NotifyComponentChanged(this, index, oldComponent, newComponent);
        }

        public override string ToString() => Name is null ? "Entity" : $"Entity {Name}";

        /// <summary>
        ///   Gets an enumerator to iterate over the <see cref="EntityComponent"/>s of this entity.
        /// </summary>
        /// <returns>An enumerator to iterate over the <see cref="EntityComponent"/>s of this entity.</returns>
        public FastCollection<EntityComponent>.Enumerator GetEnumerator() => Components.GetEnumerator();
        IEnumerator<EntityComponent> IEnumerable<EntityComponent>.GetEnumerator() => Components.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Components.GetEnumerator();

        /// <summary>
        ///   A serializer which will not populate the new entity with a default transform.
        /// </summary>
        internal class EntityContentSerializer : DataContentSerializerWithReuse<Entity>
        {
            public override object Construct(ContentSerializerContext context)
            {
                return new Entity(null, false);
            }
        }

        /// <summary>
        ///   Dedicated Debugger for an entity that displays children from Entity.Transform.Children.
        /// </summary>
        internal class EntityDebugView
        {
            private readonly Entity entity;

            public EntityDebugView(Entity entity)
            {
                this.entity = entity;
            }

            public string Name => entity.Name;

            public Guid Id => entity.Id;

            public Entity Parent => entity.Transform?.Parent?.Entity;

            public Entity[] Children => entity.Transform?.Children.Select(x => x.Entity).ToArray();

            public EntityComponent[] Components => entity.Components.ToArray();
        }

        /// <summary>
        ///   Specialized serializer.
        /// </summary>
        /// <seealso cref="Entity" />
        internal class EntitySerializer : DataSerializer<Entity>
        {
            private DataSerializer<Guid> guidSerializer;
            private DataSerializer<string> stringSerializer;
            private DataSerializer<EntityComponentCollection> componentCollectionSerializer;

            /// <inheritdoc/>
            public override void Initialize(SerializerSelector serializerSelector)
            {
                guidSerializer = MemberSerializer<Guid>.Create(serializerSelector);
                stringSerializer = MemberSerializer<string>.Create(serializerSelector);
                componentCollectionSerializer = MemberSerializer<EntityComponentCollection>.Create(serializerSelector);
            }

            public override void PreSerialize(ref Entity obj, ArchiveMode mode, SerializationStream stream)
            {
                // Create an empty Entity without a Transform component by default when deserializing
                if (mode == ArchiveMode.Deserialize)
                {
                    if (obj is null)
                        obj = new Entity(null, false);
                    else
                        obj.Components.Clear();
                }
            }

            public override void Serialize(ref Entity obj, ArchiveMode mode, SerializationStream stream)
            {
                // Serialize Id
                var id = obj.Id;
                guidSerializer.Serialize(ref id, mode, stream);
                if (mode == ArchiveMode.Deserialize)
                {
                    obj.Id = id;
                }

                // Serialize Name
                var name = obj.Name;
                stringSerializer.Serialize(ref name, mode, stream);
                obj.Name = name;

                // Components
                var collection = obj.Components;
                componentCollectionSerializer.Serialize(ref collection, mode, stream);
            }
        }
    }
}
