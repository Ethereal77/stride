// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets;
using Stride.Assets.Entities;
using Stride.UI.Panels;

namespace Stride.Assets.UI
{
    internal class UIPageFactory : AssetFactory<UIPageAsset>
    {
        public static UIPageAsset Create()
        {
            var grid = new Grid();

            return new UIPageAsset
            {
                Hierarchy = { RootParts = { grid }, Parts = { new UIElementDesign(grid) } }
            };
        }

        public override UIPageAsset New() => Create();
    }
}
