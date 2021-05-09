// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering.Materials
{
    /// <summary>
    ///   Defines the interface for a material feature.
    /// </summary>
    public interface IMaterialFeature : IMaterialShaderGenerator
    {
        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref="IMaterialFeature"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool Enabled { get; set; }
    }
}
