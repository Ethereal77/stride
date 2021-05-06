// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using Stride.Core.Collections;
using Stride.Core.Mathematics;
using Stride.Core.Serialization;
using Stride.Core.Quantum;
using Stride.Core.Assets;
using Stride.Core.Assets.Editor.ViewModel;
using Stride.Assets.Entities;
using Stride.Assets.Navigation;
using Stride.Graphics;
using Stride.Graphics.GeometricPrimitives;
using Stride.Rendering;
using Stride.Rendering.Materials;
using Stride.Rendering.Materials.ComputeColors;
using Stride.Games;
using Stride.Navigation;
using Stride.Engine;
using Stride.Extensions;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Services;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels;
using Stride.Assets.Presentation.AssetEditors.SceneEditor.Services;
using Stride.Assets.Presentation.AssetEditors.SceneEditor.Game;
using Stride.Editor.Build;
using Stride.Editor.EditorGame.Game;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Game
{
    /// <summary>
    ///   Editor service that handles rendering of navigation meshes associated with the current scene.
    /// </summary>
    public class EditorGameNavigationMeshService : EditorGameServiceBase, IEditorGameNavigationViewModelService
    {
        private const float LayerHeightMultiplier = 0.05f;

        private readonly NavigationMeshManager navigationMeshManager;

        private readonly EntityHierarchyEditorViewModel editor;

        private readonly Dictionary<NavigationMesh, NavigationMeshDebugVisual> debugVisuals = new Dictionary<NavigationMesh, NavigationMeshDebugVisual>();

        private readonly Dictionary<Guid, NavigationMeshDisplayGroup> groupDisplaySettings = new Dictionary<Guid, NavigationMeshDisplayGroup>();

        private readonly Dictionary<AssetId, AssetViewModel> navigationMeshAssets = new Dictionary<AssetId, AssetViewModel>();

        private readonly Dictionary<AssetId, Scene> loadedScenes = new Dictionary<AssetId, Scene>();

        private DynamicNavigationMeshSystem dynamicNavigationMeshSystem;

        // The currently displayed dynamic navigation mesh
        private NavigationMesh dynamicNavigationMesh;

        private SceneEditorGame game;

        // Root debug entity, which will have child entities attached to it for every debug element
        private Entity rootDebugEntity;

        private bool visibility = true;

        private SceneEditorController sceneEditorController;
        private GameSettingsProviderService gameSettingsProviderService;

        public EditorGameNavigationMeshService(EntityHierarchyEditorViewModel editor)
        {
            this.editor = editor;
            navigationMeshManager = new NavigationMeshManager(editor.Controller);
        }

        public override async ValueTask DisposeAsync()
        {
            // Remove registered events
            editor.Session.DeletedAssetsChanged -= OnDeletedAssetsChanged;
            editor.Session.AssetPropertiesChanged -= OnAssetPropertiesChanged;

            game.SceneAdded -= GameOnSceneAdded;
            game.SceneRemoved -= GameOnSceneRemoved;

            gameSettingsProviderService.GameSettingsChanged -= GameSettingsProviderServiceOnGameSettingsChanged;

            foreach (var debugVisual in debugVisuals)
            {
                debugVisual.Value.Dispose();
            }

            game.EditorScene.Entities.Remove(rootDebugEntity);

            await navigationMeshManager.DisposeAsync();
            await base.DisposeAsync();
        }

        /// <summary>
        /// Is navigation visualization enabled
        /// </summary>
        public override bool IsActive
        {
            get { return visibility; }
            set
            {
                visibility = value;
                ToggleVisiblity(value);
            }
        }

        /// <summary>
        /// Updates which navigation mesh groups should be displayed and how (color, name, layer height, initial visibility)
        /// </summary>
        public void UpdateGroups(IList<EditorNavigationGroupViewModel> groups)
        {
            // Extract the information from the view model
            List<NavigationMeshDisplayGroup> newDisplayGroups = new List<NavigationMeshDisplayGroup>();
            foreach (var group in groups)
            {
                var displayGroup = new NavigationMeshDisplayGroup
                {
                    Color = new Color(group.Color.R, group.Color.G, group.Color.B),
                    Id = group.Id,
                    Index = group.Index,
                    IsVisible = group.IsVisible,
                };
                newDisplayGroups.Add(displayGroup);
            }

            // Run everything affecting the display groups on the game thread
            sceneEditorController.InvokeAsync(() =>
            {
                groupDisplaySettings.Clear();
                foreach (var group in newDisplayGroups)
                {
                    groupDisplaySettings.Add(group.Id, group);
                    group.Material = CreateDebugMaterial(group.Color);
                    group.HighlightMaterial = CreateDebugMaterial(group.Color);
                }

                var navigationMeshesToUpdate = debugVisuals.Keys.ToList();
                foreach (var navigationMesh in navigationMeshesToUpdate)
                {
                    UpdateNavigationMesh(navigationMesh, navigationMesh, debugVisuals[navigationMesh].Scene);
                }
            });
        }

        /// <summary>
        /// Updates the visibility of an existing group by Id
        /// </summary>
        public void UpdateGroupVisibility(Guid groupId, bool isVisible)
        {
            // Run everything affecting the display groups on the game thread
            sceneEditorController.InvokeAsync(() =>
            {
                NavigationMeshDisplayGroup displayGroup;
                if (groupDisplaySettings.TryGetValue(groupId, out displayGroup))
                {
                    displayGroup.IsVisible = isVisible;
                    foreach (var component in debugVisuals)
                    {
                        ModelComponent modelComponent;
                        if (component.Value.ModelComponents.TryGetValue(displayGroup.Id, out modelComponent))
                        {
                            modelComponent.Enabled = isVisible;
                        }
                    }
                }
            });
        }

        protected override async Task<bool> Initialize(EditorServiceGame editorGame)
        {
            if (editorGame is null)
                throw new ArgumentNullException(nameof(editorGame));

            if (editorGame is not SceneEditorGame sceneEditorGame)
                throw new ArgumentException($"{nameof(game)} is not of type {nameof(EntityHierarchyEditorGame)}.");
            game = sceneEditorGame;

            if (editor.Controller is not SceneEditorController controller)
                throw new ArgumentNullException(nameof(sceneEditorController));
            sceneEditorController = controller;

            gameSettingsProviderService = editor.ServiceProvider.Get<GameSettingsProviderService>();
            gameSettingsProviderService.GameSettingsChanged += GameSettingsProviderServiceOnGameSettingsChanged;
            await navigationMeshManager.Initialize();

            game.SceneAdded += GameOnSceneAdded;
            game.SceneRemoved += GameOnSceneRemoved;

            // Add debug entity
            rootDebugEntity = new Entity("Navigation debug entity");
            game.EditorScene.Entities.Add(rootDebugEntity);

            // Handle added/updated navigation meshes so that they can be made visible when this scene is shown
            editor.Session.AssetPropertiesChanged += OnAssetPropertiesChanged;
            editor.Session.DeletedAssetsChanged += OnDeletedAssetsChanged;

            editorGame.Script.AddTask(async () =>
            {
                while (editorGame.IsRunning)
                {
                    Update();
                    await editorGame.Script.NextFrame();
                }
            });

            // Initial update
            foreach (var asset in editor.Session.AllAssets)
            {
                if (asset.AssetType == typeof(NavigationMeshAsset))
                {
                    await UpdateNavigationMeshLink(asset);
                }
            }

            // Update linked navigation meshes when loaded content has changed
            //   This happens when a navigation mesh gets recompiled by changes in the scene or navigation mesh asset
            navigationMeshManager.Changed += NavigationMeshManagerOnChanged;

            SetDynamicNavigationSystem(game.GameSystems.OfType<DynamicNavigationMeshSystem>().FirstOrDefault());
            game.GameSystems.CollectionChanged += GameSystemsOnCollectionChanged;

            return true;
        }

        private Material CreateDebugMaterial(Color4 color)
        {
            Material navmeshMaterial = Material.New(game.GraphicsDevice, new MaterialDescriptor
            {
                Attributes =
                {
                    Diffuse = new MaterialDiffuseMapFeature(new ComputeColor()),
                    DiffuseModel = new MaterialDiffuseLambertModelFeature(),
                    Emissive = new MaterialEmissiveMapFeature(new ComputeColor()),
                }
            });

            Color4 deviceSpaceColor = color.ToColorSpace(game.GraphicsDevice.ColorSpace);
            deviceSpaceColor.A = 0.33f;

            // Set the color to the material
            var navmeshMaterialPass = navmeshMaterial.Passes[0];
            navmeshMaterialPass.Parameters.Set(MaterialKeys.DiffuseValue, deviceSpaceColor);
            navmeshMaterialPass.Parameters.Set(MaterialKeys.EmissiveValue, deviceSpaceColor);
            navmeshMaterialPass.Parameters.Set(MaterialKeys.EmissiveIntensity, 1.0f);
            navmeshMaterialPass.HasTransparency = true;

            return navmeshMaterial;
        }

        private void NavigationMeshManagerOnChanged(object sender, ItemChangeEventArgs args)
        {
            var assetId = (AssetId) args.Index.Value;

            if (navigationMeshAssets.TryGetValue(assetId, out AssetViewModel asset))
            {
                Scene targetScene = null;

                // Find out which scene to attach the navigation mesh preview to
                var navigationMeshAsset = (NavigationMeshAsset) asset.Asset;
                if (navigationMeshAsset.Scene is not null)
                {
                    var referencedSceneReference = AttachedReferenceManager.GetAttachedReference(navigationMeshAsset.Scene);
                    if (referencedSceneReference is not null)
                    {
                        loadedScenes.TryGetValue(referencedSceneReference.Id, out targetScene);
                    }
                }

                UpdateNavigationMesh((NavigationMesh) args.NewValue, (NavigationMesh)args.OldValue, targetScene);
            }
        }

        private NavigationMeshDebugVisual CreateDebugVisual(NavigationMesh navigationMesh, NavigationMesh previousNavigationMesh)
        {
            NavigationMeshDebugVisual debugVisual = new();

            debugVisual.DebugEntity = new Entity("Debug entity for navigation mesh");

            // Create a visual for every layer with a separate color
            using (var layers = navigationMesh.Layers.GetEnumerator())
            {
                while (layers.MoveNext())
                {
                    Model model = new Model();

                    var currentLayer = layers.Current.Value;
                    var currentId = layers.Current.Key;

                    if (!groupDisplaySettings.TryGetValue(currentId, out NavigationMeshDisplayGroup displayGroup))
                        // No display settings for this group
                        continue;

                    model.Add(displayGroup.Material);
                    model.Add(displayGroup.HighlightMaterial);

                    foreach (var p in currentLayer.Tiles)
                    {
                        bool updated = true;

                        NavigationMeshTile tile = p.Value;

                        // Extract vertex data
                        var tileVertexList = new List<Vector3>();
                        var tileIndexList = new List<int>();
                        if (!tile.GetTileVertices(tileVertexList, tileIndexList))
                            continue;

                        // Check if updated
                        if (previousNavigationMesh is not null &&
                            previousNavigationMesh.Layers.TryGetValue(currentId, out NavigationMeshLayer sourceLayer))
                        {
                            NavigationMeshTile oldTile = sourceLayer.FindTile(p.Key);
                            if (oldTile is not null && oldTile.Data.SequenceEqual(tile.Data))
                                updated = false;
                        }

                        // Stack layers vertically
                        Vector3 offset = new Vector3(0.0f, LayerHeightMultiplier * displayGroup.Index, 0.0f);

                        // Calculate mesh bounding box from navigation mesh points
                        BoundingBox aabb = BoundingBox.Empty;

                        var meshVertices = new List<VertexPositionNormalTexture>();
                        for (int i = 0; i < tileVertexList.Count; i++)
                        {
                            Vector3 position = tileVertexList[i] + offset;
                            BoundingBox.Merge(ref aabb, ref position, out aabb);

                            meshVertices.Add(new VertexPositionNormalTexture
                            {
                                Position = position,
                                Normal = Vector3.UnitY,
                                TextureCoordinate = new Vector2(0.5f, 0.5f)
                            });
                        }

                        MeshDraw draw;
                        using (var meshData = new GeometricMeshData<VertexPositionNormalTexture>(meshVertices.ToArray(), tileIndexList.ToArray(), true))
                        {
                            GeometricPrimitive primitive = new GeometricPrimitive(game.GraphicsDevice, meshData);
                            debugVisual.GeneratedDynamicPrimitives.Add(primitive);
                            draw = primitive.ToMeshDraw();
                        }

                        Mesh mesh = new Mesh
                        {
                            Draw = draw,
                            MaterialIndex = updated ? 1 : 0,
                            BoundingBox = aabb
                        };
                        model.Add(mesh);
                    }

                    // Create an entity per layer
                    var layerEntity = new Entity($"Navigation group {currentId}");

                    // Add a new model component
                    var modelComponent = new ModelComponent(model);
                    layerEntity.Add(modelComponent);
                    modelComponent.Enabled = displayGroup.IsVisible;
                    debugVisual.ModelComponents.Add(currentId, modelComponent);

                    debugVisual.DebugEntity.AddChild(layerEntity);
                }
            }

            return debugVisual;
        }

        private void GameSystemsOnCollectionChanged(object sender, TrackingCollectionChangedEventArgs e)
        {
            if (dynamicNavigationMeshSystem is not null)
                return;

            // Handle addition of dynamic navigation mesh system
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                SetDynamicNavigationSystem(e.Item as DynamicNavigationMeshSystem);
            }
        }

        private void SetDynamicNavigationSystem(DynamicNavigationMeshSystem newDynamicNavigationMeshSystem)
        {
            if (dynamicNavigationMeshSystem is not null)
                return;

            if (newDynamicNavigationMeshSystem is not null)
            {
                dynamicNavigationMeshSystem = newDynamicNavigationMeshSystem;

                newDynamicNavigationMeshSystem.NavigationMeshUpdated += DynamicNavigationMeshSystemOnNavigationMeshUpdated;

                // Initialize dynamic navigation mesh system with game settings
                if (gameSettingsProviderService.CurrentGameSettings is not null)
                {
                    var navigationSettings = gameSettingsProviderService.CurrentGameSettings.GetOrDefault<NavigationSettings>();
                    sceneEditorController.InvokeAsync(() => newDynamicNavigationMeshSystem.InitializeSettingsFromNavigationSettings(navigationSettings));
                }

                newDynamicNavigationMeshSystem.EnabledChanged += NewDynamicNavigationMeshSystemOnEnabledChanged;
            }
        }

        private void NewDynamicNavigationMeshSystemOnEnabledChanged(object sender, EventArgs eventArgs)
        {
            // Hide regular navigation meshes when dynamic system is enabled, this gets checked in ShouldDisplayNavigationMesh
            sceneEditorController.InvokeTask(UpdateAllNavigationMeshLinks);
        }

        private void GameSettingsProviderServiceOnGameSettingsChanged(object sender, GameSettingsChangedEventArgs gameSettingsChangedEventArgs)
        {
            // Send game settings changes to dynamic navigation mesh system
            if (dynamicNavigationMeshSystem is not null && gameSettingsChangedEventArgs.GameSettings is not null)
            {
                var navigationSettings = gameSettingsChangedEventArgs.GameSettings.GetOrDefault<NavigationSettings>();
                sceneEditorController.InvokeAsync(() => dynamicNavigationMeshSystem.InitializeSettingsFromNavigationSettings(navigationSettings));
            }
        }

        private void GameOnSceneAdded(Scene scene)
        {
            AssetId sceneAssetId = sceneEditorController.GetSceneAssetId(scene);
            loadedScenes.Add(sceneAssetId, scene);
            sceneEditorController.InvokeTask(UpdateAllNavigationMeshLinks);
        }

        private void GameOnSceneRemoved(Scene scene)
        {
            loadedScenes.Remove(sceneEditorController.GetSceneAssetId(scene));
            sceneEditorController.InvokeTask(UpdateAllNavigationMeshLinks);
        }

        private void DynamicNavigationMeshSystemOnNavigationMeshUpdated(object sender, EventArgs eventArgs)
        {
            NavigationMesh newNavigationMesh = dynamicNavigationMeshSystem.CurrentNavigationMesh;

            UpdateNavigationMesh(newNavigationMesh, dynamicNavigationMesh, null);
            dynamicNavigationMesh = newNavigationMesh;
        }

        private void OnAssetPropertiesChanged(object sender, AssetChangedEventArgs args)
        {
            sceneEditorController.InvokeTask(async () =>
            {
                bool shouldRebuild = false;
                foreach (var assetViewModel in args.Assets)
                {
                    if (assetViewModel.AssetType == typeof(NavigationMeshAsset))
                    {
                        await UpdateNavigationMeshLink(assetViewModel);
                    }
                    else if (assetViewModel.AssetType == typeof(SceneAsset))
                    {
                        shouldRebuild = true;
                    }
                }

                if (shouldRebuild && (dynamicNavigationMeshSystem?.Enabled ?? false))
                {
                    // Trigger rebuild of dynamic navigation
                    game.Script.AddTask(async () => await dynamicNavigationMeshSystem.Rebuild());
                }
            });
        }

        private void OnDeletedAssetsChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            sceneEditorController.InvokeTask(async () =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in args.NewItems)
                    {
                        AssetViewModel asset = (AssetViewModel)item;
                        if (asset is not null && asset.AssetType == typeof(NavigationMeshAsset))
                        {
                            await RemoveNavigationMeshLink(asset);
                        }
                    }
                }
            });
        }

        private void Update()
        {
            // Update highlighting
            foreach (var group in groupDisplaySettings.Values)
            {
                group.UpdateHighlighting(game);
            }
        }

        private void ToggleVisiblity(bool value)
        {
            foreach (var visual in debugVisuals)
            {
                foreach (var model in visual.Value.ModelComponents)
                {
                    lock (groupDisplaySettings)
                    {
                        if (value && groupDisplaySettings.TryGetValue(model.Key, out NavigationMeshDisplayGroup displayGroup))
                            model.Value.Enabled = displayGroup.IsVisible;
                        else
                            model.Value.Enabled = false;
                    }
                    model.Value.Enabled = value;
                }
            }
        }

        private void UpdateNavigationMesh(NavigationMesh newNavigationMesh, NavigationMesh oldNavigationMesh, Scene targetScene)
        {
            if (oldNavigationMesh is not null)
            {
                RemoveNavigationMesh(oldNavigationMesh);
            }

            if (newNavigationMesh is not null)
            {
                AddNavigationMesh(newNavigationMesh, oldNavigationMesh, targetScene);
            }

            foreach (var group in groupDisplaySettings.Values)
            {
                group.ResetHighlighting();
            }
        }

        private void AddNavigationMesh(NavigationMesh navigationMesh, NavigationMesh oldNavigationMesh, Scene targetScene)
        {
            var visual = CreateDebugVisual(navigationMesh, oldNavigationMesh);
            if (visual is not null)
            {
                // Apply scene offset to debug visual
                visual.DebugEntity.Transform.Position = targetScene?.Offset ?? Vector3.Zero;
                visual.Scene = targetScene;

                debugVisuals.Add(navigationMesh, visual);
                rootDebugEntity.AddChild(visual.DebugEntity);
            }
        }

        private void RemoveNavigationMesh(NavigationMesh navigationMesh)
        {
            if (debugVisuals.TryGetValue(navigationMesh, out NavigationMeshDebugVisual visual))
            {
                visual.Dispose();
                rootDebugEntity.RemoveChild(visual.DebugEntity);
                debugVisuals.Remove(navigationMesh);
            }
        }

        private bool HasSceneOrChildSceneReference(AttachedReference referencedSceneReference, Scene scene)
        {
            bool hasReference = referencedSceneReference is not null &&
                                loadedScenes.ContainsKey(referencedSceneReference.Id);
            if (hasReference)
                return true;

            // Recursive check for child scene references from this navigation mesh
            foreach (var childScene in scene.Children)
            {
                if (HasSceneOrChildSceneReference(referencedSceneReference, childScene))
                    return true;
            }

            return false;
        }

        private bool ShouldDisplayNavigationMesh(NavigationMeshAsset navigationMeshAsset)
        {
            // Don't show static navigation meshes when the dynamic system is enabled
            if (dynamicNavigationMeshSystem?.Enabled ?? false)
                return false;

            var referencedSceneReference = AttachedReferenceManager.GetAttachedReference(navigationMeshAsset.Scene);
            if (referencedSceneReference is null)
                return false;

            // Check the current scene and all child scenes if they are being referenced by this navigation mesh asset
            if (game.ContentScene is null)
                return false;

            if (HasSceneOrChildSceneReference(referencedSceneReference, game.ContentScene))
                return true;

            return false;
        }

        private async Task UpdateAllNavigationMeshLinks()
        {
            // Update all previous navigation mesh links in the session
            var previousNavigationMeshes = navigationMeshAssets.Values.ToArray();
            foreach (var navigationMeshAssetViewModel in previousNavigationMeshes)
            {
                await UpdateNavigationMeshLink(navigationMeshAssetViewModel);
            }
        }

        private async Task UpdateNavigationMeshLink(AssetViewModel asset)
        {
            if (!navigationMeshAssets.ContainsKey(asset.Id))
                navigationMeshAssets.Add(asset.Id, asset);

            // Either add or remove the navigation mesh to the navigation mesh manager, which will then handle loading the
            //   navigation mesh whenever it gets compiled and then call NavigationMeshManagerOnChanged to update the
            //   shown navigation mesh
            var navigationMeshAsset = (NavigationMeshAsset) asset.Asset;
            if (ShouldDisplayNavigationMesh(navigationMeshAsset))
                await navigationMeshManager.AddUnique(asset.Id);
            else
            {
                if (navigationMeshManager.Meshes.TryGetValue(asset.Id, out NavigationMesh navigationMesh))
                {
                    await navigationMeshManager.Remove(asset.Id);
                    RemoveNavigationMesh(navigationMesh);
                }
            }
        }

        private async Task RemoveNavigationMeshLink(AssetViewModel asset)
        {
            navigationMeshAssets.Remove(asset.Id);

            if (navigationMeshManager.Meshes.TryGetValue(asset.Id, out NavigationMesh navigationMesh))
            {
                await navigationMeshManager.Remove(asset.Id);
                RemoveNavigationMesh(navigationMesh);
            }
        }

        private class NavigationMeshDebugVisual : IDisposable
        {

            public readonly List<GeometricPrimitive> GeneratedDynamicPrimitives = new();
            public readonly Dictionary<Guid, ModelComponent> ModelComponents = new();

            public Entity DebugEntity;
            public Scene Scene;

            public void Dispose()
            {
                foreach (var primitive in GeneratedDynamicPrimitives)
                    primitive.Dispose();

                GeneratedDynamicPrimitives.Clear();
            }
        }

        private class NavigationMeshDisplayGroup
        {
            /// <summary>
            ///   The duration of highlighting for updated navigation mesh tiles.
            /// </summary>
            private const float HighlightDuration = 1.0f;
            private float highlightTimer;

            public Guid Id;
            public bool IsVisible;
            public Color4 Color;
            public Material Material;
            public Material HighlightMaterial;
            public int Index;

            /// <summary>
            ///   Restarts the highlight animation for visuals using the highlight material.
            /// </summary>
            public void ResetHighlighting()
            {
                highlightTimer = HighlightDuration;
            }

            /// <summary>
            /// Updates the highlight material animation
            /// </summary>
            public void UpdateHighlighting(GameBase game)
            {
                float elapsedTotalSeconds = (float) game.UpdateTime.Elapsed.TotalSeconds;
                if (highlightTimer > 0)
                {
                    if ((highlightTimer -= elapsedTotalSeconds) <= 0.0f)
                    {
                        highlightTimer = 0.0f;
                    }

                    float c = highlightTimer / HighlightDuration;
                    var color = Color4.Lerp(Color, new Color4(2.0f, 0.1f, 0.1f, 1.0f), c);

                    Color4 deviceSpaceColor = color.ToColorSpace(game.GraphicsDevice.ColorSpace);
                    deviceSpaceColor.A = 0.33f;

                    HighlightMaterial.Passes[0].Parameters.Set(MaterialKeys.DiffuseValue, deviceSpaceColor);
                    HighlightMaterial.Passes[0].Parameters.Set(MaterialKeys.EmissiveValue, deviceSpaceColor);
                    HighlightMaterial.Passes[0].Parameters.Set(MaterialKeys.EmissiveIntensity, 1.0f);
                }
            }
        }
    }
}
