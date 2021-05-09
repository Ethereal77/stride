// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Assets;

namespace Stride.Core
{
    /// <summary>
    ///   Represents the absolute identifier of an identifiable object in an Asset.
    /// </summary>
    [DataContract("AbsoluteId")]
    public struct AbsoluteId : IEquatable<AbsoluteId>
    {
        /// <summary>
        ///   Gets the identifier of the containing Asset.
        /// </summary>
        public AssetId AssetId { get; }

        /// <summary>
        ///   Gets the identifier of the object in the Asset.
        /// </summary>
        public Guid ObjectId { get; }


        /// <summary>
        ///   Initializes a new instance of <see cref="AbsoluteId"/>.
        /// </summary>
        /// <param name="assetId">Identifier of the Asset.</param>
        /// <param name="objectId">Identifier of the object.</param>
        /// <exception cref="ArgumentException"><paramref name="assetId"/> and <paramref name="objectId"/> cannot both be empty.</exception>
        public AbsoluteId(AssetId assetId, Guid objectId)
        {
            if (assetId == AssetId.Empty && objectId == Guid.Empty)
                throw new ArgumentException($"{nameof(assetId)} and {nameof(objectId)} cannot both be empty.");

            AssetId = assetId;
            ObjectId = objectId;
        }


        public static bool operator ==(AbsoluteId left, AbsoluteId right) => left.Equals(right);

        public static bool operator !=(AbsoluteId left, AbsoluteId right) => !left.Equals(right);

        /// <inheritdoc />
        public bool Equals(AbsoluteId other)
        {
            return AssetId.Equals(other.AssetId) &&
                   ObjectId.Equals(other.ObjectId);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is AbsoluteId id && Equals(id);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (AssetId.GetHashCode() * 397) ^ ObjectId.GetHashCode();
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"{AssetId}/{ObjectId}";
    }
}
