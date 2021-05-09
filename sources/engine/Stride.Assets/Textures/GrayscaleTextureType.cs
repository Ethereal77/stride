// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Assets.Textures
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
