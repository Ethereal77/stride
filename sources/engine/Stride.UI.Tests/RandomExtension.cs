// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Mathematics;

namespace Stride.UI.Tests
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
