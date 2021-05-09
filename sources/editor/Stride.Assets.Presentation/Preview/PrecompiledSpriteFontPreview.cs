// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Assets.Presentation.Preview.Views;
using Stride.Assets.SpriteFont;
using Stride.Editor.Preview;

namespace Stride.Assets.Presentation.Preview
{
    /// <summary>
    /// An implementation of the <see cref="AssetPreview"/> that can preview precompiled sprite fonts.
    /// </summary>
    [AssetPreview(typeof(PrecompiledSpriteFontAsset), typeof(SpriteFontPreviewView))]
    public class PrecompiledSpriteFontPreview : FontPreview<PrecompiledSpriteFontAsset>
    {
        protected override bool IsFontNotPremultiplied()
        {
            return !Asset.IsPremultiplied;
        }
    }
}
