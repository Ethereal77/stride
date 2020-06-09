// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Core
{
    /// <summary>
    /// Describes the platform operating system.
    /// </summary>
#if STRIDE_ASSEMBLY_PROCESSOR
    // To avoid a CS1503 error when compiling projects that are using both the AssemblyProcessor
    // and Stride.Core.
    internal enum PlatformType
#else
    [DataContract("PlatformType")]
    public enum PlatformType
#endif
    {
        // **************************************************************************************
        // NOTE: This file is shared with the AssemblyProcessor.
        //       If this file is modified, the AssemblyProcessor has to be recompiled separately.
        //       See build\Stride-AssemblyProcessor.sln
        // **************************************************************************************

        /// <summary>
        /// This is shared across platforms
        /// </summary>
        Shared,

        /// <summary>
        /// The Windows desktop OS.
        /// </summary>
        Windows
    }
}
