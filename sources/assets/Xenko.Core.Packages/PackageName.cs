// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;
using Xenko.Core.Annotations;

namespace Xenko.Core.Packages
{
    /// <summary>
    /// Representation of the name of a package made of an Id and a version.
    /// </summary>
    public sealed class PackageName : IEquatable<PackageName>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PackageName"/>.
        /// </summary>
        /// <param name="id">Id of package.</param>
        /// <param name="version">Version of package.</param>
        public PackageName([NotNull] string id, [NotNull] PackageVersion version)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        /// <summary>
        /// Identity of the package.
        /// </summary>
        [NotNull]
        public string Id { get; }

        /// <summary>
        /// Version of the package.
        /// </summary>
        [NotNull]
        public PackageVersion Version { get; }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (Id.GetHashCode() * 397) ^ Version.GetHashCode();
        }

        /// <inheritdoc />
        public bool Equals(PackageName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Id, other.Id) && Equals(Version, other.Version);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as PackageName);
        }
    }
}
