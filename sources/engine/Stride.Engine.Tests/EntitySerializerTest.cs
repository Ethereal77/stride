// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Xunit;

using Stride.Core.Mathematics;
using Stride.Graphics.Regression;

namespace Stride.Engine.Tests
{
    public class EntitySerializerTest : GameTestBase
    {
        [Fact]
        public void TestSaveAndLoadEntities()
        {
            PerformTest(game =>
            {
                var entity = new Entity { Transform = { Position = new Vector3(100.0f, 0.0f, 0.0f) } };
                game.Content.Save("EntityAssets/Entity", entity);

                GC.Collect();

                var entity2 = game.Content.Load<Entity>("EntityAssets/Entity");
                Assert.Equal(entity.Transform.Position, entity2.Transform.Position);
            });
        }
    }
}
