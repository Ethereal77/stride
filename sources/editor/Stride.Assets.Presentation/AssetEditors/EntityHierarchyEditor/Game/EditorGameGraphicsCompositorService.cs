// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;

using Stride.Core.Annotations;
using Stride.Core.Extensions;
using Stride.Core.Serialization;
using Stride.Core.Assets.Editor.ViewModel;
using Stride.Rendering.Compositing;
using Stride.Assets.Presentation.ViewModel;
using Stride.Assets.Presentation.AssetEditors.GameEditor.Services;
using Stride.Assets.Presentation.AssetEditors.GameEditor.ViewModels;
using Stride.Editor.Build;
using Stride.Editor.EditorGame.Game;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Game
{
    public class EditorGameGraphicsCompositorService : EditorGameServiceBase
    {
        private readonly IEditorGameController controller;
        private readonly GameEditorViewModel editor;
        private readonly GameSettingsProviderService settingsProvider;

        private EditorServiceGame game;
        private GraphicsCompositor loadedGraphicsCompositor;
        private GraphicsCompositorViewModel currentGraphicsCompositorAsset;

        public EditorGameGraphicsCompositorService([NotNull] IEditorGameController controller, [NotNull] GameEditorViewModel editor)
        {
            if (controller is null)
                throw new ArgumentNullException(nameof(controller));
            if (editor is null)
                throw new ArgumentNullException(nameof(editor));

            this.controller = controller;
            this.editor = editor;

            settingsProvider = editor.ServiceProvider.Get<GameSettingsProviderService>();
        }

        protected override Task<bool> Initialize(EditorServiceGame editorGame)
        {
            game = editorGame;

            editor.Dispatcher.InvokeAsync(async () =>
            {
                // Be notified of future changes to game settings
                settingsProvider.GameSettingsChanged += GameSettingsChanged;
                editor.Session.AssetPropertiesChanged += AssetPropertyChanged;

                await ReloadGraphicsCompositor(forceIfSame: true);
            });

            return Task.FromResult(true);
        }

        public override ValueTask DisposeAsync()
        {
            settingsProvider.GameSettingsChanged -= GameSettingsChanged;
            editor.Session.AssetPropertiesChanged -= AssetPropertyChanged;

            return base.DisposeAsync();
        }

        private void GameSettingsChanged(object sender, GameSettingsChangedEventArgs e)
        {
            ReloadGraphicsCompositor(forceIfSame: false).Forget();
        }

        private void AssetPropertyChanged(object sender, AssetChangedEventArgs e)
        {
            if (currentGraphicsCompositorAsset != null && e.Assets.Any(x => x.Asset == currentGraphicsCompositorAsset.Asset))
            {
                ReloadGraphicsCompositor(forceIfSame: true).Forget();
            }
        }

        private async Task ReloadGraphicsCompositor(bool forceIfSame)
        {
            var graphicsCompositorId = AttachedReferenceManager.GetAttachedReference(settingsProvider.CurrentGameSettings.GraphicsCompositor)?.Id;
            var graphicsCompositorAsset = (GraphicsCompositorViewModel)(graphicsCompositorId.HasValue ? editor.Session.GetAssetById(graphicsCompositorId.Value) : null);

            // Same compositor as before?
            if (graphicsCompositorAsset == currentGraphicsCompositorAsset && !forceIfSame)
                return;

            // TODO: Start listening for changes in this compositor
            currentGraphicsCompositorAsset = graphicsCompositorAsset;

            // TODO: If nothing, fallback to default compositor, or stop rendering?
            if (graphicsCompositorAsset is null)
                return;

            // TODO: Prevent reentrancy
            var database = editor.ServiceProvider.Get<GameStudioDatabase>();
            await database.Build(graphicsCompositorAsset.AssetItem);

            await controller.InvokeTask(async () =>
            {
                using (await database.MountInCurrentMicroThread())
                {
                    // Unlaod previous graphics compositor
                    if (loadedGraphicsCompositor != null)
                    {
                        game.Content.Unload(loadedGraphicsCompositor);
                        loadedGraphicsCompositor = null;
                    }
                    else
                    {
                        // Should only happen when graphics compositor is fallback one (i.e. first load or failure)
                        game.SceneSystem.GraphicsCompositor?.Dispose();
                    }

                    game.SceneSystem.GraphicsCompositor = null;

                    // Load and set new graphics compositor
                    loadedGraphicsCompositor = game.Content.Load<GraphicsCompositor>(graphicsCompositorAsset.AssetItem.Location);
                    game.UpdateGraphicsCompositor(loadedGraphicsCompositor);
                }
            });
        }
    }
}
