// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1402 // File may only contain a single type

using System;
using System.Reflection;

using Stride.Core;
using Stride.Core.Serialization;
using Stride.Core.Storage;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents a key by which to identify an effect parameter.
    /// </summary>
    public abstract class ParameterKey : PropertyKey
    {
        public ulong HashCode;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParameterKey" /> class.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="length">Number of elements.</param>
        /// <param name="metadatas">Additional data about the parameter.</param>
        protected ParameterKey(Type propertyType, string name, int length, params PropertyKeyMetadata[] metadatas)
            : base(name, propertyType, null, metadatas)
        {
            Length = length;

            // Cache hashCode for faster lookup (string is immutable)
            // TODO: Make it unique (global dictionary?)
            UpdateName();
        }

        [DataMemberIgnore]
        public new ParameterKeyValueMetadata DefaultValueMetadata { get; private set; }

        /// <summary>
        ///   Gets the number of elements for this key.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        ///   Get the type of the property key.
        /// </summary>
        public ParameterKeyType Type { get; protected set; }

        /// <summary>
        ///   Get the size of the property, in bytes.
        /// </summary>
        public abstract int Size { get; }

        /// <summary>
        ///   Sets the name of the property key.
        /// </summary>
        /// <param name="name">The new name.</param>
        internal void SetName(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            Name = string.Intern(name);
            UpdateName();
        }

        internal void SetOwnerType(Type ownerType)
        {
            OwnerType = ownerType;
        }

        /// <summary>
        ///   Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is ParameterKey parameterKey)
                return Equals(parameterKey.Name, Name);

            return false;
        }

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return (int) HashCode;
        }

        public static bool operator ==(ParameterKey left, ParameterKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ParameterKey left, ParameterKey right)
        {
            return !Equals(left, right);
        }

        //public abstract ParameterKey AppendKeyOverride(object obj);

        private unsafe void UpdateName()
        {
            fixed (char* bufferStart = Name)
            {
                var objectIdBuilder = new ObjectIdBuilder();
                objectIdBuilder.Write((byte*) bufferStart, sizeof(char) * Name.Length);

                var objId = objectIdBuilder.ComputeHash();
                var objIdData = (ulong*) &objId;
                HashCode = objIdData[0] ^ objIdData[1];
            }
        }

        /// <summary>
        ///   Converts a value to the expected value of this parameter key (for example, if value is
        ///   an integer while this parameter key is expecting a float).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public object ConvertValue(object value)
        {
            // If not a value type, return the value as-is
            if (!PropertyType.GetTypeInfo().IsValueType)
            {
                return value;
            }

            if (value != null)
            {
                // If target type is same type, then return the value directly
                if (PropertyType == value.GetType())
                {
                    return value;
                }

                if (PropertyType.GetTypeInfo().IsEnum)
                {
                    value = Enum.Parse(PropertyType, value.ToString());
                }
            }

            // Convert the value to the target type if different
            value = Convert.ChangeType(value, PropertyType);
            return value;
        }

        protected override void SetupMetadata(PropertyKeyMetadata metadata)
        {
            if (metadata is ParameterKeyValueMetadata valueMetadata)
            {
                DefaultValueMetadata = valueMetadata;
            }
            else
            {
                base.SetupMetadata(metadata);
            }
        }

        internal virtual object ReadValue(IntPtr data)
        {
            throw new NotSupportedException("Only implemented for ValueParameterKey.");
        }

        internal abstract void SerializeHash(SerializationStream stream, object value);
    }

    public enum ParameterKeyType
    {
        Value,
        Object,
        Permutation,
    }

    /// <summary>
    ///   Represents a key by which to identify an effect parameter.
    /// </summary>
    /// <typeparam name="T">Type of the parameter key.</typeparam>
    public abstract class ParameterKey<T> : ParameterKey
    {
        private static DataSerializer<T> dataSerializer;
        private static readonly bool isValueType = typeof(T).GetTypeInfo().IsValueType;

        public override bool IsValueType => isValueType;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParameterKey{T}"/> class.
        /// </summary>
        /// <param name="type">The type of the parameter key.</param>
        /// <param name="name">Name of the parameter key.</param>
        /// <param name="length">Number of elements of the parameter key.</param>
        /// <param name="metadata">Additional data for the parameter.</param>
        protected ParameterKey(ParameterKeyType type, string name, int length, PropertyKeyMetadata metadata)
            : this(type, name, length, new[] { metadata })
        { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ParameterKey{T}"/> class.
        /// </summary>
        /// <param name="type">The type of the parameter key.</param>
        /// <param name="name">Name of the parameter key.</param>
        /// <param name="length">Number of elements of the parameter key.</param>
        /// <param name="metadatas">Additional data for the parameter.</param>
        protected ParameterKey(ParameterKeyType type, string name, int length = 1, params PropertyKeyMetadata[] metadatas)
            : base(typeof(T), name, length, metadatas.Length > 0 ? metadatas : new PropertyKeyMetadata[] { new ParameterKeyValueMetadata<T>() })
        {
            Type = type;
        }

        [DataMemberIgnore]
        public ParameterKeyValueMetadata<T> DefaultValueMetadataT { get; private set; }

        public override int Size => Interop.SizeOf<T>();

        public override string ToString() => Name;

        internal override void SerializeHash(SerializationStream stream, object value)
        {
            var currentDataSerializer = dataSerializer;
            if (currentDataSerializer is null)
            {
                dataSerializer = currentDataSerializer = MemberSerializer<T>.Create(stream.Context.SerializerSelector);
            }

            currentDataSerializer.Serialize(ref value, ArchiveMode.Serialize, stream);
        }

        protected override void SetupMetadata(PropertyKeyMetadata metadata)
        {
            if (metadata is ParameterKeyValueMetadata<T> valueMetadata)
            {
                DefaultValueMetadataT = valueMetadata;
            }
            // Run the always base as ParameterKeyValueMetadata<T> is also ParameterKeyValueMetadata used by the base
            base.SetupMetadata(metadata);
        }

        internal override PropertyContainer.ValueHolder CreateValueHolder(object value)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///   Represents a key by which to identify an effect value parameter, usually for use by shaders (.SDSL).
    /// </summary>
    /// <typeparam name="T">Type of the parameter key.</typeparam>
    [DataSerializer(typeof(ValueParameterKeySerializer<>), Mode = DataSerializerGenericMode.GenericArguments)]
    public sealed class ValueParameterKey<T> : ParameterKey<T> where T : struct
    {
        public ValueParameterKey(string name, int length, PropertyKeyMetadata metadata)
            : base(ParameterKeyType.Value, name, length, metadata)
        { }

        public ValueParameterKey(string name, int length = 1, params PropertyKeyMetadata[] metadatas)
            : base(ParameterKeyType.Value, name, length, metadatas)
        { }

        internal override object ReadValue(IntPtr data)
        {
            return Utilities.Read<T>(data);
        }
    }

    /// <summary>
    ///   Represents a key by which to identify an effect object (or boxed value) parameter, usually for use by shaders (.SDSL).
    /// </summary>
    /// <typeparam name="T">Type of the parameter key.</typeparam>
    [DataSerializer(typeof(ObjectParameterKeySerializer<>), Mode = DataSerializerGenericMode.GenericArguments)]
    public sealed class ObjectParameterKey<T> : ParameterKey<T>
    {
        public ObjectParameterKey(string name, int length, PropertyKeyMetadata metadata)
            : base(ParameterKeyType.Object, name, length, metadata)
        { }

        public ObjectParameterKey(string name, int length = 1, params PropertyKeyMetadata[] metadatas)
            : base(ParameterKeyType.Object, name, length, metadatas)
        { }
    }

    /// <summary>
    ///   Represents a key by which to identify an effect permutation, usually for use by effects (.SDFX).
    /// </summary>
    /// <typeparam name="T">Type of the parameter key.</typeparam>
    [DataSerializer(typeof(PermutationParameterKeySerializer<>), Mode = DataSerializerGenericMode.GenericArguments)]
    public sealed class PermutationParameterKey<T> : ParameterKey<T>
    {
        public PermutationParameterKey(string name, int length, PropertyKeyMetadata metadata)
            : base(ParameterKeyType.Permutation, name, length, metadata)
        { }

        public PermutationParameterKey(string name, int length = 1, params PropertyKeyMetadata[] metadatas)
            : base(ParameterKeyType.Permutation, name, length, metadatas)
        { }
    }
}
