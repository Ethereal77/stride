// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Assets;

namespace Xenko.Core.Serialization.Contents
{
    /// <summary>
    /// An interface that provides a reference to an object identified by a <see cref="Guid"/> and a location.
    /// </summary>
    public interface IReference
    {
        /// <summary>
        /// Gets the asset unique identifier.
        /// </summary>
        /// <value>The identifier.</value>
        AssetId Id { get; }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        string Location { get; }
    }

    /// <summary>
    /// A typed <see cref="IReference"/>
    /// </summary>
    public interface ITypedReference : IReference
    {
        /// <summary>
        /// Gets the type of this content reference.
        /// </summary>
        /// <value>The type.</value>
        Type Type { get; }
    }
}
