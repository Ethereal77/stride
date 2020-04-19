// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;

namespace Xenko.Rendering
{
    /// <summary>
    /// Flags describing state of a <see cref="ModelNodeDefinition"/>.
    /// </summary>
    [Flags]
    [DataContract]
    public enum ModelNodeFlags
    {
        /// <summary>
        /// If true, <see cref="ModelNodeTransformation.Transform"/> will be used to update <see cref="ModelNodeTransformation.LocalMatrix"/>.
        /// </summary>
        EnableTransform = 1,

        /// <summary>
        /// If true, associated <see cref="Mesh"/> will be rendered.
        /// </summary>
        EnableRender = 2,

        /// <summary>
        /// Used by the physics engine to override the world matrix transform
        /// </summary>
        OverrideWorldMatrix = 4,

        /// <summary>
        /// The default flags.
        /// </summary>
        Default = EnableTransform | EnableRender,       
    }
}
