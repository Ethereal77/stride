// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets;
using Stride.Core.Assets.Editor.Services;
using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Annotations;
using Stride.Assets.Models;
using Stride.Engine;
using Stride.Rendering;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels
{
    internal class AddModelAssetPolicy<TModelAsset> : CreateComponentPolicyBase<TModelAsset, AssetViewModel<TModelAsset>>
        where TModelAsset : Asset, IModelAsset
    {
        /// <inheritdoc />
        [NotNull]
        protected override EntityComponent CreateComponentFromAsset(EntityHierarchyItemViewModel parent, AssetViewModel<TModelAsset> asset)
        {
            return new ModelComponent
            {
                Model = ContentReferenceHelper.CreateReference<Model>(asset)
            };
        }
    }
}
