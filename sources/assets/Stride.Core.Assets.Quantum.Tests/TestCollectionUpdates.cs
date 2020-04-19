// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Quantum.Tests.Helpers;
using Xenko.Core.Reflection;
using Xenko.Core.Quantum;

using Xunit;

namespace Xenko.Core.Assets.Quantum.Tests
{
    public class TestCollectionUpdates
    {
        [Fact]
        public void TestSimpleCollectionUpdate()
        {
            var container = new AssetPropertyGraphContainer(new AssetNodeContainer { NodeBuilder = { NodeFactory = new AssetNodeFactory() } });
            var asset = new Types.MyAsset2 { MyStrings = { "aaa", "bbb", "ccc" } };
            var assetItem = new AssetItem("MyAsset", asset);
            var graph = AssetQuantumRegistry.ConstructPropertyGraph(container, assetItem, null);
            var node = graph.RootNode[nameof(Types.MyAsset2.MyStrings)];
            //var ids = CollectionItemIdHelper.TryGetCollectionItemIds(asset.MyStrings, out itemIds);
        }
    }
}
