// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Assets.Analysis
{
    /// <summary>
    /// Options used when searching asset dependencies.
    /// </summary>
    [Flags]
    public enum AssetDependencySearchOptions
    {
        /// <summary>
        /// Search for <c>in</c> only dependencies.
        /// </summary>
        In = 1,

        /// <summary>
        /// Search for <c>out</c> only dependencies.
        /// </summary>
        Out = 2,

        /// <summary>
        /// Search for <c>in</c> and <c>out</c> dependencies.
        /// </summary>
        InOut = In | Out,

        /// <summary>
        /// Search recursively
        /// </summary>
        Recursive = 4,

        /// <summary>
        /// Search recursively all <c>in</c> and <c>out</c> dependencies.
        /// </summary>
        All = InOut | Recursive
    }
}
