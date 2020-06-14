// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Serialization;

namespace Stride.Engine
{
    /// <summary>
    ///   Base class for <see cref="Engine.Entity"/> components.
    /// </summary>
    [DataSerializer(typeof(Serializer))]
    [DataContract(Inherited = true)]
    [ComponentCategory("Miscellaneous")]
    public abstract class EntityComponent : IIdentifiable
    {
        /// <summary>
        ///   Gets the owner entity.
        /// </summary>
        /// <value>The owner entity.</value>
        [DataMemberIgnore]
        public Entity Entity { get; internal set; }

        /// <summary>
        ///   Gets or sets the unique identifier of this component.
        /// </summary>
        [DataMember(int.MinValue)]
        [Display(Browsable = false)]
        [NonOverridable]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        ///   Gets the owner entity ensuring it is not <c>null</c>.
        /// </summary>
        /// <value>The owner entity.</value>
        /// <exception cref="InvalidOperationException">The owner <see cref="Entity"/> on this instance is a <c>null</c> reference.</exception>
        [DataMemberIgnore]
        protected Entity EnsureEntity => Entity ?? throw new InvalidOperationException($"Entity on this instance [{GetType().Name}] cannot be null.");

        internal class Serializer : DataSerializer<EntityComponent>
        {
            private DataSerializer<Guid> guidSerializer;

            /// <inheritdoc/>
            public override void Initialize(SerializerSelector serializerSelector)
            {
                guidSerializer = MemberSerializer<Guid>.Create(serializerSelector);
            }

            public override void Serialize(ref EntityComponent obj, ArchiveMode mode, SerializationStream stream)
            {
                var entity = obj.Entity;

                // Force containing Entity to be collected by serialization, no need to reassign it to EntityComponent.Entity
                stream.SerializeExtended(ref entity, mode);

                // Serialize Id
                var id = obj.Id;
                guidSerializer.Serialize(ref id, mode, stream);
                if (mode == ArchiveMode.Deserialize)
                {
                    obj.Id = id;
                }
            }
        }
    }
}
