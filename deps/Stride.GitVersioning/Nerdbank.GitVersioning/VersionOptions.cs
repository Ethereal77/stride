// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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
