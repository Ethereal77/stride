// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Xunit;

using Stride.Core.Mathematics;

namespace Stride.UI.Tests.Layering
{
    public static class Utilities
    {
        // ReSharper disable UnusedParameter.Local
        public static void AssertAreNearlyEqual(float reference, float value)
        {
            Assert.True(Math.Abs(reference - value) < 0.001f);
        }

        public static void AssertAreNearlyEqual(Vector2 reference, Vector2 value)
        {
            Assert.True((reference - value).Length() < 0.001f);
        }

        public static void AssertAreNearlyEqual(Matrix reference, Matrix value)
        {
            var diffMat = reference - value;
            for (int i = 0; i < 16; i++)
                Assert.True(Math.Abs(diffMat[i]) < 0.001);
        }
        // ReSharper restore UnusedParameter.Local 

        public static void AreExactlyEqual(Vector3 left, Vector3 right)
        {
            Assert.Equal(left.X, right.X);
            Assert.Equal(left.Y, right.Y);
            Assert.Equal(left.Z, right.Z);
        }
    }
}
