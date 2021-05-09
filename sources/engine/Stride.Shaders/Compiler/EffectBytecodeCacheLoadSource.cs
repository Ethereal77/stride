// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Shaders.Compiler
{
    /// <summary>
    /// Describes what kind of cache (if any) was used to retrieve the bytecode.
    /// </summary>
    public enum EffectBytecodeCacheLoadSource
    {
        /// <summary>
        /// The bytecode has just been compiled.
        /// </summary>
        JustCompiled = 0,

        /// <summary>
        /// The bytecode was loaded through the startup cache (part of asset compilation).
        /// </summary>
        StartupCache = 1,

        /// <summary>
        /// The bytecode was loaded through the runtime cache (not part of asset compilation).
        /// </summary>
        DynamicCache = 2,
    }
}
