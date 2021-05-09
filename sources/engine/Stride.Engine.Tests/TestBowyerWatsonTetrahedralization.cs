// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Xunit;

using Stride.Core.Collections;
using Stride.Core.Mathematics;
using Stride.Rendering.LightProbes;

namespace Stride.Engine.Tests
{
    public class TestBowyerWatsonTetrahedralization
    {
        [Fact]
        public void TestCube()
        {
            // Build cube from (0,0,0) to (1,1,1)
            var positions = new FastList<Vector3>();
            for (int i = 0; i < 8; ++i)
            {
                positions.Add(new Vector3
                {
                    X = i % 2 == 0 ? 0.0f : 1.0f,
                    Y = i / 4 == 0 ? 0.0f : 1.0f,
                    Z = (i / 2) % 2 == 0 ? 0.0f : 1.0f,
                });
            }

            var tetra = new BowyerWatsonTetrahedralization();
            var tetraResult = tetra.Compute(positions);
        }
    }
}
