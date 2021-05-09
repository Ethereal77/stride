// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Diagnostics;

namespace Stride.Core.Assets
{
    /// <summary>
    /// Keys used for profiling the game class.
    /// </summary>
    public static class PackageSessionProfilingKeys
    {
        public static readonly ProfilingKey Session = new ProfilingKey("PackageSession");

        /// <summary>
        /// Profiling load of a session.
        /// </summary>
        public static readonly ProfilingKey Loading = new ProfilingKey(Session, "Load", ProfilingKeyFlags.Log);

        /// <summary>
        /// Profiling save of a session.
        /// </summary>
        public static readonly ProfilingKey Saving = new ProfilingKey(Session, "Save", ProfilingKeyFlags.Log);
    }
}
