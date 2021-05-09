// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Serialization;

namespace Stride.Shaders
{
    /// <summary>
    /// Enum to specify shader stage.
    /// </summary>
    [DataContract]
    public enum ShaderStage
    {
        /// <summary>
        /// No shader stage defined.
        /// </summary>
        None = 0,

        /// <summary>
        /// The vertex shader stage.
        /// </summary>
        Vertex = 1,

        /// <summary>
        /// The Hull shader stage.
        /// </summary>
        Hull = 2,

        /// <summary>
        /// The domain shader stage.
        /// </summary>
        Domain = 3,

        /// <summary>
        /// The geometry shader stage.
        /// </summary>
        Geometry = 4,

        /// <summary>
        /// The pixel shader stage.
        /// </summary>
        Pixel = 5,

        /// <summary>
        /// The compute shader stage.
        /// </summary>
        Compute = 6,
    }
}
