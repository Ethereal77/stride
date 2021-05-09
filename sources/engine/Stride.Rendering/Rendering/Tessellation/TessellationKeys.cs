// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Graphics;

namespace Stride.Rendering.Tessellation
{
    public class TessellationKeys
    {
        /// <summary>
        /// Desired maximum triangle size in screen space during tessellation.
        /// </summary>
        public static readonly ValueParameterKey<float> DesiredTriangleSize = ParameterKeys.NewValue(12.0f);

        /// <summary>
        /// The intensity of the smoothing for PN-based tessellation.
        /// </summary>
        public static readonly ObjectParameterKey<Texture> SmoothingMap = ParameterKeys.NewObject<Texture>();
        public static readonly ValueParameterKey<float> SmoothingValue = ParameterKeys.NewValue<float>(); 
    }
}
