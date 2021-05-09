// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;

using Stride.Core.Assets.Editor.Quantum.NodePresenters;
using Stride.Assets.Presentation.NodePresenters.Keys;
using Stride.Assets.SpriteFont;

namespace Stride.Assets.Presentation.NodePresenters.Updaters
{
    internal sealed class SpriteFontAssetNodeUpdater : AssetNodePresenterUpdaterBase
    {
        private static readonly IEnumerable<string> InstalledFonts;

        static SpriteFontAssetNodeUpdater()
        {
            var installedFontCollection = new InstalledFontCollection();
            InstalledFonts = installedFontCollection.Families.Select(x => x.Name).ToArray();
        }

        protected override void UpdateNode(IAssetNodePresenter node)
        {
            // Root node
            if (typeof(SpriteFontAsset).IsAssignableFrom(node.Type))
            {
                node.AttachedProperties.Add(SpriteFontData.Key, InstalledFonts);
            }
        }
    }
}
