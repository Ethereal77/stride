// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Physics;

namespace Stride.Assets.Physics
{
    [DataContract]
    [Display("Short")]
    public class ShortHeightmapHeightConversionParameters : IHeightmapHeightConversionParameters
    {
        [DataMemberIgnore]
        public HeightfieldTypes HeightType => HeightfieldTypes.Short;

        [DataMember(10)]
        public Vector2 HeightRange { get; set; } = new Vector2(-10, 10);

        [DataMemberIgnore]
        public float HeightScale => HeightScaleCalculator.Calculate(this);

        /// <summary>
        ///   Select how to calculate HeightScale.
        /// </summary>
        [DataMember(20)]
        [NotNull]
        [Display("HeightScale", Expand = ExpandRule.Always)]
        public IHeightScaleCalculator HeightScaleCalculator { get; set; } = new HeightScaleCalculator();
    }
}
