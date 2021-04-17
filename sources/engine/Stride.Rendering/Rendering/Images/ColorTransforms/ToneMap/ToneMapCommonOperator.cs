// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ComponentModel;

using Stride.Core;

namespace Stride.Rendering.Images
{
    /// <summary>
    ///   Represents the common tonemapping properties shared by <see cref="ToneMapReinhardOperator"/>,
    ///   <see cref="ToneMapDragoOperator"/>, <see cref="ToneMapExponentialOperator"/>,
    ///   <see cref="ToneMapLogarithmicOperator"/>, and <see cref="ToneMapACESOperator"/>.
    /// </summary>
    [DataContract]
    public abstract class ToneMapCommonOperator : ToneMapOperator
    {
        protected ToneMapCommonOperator(string effectName) : base(effectName) { }


        /// <summary>
        ///   Gets or sets the luminance saturation.
        /// </summary>
        /// <value>The luminance saturation.</value>
        [DataMember(5)]
        [DefaultValue(1.0f)]
        public float LuminanceSaturation
        {
            get => Parameters.Get(ToneMapCommonOperatorShaderKeys.LuminanceSaturation);
            set => Parameters.Set(ToneMapCommonOperatorShaderKeys.LuminanceSaturation, value);
        }

        /// <summary>
        ///   Gets or sets the white level.
        /// </summary>
        /// <value>The white level.</value>
        [DataMember(8)]
        [DefaultValue(5.0f)]
        public float WhiteLevel
        {
            get => Parameters.Get(ToneMapCommonOperatorShaderKeys.WhiteLevel);
            set => Parameters.Set(ToneMapCommonOperatorShaderKeys.WhiteLevel, value);
        }
    }
}
