// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma warning disable SA1402 // File may only contain a single type

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Core.Reflection;
using Stride.Engine;
using Stride.Engine.Design;
using Stride.Engine.Processors;
using Stride.Navigation.Processors;
using Stride.Physics;
using Stride.Games;

namespace Stride.Navigation
{
    /// <summary>
    ///   Represents a system that handles the building of navigation meshes at runtime.
    /// </summary>
    public class DynamicNavigationMeshSystem : GameSystem
    {
        /// <summary>
        ///   Gets or sets a value indicating whether to automatically rebuild the navigation mesh on
        ///   addition or removal of <see cref="StaticColliderComponent"/>s.
        /// </summary>
        [DataMember(5)]
        public bool AutomaticRebuild { get; set; } = true;

        /// <summary>
        ///   Gets or sets the collision filter that determines which colliders are used in the
        ///   generation of a navigation mesh.
        /// </summary>
        [DataMember(10)]
        public CollisionFilterGroupFlags IncludedCollisionGroups { get; set; }

        /// <summary>
        ///   Gets or sets the navigation mesh build settings.
        /// </summary>
        [DataMember(20)]
        public NavigationMeshBuildSettings BuildSettings { get; set; }

        /// <summary>
        ///   Gets a list of the mesh groups used for agents with the dynamic navigation mesh.
        /// </summary>
        [DataMember(30)]
        public List<NavigationMeshGroup> Groups { get; private set; } = new List<NavigationMeshGroup>();

        private bool pendingRebuild = true;

        private SceneInstance currentSceneInstance;

        private NavigationMeshBuilder builder = new NavigationMeshBuilder();

        private CancellationTokenSource buildTaskCancellationTokenSource;

        private SceneSystem sceneSystem;
        private ScriptSystem scriptSystem;
        private StaticColliderProcessor processor;


        public DynamicNavigationMeshSystem(IServiceRegistry registry) : base(registry)
        {
            Enabled = false;
            EnabledChanged += OnEnabledChanged;
        }


        /// <summary>
        ///   Occurs when the navigation mesh for the current scene is updated.
        /// </summary>
        public event EventHandler<NavigationMeshUpdatedEventArgs> NavigationMeshUpdated;


        /// <summary>
        ///   Gets the most recently built navigation mesh.
        /// </summary>
        public NavigationMesh CurrentNavigationMesh { get; private set; }


        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();

            var gameSettings = Services.GetService<IGameSettingsService>()?.Settings;
            if (gameSettings != null)
            {
                InitializeSettingsFromGameSettings(gameSettings);
            }
            else
            {
                // Initial build settings
                BuildSettings = ObjectFactoryRegistry.NewInstance<NavigationMeshBuildSettings>();
                IncludedCollisionGroups = CollisionFilterGroupFlags.AllFilter;
                Groups = new List<NavigationMeshGroup>
                {
                    ObjectFactoryRegistry.NewInstance<NavigationMeshGroup>(),
                };
            }

            sceneSystem = Services.GetSafeServiceAs<SceneSystem>();
            scriptSystem = Services.GetSafeServiceAs<ScriptSystem>();
        }

        /// <summary>
        ///   Copies the default settings from the <see cref="GameSettings"/> for navigation mesh building.
        /// </summary>
        public void InitializeSettingsFromGameSettings(GameSettings gameSettings)
        {
            if (gameSettings is null)
                throw new ArgumentNullException(nameof(gameSettings));

            // Initialize build settings from game settings
            var navigationSettings = gameSettings.Configurations.Get<NavigationSettings>();

            InitializeSettingsFromNavigationSettings(navigationSettings);
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            // This system should load from settings before becoming functional
            if (!Enabled)
                return;

            if (currentSceneInstance != sceneSystem?.SceneInstance)
            {
                // ReSharper disable once PossibleNullReferenceException
                UpdateScene(sceneSystem.SceneInstance);
            }

            if (pendingRebuild && currentSceneInstance != null)
            {
                scriptSystem.AddTask(async () =>
                {
                    // TODO: EntityProcessors
                    //   Currently have to wait a frame for transformations to update
                    //   for example when calling Rebuild from the event that a component was added to the scene, this component will not be in the correct location yet
                    //   since the TransformProcessor runs the next frame
                    await scriptSystem.NextFrame();
                    await Rebuild();
                });
                pendingRebuild = false;
            }
        }

