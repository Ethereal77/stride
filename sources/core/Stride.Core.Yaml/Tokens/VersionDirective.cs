// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Yaml.Tokens
{
    /// <summary>
    /// Represents a version directive token.
    /// </summary>
    public class VersionDirective : Token
    {
        private readonly Version version;

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public Version Version { get { return version; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionDirective"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        public VersionDirective(Version version)
            : this(version, Mark.Empty, Mark.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionDirective"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="start">The start position of the token.</param>
        /// <param name="end">The end position of the token.</param>
        public VersionDirective(Version version, Mark start, Mark end)
            : base(start, end)
        {
            this.version = version;
        }

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current System.Object.
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current System.Object.</param>
        /// <returns>
        /// true if the specified System.Object is equal to the current System.Object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            VersionDirective other = obj as VersionDirective;
            return other != null && version.Equals(other.version);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return version.GetHashCode();
        }
    }
}