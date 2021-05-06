// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;

namespace Stride.Rendering.Images
{
    /// <summary>
    ///   Represents the ACES filmic tone mapping operator with highlight desaturation ("crosstalk"),
    ///   based on the curve fit by Krzysztof Narkowicz.
    /// </summary>
    /// <seealso href="https://knarkowicz.wordpress.com/2016/01/06/aces-filmic-tone-mapping-curve/">ACES Filmic Tone Mapping Curve</seealso>
    [DataContract("ToneMapACESOperator")]
    [Display("ACES")]
    public class ToneMapACESOperator : ToneMapCommonOperator
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="ToneMapReinhardOperator"/> class.
        /// </summary>
        public ToneMapACESOperator() : base("ToneMapACESOperatorShader") { }
    }
}
