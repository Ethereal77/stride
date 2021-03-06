// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Xunit;

namespace Stride.Core.Assets.Tests
{
    public class TestAssetInheritance
    {
        [Fact]
        public void TestWithParts()
        {
            // Create a derivative asset with asset parts
            var project = new Package();
            var assets = new List<TestAssetWithParts>();
            var assetItems = new List<AssetItem>();

            assets.Add(new TestAssetWithParts()
            {
                Parts =
                {
                        new AssetPartTestItem(Guid.NewGuid()),
                        new AssetPartTestItem(Guid.NewGuid())
                }
            });
            assetItems.Add(new AssetItem("asset-0", assets[0]));
            project.Assets.Add(assetItems[0]);

            var childAsset = (TestAssetWithParts)assetItems[0].CreateDerivedAsset();

            // Check that child asset has a base
            Assert.NotNull(childAsset.Archetype);

            // Check base asset
            Assert.Equal(assets[0].Id, childAsset.Archetype.Id);

            // Check that base is correctly setup for the part
            var i = 0;
            var instanceId = Guid.Empty;
            foreach (var part in childAsset.Parts)
            {
                Assert.Equal(assets[0].Id, part.Base.BasePartAsset.Id);
                Assert.Equal(assets[0].Parts[i].Id, part.Base.BasePartId);
                if (instanceId == Guid.Empty)
                    instanceId = part.Base.InstanceId;
                Assert.NotEqual(Guid.Empty, instanceId);
                Assert.Equal(instanceId, part.Base.InstanceId);
                ++i;
            }
        }
    }
}
