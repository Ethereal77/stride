// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Stride.Core.Annotations;
using Stride.Core.Extensions;
using Stride.Core.Assets.Analysis;
using Stride.Core.Assets.Editor.ViewModel;
using Stride.Assets.Presentation.AssetEditors.AssetHighlighters;
using Stride.Assets.Presentation.AssetEditors.GameEditor.Services;
using Stride.Editor.EditorGame.Game;

namespace Stride.Assets.Presentation.AssetEditors.GameEditor.Game
{
    public class EditorGameAssetHighlighterService : EditorGameServiceBase, IEditorGameAssetHighlighterViewModelService
    {
        private readonly IEditorGameController controller;
        private readonly Dictionary<Type, AssetHighlighter> assetHighlighters = new Dictionary<Type, AssetHighlighter>();
        private EditorServiceGame game;

        public EditorGameAssetHighlighterService(IEditorGameController controller, [NotNull] IAssetDependencyManager dependencyManager)
        {
            this.controller = controller;
            foreach (var assetHighlighterType in StrideDefaultAssetsPlugin.AssetHighlighterTypesDictionary)
            {
                var instance = (AssetHighlighter)Activator.CreateInstance(assetHighlighterType.Value, dependencyManager);
                assetHighlighters.Add(assetHighlighterType.Key, instance);
            }
        }

        /// <inheritdoc/>
        public override ValueTask DisposeAsync()
        {
            EnsureNotDestroyed(nameof(EditorGameAssetHighlighterService));
            assetHighlighters.Select(x => x.Value).ForEach(x => x.Clear());

            return base.DisposeAsync();
        }

        public void HighlightAssets(IEnumerable<AssetViewModel> assets)
        {
            const float duration = 1.0f;

            var assetItems = assets.Select(x => x.AssetItem).ToList();
            controller.InvokeAsync(() =>
            {
                assetHighlighters.Select(x => x.Value).ForEach(x => x.Clear());

                foreach (var assetItem in assetItems)
                {
                    if (assetHighlighters.TryGetValue(assetItem.Asset.GetType(), out AssetHighlighter highlighter))
                    {
                        highlighter.Highlight(controller, game, assetItem, duration);
                    }
                }
            });
        }

        protected override Task<bool> Initialize(EditorServiceGame editorGame)
        {
            game = editorGame;

            return Task.FromResult(true);
        }
    }
}
