// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering
{
    /// <summary>
    /// Describes the state of a <see cref="RenderEffect"/>.
    /// </summary>
    public enum RenderEffectState
    {
        /// <summary>
        /// The effect is in normal state.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// The effect is being asynchrounously compiled.
        /// </summary>
        Compiling = 1,
        
        /// <summary>
        /// There was an error while compiling the effect.
        /// </summary>
        Error = 2,

        /// <summary>
        /// The effect is skipped.
        /// </summary>
        Skip = 3,
    }
}
