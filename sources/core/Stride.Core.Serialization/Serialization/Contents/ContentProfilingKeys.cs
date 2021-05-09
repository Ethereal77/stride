// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Diagnostics;

namespace Stride.Core.Serialization.Contents
{
    /// <summary>
    /// Keys used for profiling the game class.
    /// </summary>
    public static class ContentProfilingKeys
    {
        public static readonly ProfilingKey Content = new ProfilingKey("Content");

        /// <summary>
        /// Profiling load of an asset.
        /// </summary>
        public static readonly ProfilingKey ContentLoad = new ProfilingKey(Content, "Load", ProfilingKeyFlags.Log);

        /// <summary>
        /// Profiling load of an asset.
        /// </summary>
        public static readonly ProfilingKey ContentReload = new ProfilingKey(Content, "Reload", ProfilingKeyFlags.Log);

        /// <summary>
        /// Profiling save of an asset.
        /// </summary>
        public static readonly ProfilingKey ContentSave = new ProfilingKey(Content, "Save", ProfilingKeyFlags.Log);
    }
}
