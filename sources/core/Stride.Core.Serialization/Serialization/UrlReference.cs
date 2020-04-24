// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Serialization.Serializers;

namespace Stride.Core.Serialization
{
    /// <summary>
    ///   Represents an URL to an Asset.
    /// </summary>
    [DataSerializer(typeof(UrlReferenceDataSerializer))]
    public sealed class UrlReference : UrlReferenceBase
    {
        /// <summary>
        ///   Initializes a new instance of the class <see cref="UrlReference"/>.
        /// </summary>
        public UrlReference() { }

        /// <summary>
        ///   Initializes a new instance of the class <see cref="UrlReference"/>.
        /// </summary>
        /// <param name="url">The URL to the Asset.</param>
        /// <exception cref="ArgumentNullException"><paramref name="url"/> is <c>null</c> or empty.</exception>
        public UrlReference(string url) : base(url)
        { }
    }

    /// <summary>
    ///   Represents an URL to an Asset of a type specified by <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the Asset referenced by this class.</typeparam>
    [DataSerializer(typeof(UrlReferenceDataSerializer<>), Mode = DataSerializerGenericMode.GenericArguments)]
    public sealed class UrlReference<T> : UrlReferenceBase
        where T : class
    {
        /// <summary>
        ///   Initializes a new instance of the class <see cref="UrlReference{T}"/>.
        /// </summary>
        public UrlReference() { }

        /// <summary>
        ///   Initializes a new instance of the class <see cref="UrlReference{T}"/>.
        /// </summary>
        /// <param name="url">The URL to the Asset.</param>
        /// <exception cref="ArgumentNullException"><paramref name="url"/> is <c>null</c> or empty.</exception>
        public UrlReference(string url) : base(url)
        { }
    }
}
