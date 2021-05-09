// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    /// Holds state that is used when emitting a stream.
    /// </summary>
    internal class EmitterState
    {
        private readonly HashSet<string> emittedAnchors = new HashSet<string>();

        /// <summary>
        /// Gets the already emitted anchors.
        /// </summary>
        /// <value>The emitted anchors.</value>
        public HashSet<string> EmittedAnchors { get { return emittedAnchors; } }
    }
}