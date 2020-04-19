// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets;
using Xenko.Assets.Entities;
using Xenko.UI.Panels;

namespace Xenko.Assets.UI
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
