// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Mathematics;

namespace Xenko.UI.Tests
{
    internal static class RandomExtension
    {
        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        public static Vector3 NextVector3(this Random random)
        {
            return new Vector3(random.NextFloat(), random.NextFloat(), random.NextFloat());
        }

        public static Thickness NextThickness(this Random random, float leftFactor, float topFactor, float backFactor, float rightFactor, float bottomFactor, float frontFactor)
        {
            return new Thickness(
                random.NextFloat() * leftFactor, random.NextFloat() * topFactor, random.NextFloat() * backFactor,
                random.NextFloat() * rightFactor, random.NextFloat() * bottomFactor, random.NextFloat() * frontFactor);
        }
    }
}
