// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Annotations;
using Stride.Core.Assets;

namespace Stride.Core.Serialization.Serializers
{
    /// <summary>
    ///   Serializer base class for <see cref="UrlReference"/>.
    /// </summary>
    public abstract class UrlReferenceDataSerializerBase<T> : DataSerializer<T>
        where T: IUrlReference
    {
        /// <inheritdoc/>
        public override void Serialize(ref T urlReference, ArchiveMode mode, [NotNull] SerializationStream stream)
        {
            if (mode == ArchiveMode.Serialize)
            {
                var attachedReference = AttachedReferenceManager.GetAttachedReference(urlReference);
                if(attachedReference is null)
                {
                    throw new InvalidOperationException("UrlReference does not have an AttachedReference.");
                }

                stream.Write(attachedReference.Id);
                stream.Write(attachedReference.Url);
            }
            else
            {
                var id = stream.Read<AssetId>();
                var url = stream.ReadString();

                urlReference = (T)UrlReferenceHelper.CreateReference(typeof(T), id, url);
            }
        }
    }

    /// <summary>
    ///   Serializer for <see cref="UrlReference"/>.
    /// </summary>
    public sealed class UrlReferenceDataSerializer : UrlReferenceDataSerializerBase<UrlReference>
    { }

    /// <summary>
    ///   Serializer for <see cref="UrlReference{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of Asset referenced by this class.</typeparam>
    public sealed class UrlReferenceDataSerializer<T> : UrlReferenceDataSerializerBase<UrlReference<T>>
        where T : class
    { }
}
