// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Rendering.Images
{
    /// <summary>
    /// The tonemap operator by Jim Hejl and Richard Burgess-Dawson.
    /// </summary>
    /// <remarks>http://filmicgames.com/archives/75</remarks>
    [DataContract("ToneMapHejlDawsonOperator")]
    [Display("Hejl-Dawson")]
    public class ToneMapHejlDawsonOperator : ToneMapOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToneMapHejlDawsonOperator"/> class.
        /// </summary>
        public ToneMapHejlDawsonOperator()
            : base("ToneMapHejlDawsonOperatorShader")
        {
        }
    }
}
