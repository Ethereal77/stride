// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Stride.Core.Annotations;
using Stride.Core.Quantum;
using Stride.Engine;
using Stride.Core.Assets.Quantum;
using Stride.Engine.Design;
using Stride.Assets.Entities;
using Stride.Assets.Presentation.AssetEditors.GameEditor.Services;
using Stride.Editor.EditorGame.Game;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Game
{
    public abstract class EditorGameComponentChangeWatcherService : EditorGameServiceBase
    {
        private readonly Dictionary<EntityComponent, GraphNodeChangeListener> registeredListeners = new();
        private readonly IEditorGameController controller;


        protected EditorGameComponentChangeWatcherService(IEditorGameController controller)
        {
            this.controller = controller;
        }


        [NotNull]
        public abstract Type ComponentType { get; }


        /// <inheritdoc/>
        public override ValueTask DisposeAsync()
        {
            EnsureNotDestroyed(nameof(EditorGameComponentChangeWatcherService));

            foreach (var component in registeredListeners.Keys.ToList())
            {
                UnregisterComponent(component);
            }

            return base.DisposeAsync();
        }

        /// <inheritdoc/>
        protected override Task<bool> Initialize(EditorServiceGame game)
        {
            game.SceneSystem.SceneInstance.EntityAdded += OnEntityAdded;
            game.SceneSystem.SceneInstance.EntityRemoved += OnEntityRemoved;
            game.SceneSystem.SceneInstance.ComponentChanged += OnComponentChanged;

            return Task.FromResult(true);
        }

        protected virtual void ComponentPropertyChanged(object sender, INodeChangeEventArgs e)
        {
            // Do nothing by default
        }

        private void RegisterComponent(EntityComponent component)
        {
            if (component is not null && ComponentType.IsInstanceOfType(component))
            {
                var rootNode = controller.GameSideNodeContainer.GetOrCreateNode(component);
                var listener = new AssetGraphNodeChangeListener(rootNode, AssetQuantumRegistry.GetDefinition(typeof(EntityHierarchyAssetBase)));
                listener.Initialize();
                listener.ValueChanged += ComponentPropertyChanged;
                listener.ItemChanged += ComponentPropertyChanged;
                registeredListeners.Add(component, listener);
            }
        }

        private void UnregisterComponent(EntityComponent component)
        {
            if (component is not null && registeredListeners.TryGetValue(component, out var listener))
            {
                listener.ValueChanged -= ComponentPropertyChanged;
                listener.ItemChanged -= ComponentPropertyChanged;
                listener.Dispose();
                registeredListeners.Remove(component);
            }
        }

        private void OnEntityAdded(object sender, Entity e)
        {
            foreach (var component in e.Components)
            {
                RegisterComponent(component);
            }
        }

        private void OnEntityRemoved(object sender, Entity e)
        {
            foreach (var component in e.Components)
            {
                UnregisterComponent(component);
            }
        }

        private void OnComponentChanged(object sender, EntityComponentEventArgs e)
        {
            UnregisterComponent(e.PreviousComponent);
            RegisterComponent(e.NewComponent);
        }
    }
}
