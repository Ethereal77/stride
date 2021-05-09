// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Serialization.Contents;

namespace Stride.Core.Serialization
{
    /// <summary>
    ///   Base class for <see cref="IUrlReference" /> implementations.
    /// </summary>
    [DataContract("urlref", Inherited = true)]
    [DataStyle(DataStyle.Compact)]
    [ReferenceSerializer]
    public abstract class UrlReferenceBase : IUrlReference
    {
        private string url;

        /// <summary>
        ///   Initializes a new instance of the class <see cref="UrlReferenceBase"/>.
        /// </summary>
        protected UrlReferenceBase() { }

        /// <summary>
        ///   Initializes a new instance of the class <see cref="UrlReferenceBase"/>.
        /// </summary>
        /// <param name="url">URL to the Asset to reference.</param>
        /// <exception cref="ArgumentNullException"><paramref name="url"/> is <c>null</c> or empty.</exception>
        protected UrlReferenceBase(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url), $"{nameof(url)} cannot be null or empty.");

            this.url = url;
        }

        /// <summary>
        ///   Gets the URL of the referenced Asset.
        /// </summary>
        [DataMember(10)]
        public string Url
        {
            get => url;
            internal set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(nameof(value), $"{nameof(Url)} cannot be null or empty.");

                url = value;
            }
        }

        /// <summary>
        ///   Gets a value that indicates whether the URL is <c>null</c> or empty.
        /// </summary>
        [DataMemberIgnore]
        public bool IsEmpty => string.IsNullOrEmpty(url);

        /// <inheritdoc/>
        public override string ToString() => url;
    }
}
