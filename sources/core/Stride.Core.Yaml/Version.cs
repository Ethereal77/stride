// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Yaml
{
    /// <summary>
    /// Specifies the version of the YAML language.
    /// </summary>
    public class Version
    {
        private readonly int major;

        /// <summary>
        /// Gets the major version number.
        /// </summary>
        public int Major { get { return major; } }

        private readonly int minor;

        /// <summary>
        /// Gets the minor version number.
        /// </summary>
        public int Minor { get { return minor; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        /// <param name="major">The the major version number.</param>
        /// <param name="minor">The the minor version number.</param>
        public Version(int major, int minor)
        {
            this.major = major;
            this.minor = minor;
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
            Version other = obj as Version;
            return other != null && major == other.major && minor == other.minor;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return major.GetHashCode() ^ minor.GetHashCode();
        }
    }
}