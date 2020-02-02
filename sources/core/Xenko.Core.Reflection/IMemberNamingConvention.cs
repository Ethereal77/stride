// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Yaml.Serialization
{
    /// <summary>
    /// Base interface for renaming members.
    /// </summary>
    public interface IMemberNamingConvention
    {
        /// <summary>
        /// Gets the comparer used for this member name.
        /// </summary>
        /// <value>The comparer.</value>
        StringComparer Comparer { get; }

        /// <summary>
        /// Converts the specified member name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        string Convert(string name);
    }
}