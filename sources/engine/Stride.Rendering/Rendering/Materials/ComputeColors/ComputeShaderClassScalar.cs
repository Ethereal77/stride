// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;

namespace Stride.Rendering.Materials.ComputeColors
{
    /// <summary>
    /// A shader outputing a color/vector value.
    /// </summary>
    [DataContract("ComputeShaderClassScalar")]
    [Display("Shader")]
    // TODO: This class has been made abstract to be removed from the editor - unabstract it to re-enable it!
    public class ComputeShaderClassScalar : ComputeShaderClassBase<IComputeScalar>, IComputeScalar
    {
    }
}
