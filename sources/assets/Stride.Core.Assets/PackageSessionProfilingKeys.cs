// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Diagnostics;

namespace Xenko.Core.Assets
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
