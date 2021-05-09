// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Physics
{
    public interface IHeightStickParameters
    {
        /// <summary>
        ///   The type of the height.
        /// </summary>
        HeightfieldTypes HeightType { get; }

        /// <summary>
        ///   The range of the height.
        /// </summary>
        /// <remarks>
        ///   X is min height and Y is max height.
        ///   (height * HeightScale) should be in this range.
        ///   Positive and negative heights can not be handle at the same time when the height type is Byte.
        /// </remarks>
        Vector2 HeightRange { get; }

        /// <summary>
        ///   Used to calculate the height when the height type is Short or Byte. HeightScale should be 1 when the height type is Float.
        /// </summary>
        float HeightScale { get; }
    }
}
