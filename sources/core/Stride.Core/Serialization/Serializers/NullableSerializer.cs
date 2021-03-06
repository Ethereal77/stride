// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Serialization.Serializers
{
    /// <summary>
    /// Data serializer for Nullable{T}.
    /// </summary>
    /// <typeparam name="T">The generic type in Nullable{T}.</typeparam>
    [DataSerializerGlobal(typeof(NullableSerializer<>), typeof(Nullable<>), DataSerializerGenericMode.GenericArguments)]
    public class NullableSerializer<T> : DataSerializer<T?> where T : struct
    {
        private DataSerializer<T> itemSerializer;

        /// <inheritdoc/>
        public override void Initialize(SerializerSelector serializerSelector)
        {
            itemSerializer = serializerSelector.GetSerializer<T>();
        }

        /// <inheritdoc/>
        public override void Serialize(ref T? obj, ArchiveMode mode, SerializationStream stream)
        {
            if (mode == ArchiveMode.Serialize)
            {
                var hasValue = obj.HasValue;
                stream.Serialize(ref hasValue);
                if (obj.HasValue)
                {
                    var value = obj.Value;
                    itemSerializer.Serialize(ref value, mode, stream);
                }
            }
            else if (mode == ArchiveMode.Deserialize)
            {
                var hasValue = false;
                stream.Serialize(ref hasValue);
                if (hasValue)
                {
                    var value = default(T);
                    itemSerializer.Serialize(ref value, mode, stream);
                    obj = value;
                }
                else
                {
                    obj = null;
                }
            }
        }
    }
}
