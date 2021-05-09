// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.BuildEngine;
using Stride.Rendering;

namespace Stride.Assets.Models
{
    /// <summary>
    /// Apply various modification to a <see cref="Model"/> during compilation of a <see cref="ModelAsset"/>.
    /// </summary>
    public interface IModelModifier
    {
        /// <summary>
        /// Used for hashing.
        /// </summary>
        int Version { get; }

        /// <summary>
        /// Apply the modifications to the model.
        /// </summary>
        /// <param name="commandContext"></param>
        /// <param name="model"></param>
        void Apply(ICommandContext commandContext, Model model);
    }
}
