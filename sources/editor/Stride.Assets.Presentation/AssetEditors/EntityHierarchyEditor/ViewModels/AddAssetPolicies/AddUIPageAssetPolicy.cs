// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.Services;
using Stride.Core.Annotations;
using Stride.Assets.UI;
using Stride.Assets.Presentation.ViewModel;
using Stride.Engine;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels
{
    internal class AddUIPageAssetPolicy : CreateComponentPolicyBase<UIPageAsset, UIPageViewModel>
    {
        /// <inheritdoc />
        [NotNull]
        protected override EntityComponent CreateComponentFromAsset(EntityHierarchyItemViewModel parent, UIPageViewModel asset)
        {
            return new UIComponent
            {
                Page = ContentReferenceHelper.CreateReference<UIPage>(asset)
            };
        }
    }
}