        /// <summary>
        ///   Starts an asynchronous rebuild of the navigation mesh.
        /// </summary>
        public async Task<NavigationMeshBuildResult> Rebuild()
        {
            if (currentSceneInstance is null)
                return new NavigationMeshBuildResult();

            // Cancel running build
            // TODO: Check if the running build can actual satisfy the current rebuild request and don't cancel in that case
            buildTaskCancellationTokenSource?.Cancel();
            buildTaskCancellationTokenSource = new CancellationTokenSource();

            // Collect bounding boxes
            var boundingBoxProcessor = currentSceneInstance.GetProcessor<BoundingBoxProcessor>();
            if (boundingBoxProcessor is null)
                return new NavigationMeshBuildResult();

            List<BoundingBox> boundingBoxes = new List<BoundingBox>();
            foreach (var boundingBox in boundingBoxProcessor.BoundingBoxes)
            {
                boundingBox.Entity.Transform.WorldMatrix.Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation);
                boundingBoxes.Add(new BoundingBox(translation - boundingBox.Size * scale, translation + boundingBox.Size * scale));
            }

            var result = Task.Run(() =>
            {
                // Only have one active build at a time
                lock (builder)
                {
                    return builder.Build(BuildSettings, Groups, IncludedCollisionGroups,  boundingBoxes, buildTaskCancellationTokenSource.Token);
                }
            });
            await result;

            FinilizeRebuild(result);

            return result.Result;
        }

        internal void InitializeSettingsFromNavigationSettings(NavigationSettings navigationSettings)
        {
            BuildSettings = navigationSettings.BuildSettings;
            IncludedCollisionGroups = navigationSettings.IncludedCollisionGroups;
            Groups = navigationSettings.Groups;
            Enabled = navigationSettings.EnableDynamicNavigationMesh;

            pendingRebuild = true;
        }

        private void FinilizeRebuild(Task<NavigationMeshBuildResult> resultTask)
        {
            var result = resultTask.Result;
            if (result.Success)
            {
                var args = new NavigationMeshUpdatedEventArgs
                {
                    OldNavigationMesh = CurrentNavigationMesh,
                    BuildResult = result,
                };
                CurrentNavigationMesh = result.NavigationMesh;
                NavigationMeshUpdated?.Invoke(this, args);
            }
        }

        private void UpdateScene(SceneInstance newSceneInstance)
        {
            if (currentSceneInstance != null)
            {
                if (processor != null)
                {
                    currentSceneInstance.Processors.Remove(processor);
                    processor.ColliderAdded -= ProcessorOnColliderAdded;
                    processor.ColliderRemoved -= ProcessorOnColliderRemoved;
                }

                // Clear builder
                builder = new NavigationMeshBuilder();
            }

            // Set the currect scene
            currentSceneInstance = newSceneInstance;

            if (currentSceneInstance != null)
            {
                // Scan for components
                processor = new StaticColliderProcessor();
                processor.ColliderAdded += ProcessorOnColliderAdded;
                processor.ColliderRemoved += ProcessorOnColliderRemoved;
                currentSceneInstance.Processors.Add(processor);

                pendingRebuild = true;
            }
        }

        private void ProcessorOnColliderAdded(StaticColliderComponent component, StaticColliderData data)
        {
            builder.Add(data);
            if (AutomaticRebuild)
            {
                pendingRebuild = true;
            }
        }

        private void ProcessorOnColliderRemoved(StaticColliderComponent component, StaticColliderData data)
        {
            builder.Remove(data);
            if (AutomaticRebuild)
            {
                pendingRebuild = true;
            }
        }

        private void Cleanup()
        {
            UpdateScene(null);

            CurrentNavigationMesh = null;
            NavigationMeshUpdated?.Invoke(this, null);
        }

        private void OnEnabledChanged(object sender, EventArgs eventArgs)
        {
            if (!Enabled)
            {
                Cleanup();
            }
            else
            {
                pendingRebuild = true;
            }
        }
    }

    public class NavigationMeshUpdatedEventArgs : EventArgs
    {
        public NavigationMesh OldNavigationMesh;
        public NavigationMeshBuildResult BuildResult;
    }
}
