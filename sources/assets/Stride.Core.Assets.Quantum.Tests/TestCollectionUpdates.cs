// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Quantum.Tests.Helpers;
using Stride.Core.Reflection;
using Stride.Core.Quantum;

using Xunit;

namespace Stride.Core.Assets.Quantum.Tests
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
