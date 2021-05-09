// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Rendering.Shadows
{
    /// <summary>
    /// Keys used for shadow mapping.
    /// </summary>
    public static partial class ShadowMapKeys
    {
        /// <summary>
        /// Final shadow map texture.
        /// </summary>
        public static readonly ObjectParameterKey<Texture> ShadowMapTexture = ParameterKeys.NewObject<Texture>();
        
        /// <summary>
        /// Final shadow map texture size
        /// </summary>
        public static readonly ValueParameterKey<Vector2> TextureSize = ParameterKeys.NewValue<Vector2>();

        /// <summary>
        /// Final shadow map texture texel size.
        /// </summary>
        public static readonly ValueParameterKey<Vector2> TextureTexelSize = ParameterKeys.NewValue<Vector2>();
    }
}
