// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Graphics;
using Stride.Shaders.Compiler;

namespace Stride.Rendering
{
    /// <summary>
    /// Extensions for <see cref="EffectSystem"/>
    /// </summary>
    public static class EffectSystemExtensions
    {
        /// <summary>
        /// Creates an effect.
        /// </summary>
        /// <param name="effectSystem">The effect system.</param>
        /// <param name="effectName">Name of the effect.</param>
        /// <returns>A new instance of an effect.</returns>
        public static TaskOrResult<Effect> LoadEffect(this EffectSystem effectSystem, string effectName)
        {
            var compilerParameters = new CompilerParameters();
            return effectSystem.LoadEffect(effectName, compilerParameters);
        }
    }
}
