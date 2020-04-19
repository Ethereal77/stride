// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Graphics;

namespace Xenko.Assets.Textures
{
    /// <summary>
    /// A single channel texture which can be used for luminance, height map, specular texture, etc.
    /// </summary>
    /// <userdoc>
    /// A single channel texture which can be used for luminance, height map, specular texture, etc.
    /// </userdoc>
    [DataContract("GrayscaleTextureType")]
    [Display("Grayscale")]
    public class GrayscaleTextureType : ITextureType
    {
        public bool IsSRgb(ColorSpace colorSpaceReference) => false;

        bool ITextureType.ColorKeyEnabled => false;

        Color ITextureType.ColorKeyColor => new Color();

        AlphaFormat ITextureType.Alpha => AlphaFormat.None;

        bool ITextureType.PremultiplyAlpha => false;

        TextureHint ITextureType.Hint => TextureHint.Grayscale;
    }
}
