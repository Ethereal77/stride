// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Yaml.Events;

namespace Xenko.Core.Yaml
{
    /// <summary>
    /// Represents a YAML stream paser.
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Gets the current event.
        /// </summary>
        ParsingEvent Current { get; }

        /// <summary>
        /// True if end of stream has been reached, false otherwise.
        /// </summary>
        bool IsEndOfStream { get; }

        /// <summary>
        /// Moves to the next event.
        /// </summary>
        /// <returns>Returns true if there are more events available, otherwise returns false.</returns>
        bool MoveNext();
    }
}