// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Stride.Rendering;
using Stride.Rendering.Compositing;
using Stride.Assets.Presentation.SceneEditor;
using Stride.Assets.Presentation.AssetEditors.GameEditor.Game;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Services;
using Stride.Editor.EditorGame.Game;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Game
{
    public class EditorGameRenderModeService : EditorGameServiceBase, IEditorGameRenderModeService, IEditorGameRenderModeViewModelService
    {
        private EntityHierarchyEditorGame game;
        private MaterialFilterRenderFeature materialFilterRenderFeature;

        // Decides the state of savedCameras (true: Game preview, false: Normal rendering)
        private bool isPreviewMode;
        private List<SceneCameraSlot> savedCameras = new();

        public EditorRenderMode RenderMode { get; set; } = EditorRenderMode.DefaultEditor;

        private readonly HashSet<IEditorGameMouseService> disabledMouseServicesDuringGamePreview = new HashSet<IEditorGameMouseService>();

        public override IEnumerable<Type> Dependencies { get { yield return typeof(IEditorGameMouseService); } }

        /// <inheritdoc/>
        public override ValueTask DisposeAsync()
        {
            EnsureNotDestroyed(nameof(EditorGameRenderModeService));

            return base.DisposeAsync();
        }

        protected override Task<bool> Initialize(EditorServiceGame editorGame)
        {
            game = (EntityHierarchyEditorGame) editorGame;
            game.Script.AddTask(Update);

            return Task.FromResult(true);
        }

        public override void UpdateGraphicsCompositor(EditorServiceGame game)
        {
            base.UpdateGraphicsCompositor(game);

            // Make a copy of cameras (for game preview)
            savedCameras.Clear();
            savedCameras.AddRange(game.SceneSystem.GraphicsCompositor.Cameras);
            // Saved camera means we are not yet in preview mode
            isPreviewMode = false;

            // Make sure it is null if nothing found
            materialFilterRenderFeature = null;

            // Meshes
            var meshRenderFeature = game.SceneSystem.GraphicsCompositor.RenderFeatures.OfType<MeshRenderFeature>().First();
            if (meshRenderFeature != null)
            {
                // Add material filtering
                meshRenderFeature.RenderFeatures.Add(materialFilterRenderFeature = new MaterialFilterRenderFeature());
            }
        }

        private async Task Update()
        {
            while (!IsDisposed)
            {
                await game.Script.NextFrame();

                if (!IsActive)
                    continue;

                var renderMode = ((IEditorGameRenderModeViewModelService) this).RenderMode;

                // Toggle graphics compositor to display scene using either editor or game graphics compositor
                var previewGameGraphicsCompositor = renderMode.PreviewGameGraphicsCompositor;
                if (game.SceneSystem.GraphicsCompositor.Game is EditorTopLevelCompositor editorTopLevelCompositor)
                {
                    // Enable preview game if requested
                    editorTopLevelCompositor.EnablePreviewGame = previewGameGraphicsCompositor;
                    // Disable Gizmo during preview game
                    game.EditorSceneSystem.GraphicsCompositor.Game.Enabled = !previewGameGraphicsCompositor;

                    if (editorTopLevelCompositor.EnablePreviewGame != isPreviewMode)
                    {
                        // Swap cameras collection content between game and savedCameras
                        var tempCameras = new List<SceneCameraSlot>(game.SceneSystem.GraphicsCompositor.Cameras);

                        game.SceneSystem.GraphicsCompositor.Cameras.Clear();
                        game.SceneSystem.GraphicsCompositor.Cameras.AddRange(savedCameras);

                        savedCameras.Clear();
                        savedCameras = tempCameras;

                        isPreviewMode = editorTopLevelCompositor.EnablePreviewGame;
                    }

                    // Setup material filter
                    materialFilterRenderFeature.MaterialFilter =
                        (materialFilterRenderFeature != null && renderMode.Mode == GameEditor.RenderMode.SingleStream)
                            ? renderMode.StreamDescriptor.Filter
                            : null;

                    // Disable mouse services while we are in game preview, and reenable them after
                    // TODO: A more robust mechanism for filtering or redirecting input?
                    if (previewGameGraphicsCompositor)
                    {
                        // Disable any mouse services that were enabled
                        foreach (var service in game.EditorServices.Services.OfType<IEditorGameMouseService>())
                        {
                            if (service.IsActive && disabledMouseServicesDuringGamePreview.Add(service))
                                service.IsActive = false;
                        }
                    }
                    else if (disabledMouseServicesDuringGamePreview.Count > 0)
                    {
                        // Need to restore some mouse services
                        foreach (var service in disabledMouseServicesDuringGamePreview)
                        {
                            service.IsActive = true;
                        }
                        disabledMouseServicesDuringGamePreview.Clear();
                    }
                }
            }
        }
    }
}
