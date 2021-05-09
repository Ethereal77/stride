// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.MicroThreading
{
    [Flags]
    public enum ScriptFlags
    {
        /// <summary>
        /// Empty value.
        /// </summary>
        None = 0,

        /// <summary>
        /// Automatically run on assembly startup.
        /// </summary>
        AssemblyStartup = 1,

        /// <summary>
        /// Automatically run on assembly first startup (not executed if assembly is reloaded).
        /// </summary>
        AssemblyFirstStartup = 2,

        /// <summary>
        /// Automatically run on assembly unload.
        /// </summary>
        AssemblyUnload = 4,

        // TODO: Not implemented yet
        /// <summary>
        /// MicroThread won't be killed if assembly is unloaded (including reload).
        /// </summary>
        KeepAliveWhenUnload = 8,
    }
}
