// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Quantum;
using Stride.Core.Assets;
using Stride.Assets.Presentation.AssetEditors.GameEditor.Services;
using Stride.Editor.EditorGame.ContentLoader;
using Stride.Navigation;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Game
{
    /// <summary>
    ///   Represents a manager that always loads the <see cref="NavigationMesh"/> associated with a given <see cref="Engine.Scene"/>.
    /// </summary>
    [DataContract]
    public class NavigationMeshManager : IAsyncDisposable
    {
        [DataMember]
        public readonly Dictionary<AssetId, NavigationMesh> Meshes = new Dictionary<AssetId, NavigationMesh>();

        private readonly AbsoluteId referencerId;
        private readonly IEditorContentLoader loader;
        private readonly IObjectNode meshesNode;

        public NavigationMeshManager([NotNull] IEditorGameController controller)
        {
            referencerId = new AbsoluteId(AssetId.Empty, Guid.NewGuid());
            loader = controller.Loader;
            var root = controller.GameSideNodeContainer.GetOrCreateNode(this);
            meshesNode = root[nameof(Meshes)].Target;
            meshesNode.ItemChanged += (sender, args) => { Changed?.Invoke(this, args); };
        }

        public async Task DisposeAsync()
        {
            foreach (var pair in Meshes)
            {
                await loader.Manager.ClearContentReference(referencerId, pair.Key, meshesNode, new NodeIndex(pair.Key));
            }
            await loader.Manager.RemoveReferencer(referencerId);
        }

        /// <summary>
        ///   Fired when a <see cref="NavigationMesh"/> has been reloaded.
        /// </summary>
        public event EventHandler<ItemChangeEventArgs> Changed;

        public Task Initialize()
        {
            return loader.Manager.RegisterReferencer(referencerId);
        }

        /// <summary>
        ///   Adds a reference to a <see cref="NavigationMesh"/> if it doesn't already exist.
        /// </summary>
        /// <param name="assetId"><see cref="AssetId"/> of the <see cref="Engine.Scene"/> for which the <see cref="NavigationMesh"/> is to be registered.</param>
        public Task AddUnique(AssetId assetId)
        {
            if (Meshes.ContainsKey(assetId))
                return Task.CompletedTask;

            Meshes.Add(assetId, new NavigationMesh());
            return loader.Manager.PushContentReference(referencerId, assetId, meshesNode, new NodeIndex(assetId));
        }

        /// <summary>
        ///   Removes a reference to a <see cref="NavigationMesh"/>.
        /// </summary>
        /// <param name="assetId"><see cref="AssetId"/> of the <see cref="Engine.Scene"/> for which the <see cref="NavigationMesh"/> is to be removed.</param>
        public Task Remove(AssetId assetId)
        {
            if (!Meshes.ContainsKey(assetId))
                throw new InvalidOperationException();

            Meshes.Remove(assetId);
            return loader.Manager.ClearContentReference(referencerId, assetId, meshesNode, new NodeIndex(assetId));
        }
    }
}
