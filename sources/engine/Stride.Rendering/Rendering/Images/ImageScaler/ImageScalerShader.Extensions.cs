// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Mathematics;

namespace Stride.Rendering.Images
{
    /// <summary>
    /// Defines default values.
    /// </summary>
    internal partial class ImageScalerShaderKeys
    {
        static ImageScalerShaderKeys()
        {
            // Default value of 1.0f
            Color = ParameterKeys.NewValue(Color4.White);
        }
    }
}
