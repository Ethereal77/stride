// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Stride.Core;
using Stride.Data;
using Stride.Core.Assets.Editor.Quantum.NodePresenters;
using Stride.Core.Assets.Editor.Quantum.NodePresenters.Commands;
using Stride.Core.Assets.Editor.Quantum.NodePresenters.Keys;
using Stride.Core.Presentation.Quantum.Presenters;
using Stride.Assets.Presentation.ViewModel;

namespace Stride.Assets.Presentation.NodePresenters.Updaters
{
    internal sealed class GameSettingsAssetNodeUpdater : AssetNodePresenterUpdaterBase
    {
        protected override void UpdateNode(IAssetNodePresenter node)
        {
            if (!(node.Asset is GameSettingsViewModel))
                return;

            if (node.Name == nameof(GameSettingsAsset.Defaults) &&
                node.Value is List<Configuration>)
            {
                foreach (var item in node.Children.ToList())
                {
                    item.Order = node.Order + item.Index.Int;
                    item.AttachedProperties.Add(CategoryData.Key, true);
                }
                if (!node.Commands.Any(cmd => cmd.Name == AddNewItemCommand.CommandName))
                    node.Commands.Add(new AddNewItemCommand());
                if (!node.Commands.Any(cmd => cmd.Name == RemoveItemCommand.CommandName))
                    node.Commands.Add(new RemoveItemCommand());
            }

            if (typeof(Configuration).IsAssignableFrom(node.Type))
            {
                node.DisplayName = DisplayAttribute.GetDisplayName(node.Value?.GetType()) ??
                                   DisplayAttribute.GetDisplayName(typeof(Configuration));
            }

            if (typeof(ConfigurationOverride).IsAssignableFrom(node.Type))
            {
                node.DisplayName = $"Override {node.Index}";
                node.AttachedProperties.Add(CategoryData.Key, true);
            }

            if (node.Parent != null &&
                typeof(ConfigurationOverride).IsAssignableFrom(node.Parent.Type) &&
                node.Name == nameof(ConfigurationOverride.SpecificFilter))
            {
                if (node.Commands.All(x => x.Name != "ClearSelection"))
                {
                    node.Commands.Add(new SyncAnonymousNodePresenterCommand("ClearSelection", (n, x) => n.UpdateValue(-1)));
                }
            }

            if (typeof(ICollection<Configuration>).IsAssignableFrom(node.Type))
            {
                var types = node.AttachedProperties.Get(AbstractNodeEntryData.Key);
                types = types.Where(x =>
                    ((ICollection<Configuration>) node.Value).All(y => !(x is AbstractNodeType &&
                                                                       y.GetType() == ((AbstractNodeType)x).Type)));
                node.AttachedProperties.Set(AbstractNodeEntryData.Key, types);
            }
        }
    }
}
