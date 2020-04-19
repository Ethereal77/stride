// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Rendering.Images
{
    /// <summary>
    /// The tonemap logarithmic operator.
    /// </summary>
    [DataContract("ToneMapLogarithmicOperator")]
    [Display("Logarithmic")]
    public class ToneMapLogarithmicOperator : ToneMapCommonOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToneMapLogarithmicOperator"/> class.
        /// </summary>
        public ToneMapLogarithmicOperator()
            : base("ToneMapLogarithmicOperatorShader")
        {
        }
    }
}
