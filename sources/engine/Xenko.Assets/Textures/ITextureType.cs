// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Mathematics;
using Xenko.Graphics;

namespace Xenko.Assets.Textures
{
    public interface ITextureType
    {
        bool IsSRgb(ColorSpace colorSpaceReference);

        bool ColorKeyEnabled { get; }

        Color ColorKeyColor { get; }

        AlphaFormat Alpha { get; }

        bool PremultiplyAlpha { get; }

        TextureHint Hint { get; }
    }
}
