// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.Services;
using Stride.Core.Annotations;
using Stride.Assets.Presentation.ViewModel;
using Stride.Engine;
using Stride.SpriteStudio.Offline;
using Stride.SpriteStudio.Runtime;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels
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
