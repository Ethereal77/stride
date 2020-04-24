// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core;

namespace Stride.Physics
{
    [DataContract]
    [Display("Auto")]
    public class HeightScaleCalculator : IHeightScaleCalculator
    {
        public float Calculate(IHeightStickParameters heightDescription)
        {
            var heightRange = heightDescription.HeightRange;

            switch (heightDescription.HeightType)
            {
                case HeightfieldTypes.Float:
                    return 1.0f;

                case HeightfieldTypes.Short:
                    return Math.Max(Math.Abs(heightRange.X), Math.Abs(heightRange.Y)) / short.MaxValue;

                case HeightfieldTypes.Byte:
                    return Math.Abs(heightRange.X) <= Math.Abs(heightRange.Y)
                        ? heightRange.Y / byte.MaxValue
                        : heightRange.X / byte.MaxValue;

                default:
                    throw new NotSupportedException($"Unknown height type.");
            }
        }
    }
}
