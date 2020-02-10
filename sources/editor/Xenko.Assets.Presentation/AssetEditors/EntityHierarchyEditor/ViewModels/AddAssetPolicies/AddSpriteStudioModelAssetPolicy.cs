// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.Annotations;
using Xenko.Assets.Presentation.ViewModel;
using Xenko.Engine;
using Xenko.SpriteStudio.Offline;
using Xenko.SpriteStudio.Runtime;

namespace Xenko.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels
{
    internal class AddSpriteStudioModelAssetPolicy : CreateComponentPolicyBase<SpriteStudioModelAsset, SpriteStudioModelViewModel>
    {
        /// <inheritdoc />
        [NotNull]
        protected override EntityComponent CreateComponentFromAsset(EntityHierarchyItemViewModel parent, SpriteStudioModelViewModel asset)
        {
            return new SpriteStudioComponent
            {
                Sheet = ContentReferenceHelper.CreateReference<SpriteStudioSheet>(asset),
            };
        }
    }
}
