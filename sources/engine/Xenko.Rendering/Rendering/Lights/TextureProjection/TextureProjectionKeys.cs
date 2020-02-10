// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Mathematics;
using Xenko.Graphics;

namespace Xenko.Rendering.Lights
{
    /// <summary>
    /// Keys used for texture projection by lights.
    /// </summary>
    public static class TextureProjectionKeys
    {
        /// <summary>
        /// Key for the texture to project.
        /// </summary>
        public static readonly ObjectParameterKey<Texture> ProjectionTexture = ParameterKeys.NewObject<Texture>();

        /// <summary>
        /// Key for the texture to project.
        /// </summary>
        public static readonly ValueParameterKey<Vector2> UVScale = ParameterKeys.NewValue<Vector2>();

        /// <summary>
        /// Key for the texture to project.
        /// </summary>
        public static readonly ValueParameterKey<Vector2> UVOffset = ParameterKeys.NewValue<Vector2>();
    }
}
