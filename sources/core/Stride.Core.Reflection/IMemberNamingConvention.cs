// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Yaml.Serialization
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