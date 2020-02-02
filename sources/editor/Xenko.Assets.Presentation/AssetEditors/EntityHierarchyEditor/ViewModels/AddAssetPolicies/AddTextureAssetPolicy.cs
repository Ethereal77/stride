// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Core.Annotations;
using Xenko.Assets.Textures;
using Xenko.Engine;
using Xenko.Graphics;
using Xenko.Rendering.Sprites;

namespace Xenko.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels
{
    internal class AddTextureAssetPolicy : CreateComponentPolicyBase<TextureAsset, AssetViewModel<TextureAsset>>
    {
        /// <inheritdoc />
        [NotNull]
        protected override EntityComponent CreateComponentFromAsset(EntityHierarchyItemViewModel parent, AssetViewModel<TextureAsset> asset)
        {
            return new SpriteComponent
            {
                SpriteProvider = new SpriteFromTexture
                {
                    Texture = ContentReferenceHelper.CreateReference<Texture>(asset)
                }
            };
        }
    }
}
