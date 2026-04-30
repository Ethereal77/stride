// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Mathematics;

namespace CSharpIntermediate.Code.Extensions
{
    public static class VectorExtensionMethods
    {
        public static string Print(this Vector2 pos)
        {
            return $"{Math.Round(pos.X, 1)} , {Math.Round(pos.Y, 1)}";
        }

        public static string Print(this Vector3 pos)
        {
            return $"{Math.Round(pos.X, 1)} , {Math.Round(pos.Y, 1)} , {Math.Round(pos.Z, 1)}";
        }
    }
}
