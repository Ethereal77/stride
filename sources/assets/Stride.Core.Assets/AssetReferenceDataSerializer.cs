// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Serialization;

namespace Stride.Core.Assets
{
    /// <summary>
    /// Serializer for <see cref="AssetReference"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class AssetReferenceDataSerializer : DataSerializer<AssetReference>
    {
        /// <inheritdoc/>
        public override void Serialize(ref AssetReference assetReference, ArchiveMode mode, SerializationStream stream)
        {
            if (mode == ArchiveMode.Serialize)
            {
                stream.Write(assetReference.Id);
                stream.Write(assetReference.Location);
            }
            else
            {
                var id = stream.Read<AssetId>();
                var location = stream.ReadString();

                assetReference = new AssetReference(id, location);
            }
        }
    }
}
