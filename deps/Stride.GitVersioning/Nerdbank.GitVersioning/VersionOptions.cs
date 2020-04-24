// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Nerdbank.GitVersioning
{
    /// <summary>
    ///   Stores the Package version read from a Stride Package (<c>.sdpkg</c>). Implemented for <see cref="GitExtensions"/>.
    /// </summary>
    class VersionOptions
    {
        public int BuildNumberOffset => 0;

        public Version Version { get; set; }
    }
}
