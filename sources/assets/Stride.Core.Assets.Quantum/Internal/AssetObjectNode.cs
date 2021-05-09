// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Annotations;
using Stride.Core.Reflection;
using Stride.Core.Quantum;
using Stride.Core.Quantum.References;

namespace Stride.Core.Assets.Quantum.Internal
{
    internal class AssetObjectNode : ObjectNode, IAssetObjectNodeInternal
    {
        private AssetObjectNodeExtended ex;

        public AssetObjectNode([NotNull] INodeBuilder nodeBuilder, object value, Guid guid, [NotNull] ITypeDescriptor descriptor, IReference reference)
            : base(nodeBuilder, value, guid, descriptor, reference)
        {
            ex = new AssetObjectNodeExtended(this);
            ItemChanged += (sender, e) => ex.OnItemChanged(sender, e);
        }

        public AssetPropertyGraph PropertyGraph => ex.PropertyGraph;

        public IGraphNode BaseNode => ex.BaseNode;

        public new IAssetMemberNode this[[NotNull] string name] => (IAssetMemberNode)base[name];

        public event EventHandler<EventArgs> OverrideChanging;

        public event EventHandler<EventArgs> OverrideChanged;

        public void SetContent([NotNull] string key, IGraphNode node) => ex.SetContent(key, node);

        [CanBeNull]
        public IGraphNode GetContent([NotNull] string key) => ex.GetContent(key);

        public void ResetOverrideRecursively() => ex.ResetOverrideRecursively(NodeIndex.Empty);

        public void ResetOverrideRecursively(NodeIndex indexToReset) => ex.ResetOverrideRecursively(indexToReset);

        public void OverrideItem(bool isOverridden, NodeIndex index) => ex.OverrideItem(isOverridden, index);

        public void OverrideKey(bool isOverridden, NodeIndex index) => ex.OverrideKey(isOverridden, index);

        public void OverrideDeletedItem(bool isOverridden, ItemId deletedId) => ex.OverrideDeletedItem(isOverridden, deletedId);

        public bool IsItemDeleted(ItemId itemId) => ex.IsItemDeleted(itemId);

        public void Restore(object restoredItem, ItemId id) => ex.Restore(restoredItem, id);

        public void Restore(object restoredItem, NodeIndex index, ItemId id) => ex.Restore(restoredItem, index, id);

        public void RemoveAndDiscard(object item, NodeIndex itemIndex, ItemId id) => ex.RemoveAndDiscard(item, itemIndex, id);

        public OverrideType GetItemOverride(NodeIndex index) => ex.GetItemOverride(index);

        public OverrideType GetKeyOverride(NodeIndex index) => ex.GetKeyOverride(index);

        public bool IsItemInherited(NodeIndex index) => ex.IsItemInherited(index);

        public bool IsKeyInherited(NodeIndex index) => ex.IsKeyInherited(index);

        public bool IsItemOverridden(NodeIndex index) => ex.IsItemOverridden(index);

        public bool IsItemOverriddenDeleted(ItemId id) => ex.IsItemOverriddenDeleted(id);

        public bool IsKeyOverridden(NodeIndex index) => ex.IsKeyOverridden(index);

        public IEnumerable<NodeIndex> GetOverriddenItemIndices() => ex.GetOverriddenItemIndices();

        public IEnumerable<NodeIndex> GetOverriddenKeyIndices() => ex.GetOverriddenKeyIndices();

        public ItemId IndexToId(NodeIndex index) => ex.IndexToId(index);

        public bool TryIndexToId(NodeIndex index, out ItemId id) => ex.TryIndexToId(index, out id);

        public bool HasId(ItemId id) => ex.HasId(id);

        public NodeIndex IdToIndex(ItemId id) => ex.IdToIndex(id);

        public bool TryIdToIndex(ItemId id, out NodeIndex index) => ex.TryIdToIndex(id, out index);

        IAssetObjectNode IAssetObjectNode.IndexedTarget(NodeIndex index) => (IAssetObjectNode)IndexedTarget(index);

        void IAssetObjectNodeInternal.DisconnectOverriddenDeletedItem(ItemId deletedId) => ex.DisconnectOverriddenDeletedItem(deletedId);

        void IAssetObjectNodeInternal.NotifyOverrideChanging() => OverrideChanging?.Invoke(this, EventArgs.Empty);

        void IAssetObjectNodeInternal.NotifyOverrideChanged() => OverrideChanged?.Invoke(this, EventArgs.Empty);

        bool IAssetNodeInternal.ResettingOverride { get => ex.ResettingOverride; set => ex.ResettingOverride = value; }

        void IAssetNodeInternal.SetPropertyGraph(AssetPropertyGraph assetPropertyGraph) => ex.SetPropertyGraph(assetPropertyGraph);

        void IAssetNodeInternal.SetBaseNode(IGraphNode node) => ex.SetBaseContent(node);
    }
}
