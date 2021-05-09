// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Engine.Design
{
    /// <summary>
    ///   Defines the different execution modes for the engine's components.
    /// </summary>
    [Flags]
    public enum ExecutionMode
    {
        /// <summary>
        ///   The component will not execute.
        /// </summary>
        None = 0,

        /// <summary>
        ///   The component will be executed at runtime in the final application / game.
        /// </summary>
        Runtime = 1,

        /// <summary>
        ///   The component will be executed at design time in the editor (Game Studio).
        /// </summary>
        Editor = 2,

        /// <summary>
        ///   The component will be executed at design time in the thumbnail generation system.
        /// </summary>
        Thumbnail = 4,

        /// <summary>
        ///   The component will be executed at design time in a preview window.
        /// </summary>
        Preview = 8,

        /// <summary>
        ///   The component will be executed at design time in the shader / effect compilation system.
        /// </summary>
        EffectCompile = 16,

        /// <summary>
        ///   The component will execute always, in every environment.
        /// </summary>
        All = Runtime | Editor | Thumbnail | Preview | EffectCompile
    }
}
