// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1402 // File may only contain a single class
#pragma warning disable SA1025 // Code must not contain multiple whitespace in a row

using System;
using System.Runtime.CompilerServices;
using System.Threading;

using Stride.Core.Annotations;
using Stride.Core.Storage;

namespace Stride.Core.Serialization
{
    /// <summary>
    ///   Represents a data serializer that describes how to serialize and deserialize an object without knowing its
    ///   <see cref="Type"/>. Used as a common base class for all data serializers.
    /// </summary>
    public abstract partial class DataSerializer
    {
        /// <summary>
        ///   The type id of <see cref="SerializationType"/>. Used internally to avoid dealing with strings.
        /// </summary>
        public ObjectId SerializationTypeId;

        /// <summary>
        ///   Used internally to know if the serializer has been initialized.
        /// </summary>
        internal bool Initialized;
        internal SpinLock InitializeLock = new SpinLock(true);

        /// <summary>
        ///   The <see cref="Type"/> of the object that can be serialized or deserialized.
        /// </summary>
        public abstract Type SerializationType { get; }

        public abstract bool IsBlittable { get; }


        /// <summary>
        ///   Initializes the specified serializer.
        /// </summary>
        /// <remarks>This method should be thread-safe and reentrant.</remarks>
        /// <param name="serializerSelector">The serializer.</param>
        public virtual void Initialize([NotNull] SerializerSelector serializerSelector) { }

        /// <summary>
        ///   Serializes or deserializes the given object.
        /// </summary>
        /// <param name="obj">The object to serialize or deserialize.</param>
        /// <param name="mode">The serialization mode.</param>
        /// <param name="stream">The stream to serialize to or deserialize from.</param>
        public abstract void Serialize(ref object obj, ArchiveMode mode, [NotNull] SerializationStream stream);

        /// <summary>
        ///   Performs the first step of serialization or deserialization.
        /// </summary>
        /// <param name="obj">The object to process.</param>
        /// <param name="mode">The serialization mode.</param>
        /// <param name="stream">The stream to serialize to or deserialize from.</param>
        /// <remarks>
        ///   Typically, if <paramref name="obj"/> is <c>null</c> it will instantiate the object. If it is a collection
        ///   it will clear it.
        /// </remarks>
        public abstract void PreSerialize(ref object obj, ArchiveMode mode, [NotNull] SerializationStream stream);
    }

    /// <summary>
    ///   Represents a data serializer that describes how to serialize and deserialize an object of a given type.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize or deserialize.</typeparam>
    public abstract class DataSerializer<T> : DataSerializer
    {
        /// <inheritdoc/>
        [NotNull]
        public override Type SerializationType => typeof(T);

        /// <inheritdoc/>
        public override bool IsBlittable => false;

        /// <inheritdoc/>
        public override void Serialize(ref object obj, ArchiveMode mode, SerializationStream stream)
        {
            var objT = (obj == null) ? default : (T) obj;
            Serialize(ref objT, mode, stream);
            obj = objT;
        }

        /// <summary>
        ///   Serializes or deserializes the given object.
        /// </summary>
        /// <param name="obj">The object to serialize or deserialize.</param>
        /// <param name="stream">The stream to serialize to or deserialize from.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Serialize(T obj, [NotNull] SerializationStream stream)
        {
            Serialize(ref obj, ArchiveMode.Serialize, stream);
        }

        /// <inheritdoc/>
        public override void PreSerialize(ref object obj, ArchiveMode mode, SerializationStream stream)
        {
            var objT = (obj == null) ? default : (T) obj;
            PreSerialize(ref objT, mode, stream);
            obj = objT;
        }

        /// <summary>
        ///   Performs the first step of serialization or deserialization.
        /// </summary>
        /// <param name="obj">The object to process.</param>
        /// <param name="mode">The serialization mode.</param>
        /// <param name="stream">The stream to serialize to or deserialize from.</param>
        /// <remarks>
        ///   Typically, if <paramref name="obj"/> is <c>null</c> it will instantiate the object. If it is a collection
        ///   it will clear it.
        /// </remarks>
        public virtual void PreSerialize(ref T obj, ArchiveMode mode, SerializationStream stream) { }

        /// <summary>
        ///   Serializes or deserializes the given object.
        /// </summary>
        /// <param name="obj">The object to serialize or deserialize.</param>
        /// <param name="mode">The serialization mode.</param>
        /// <param name="stream">The stream to serialize to or deserialize from.</param>
        public abstract void Serialize(ref T obj, ArchiveMode mode, [NotNull] SerializationStream stream);
    }
}
