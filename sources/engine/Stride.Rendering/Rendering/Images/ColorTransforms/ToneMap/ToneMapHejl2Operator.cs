// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.ComponentModel;

using Stride.Core;

namespace Stride.Rendering.Images
{
    /// <summary>
    /// The tonemap operator by Jim Hejl version 2 that does not include the gamma correction and has a whitepoint parameter.
    /// </summary>
    /// <remarks>
    /// https://twitter.com/jimhejl/status/633777619998130176
    /// </remarks>
    [DataContract("ToneMapHejl2Operator")]
    [Display("Hejl2")]
    public class ToneMapHejl2Operator : ToneMapOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToneMapHejlDawsonOperator"/> class.
        /// </summary>
        public ToneMapHejl2Operator()
            : base("ToneMapHejl2OperatorShader")
        {
        }

        /// <summary>
        /// Gets or sets the white point.
        /// </summary>
        /// <value>The white point.</value>
        [DataMember(10)]
        [DefaultValue(5.0f)]
        public float WhitePoint
        {
            get
            {
                return Parameters.Get(ToneMapHejl2OperatorShaderKeys.WhitePoint);
            }
            set
            {
                Parameters.Set(ToneMapHejl2OperatorShaderKeys.WhitePoint, value);
            }
        }
    }
}
