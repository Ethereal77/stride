// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core
{
    /// <summary>
    /// Allows to re-map a previous class/field/property/enum name to the specified property/field/enum/class/struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Enum | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public sealed class DataAliasAttribute : Attribute
    {
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAliasAttribute"/> class.
        /// </summary>
        /// <param name="name">The previous name.</param>
        public DataAliasAttribute(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Gets the previous name.
        /// </summary>
        /// <value>The previous name.</value>
        public string Name
        {
            get { return name; }
        }
    }
}
