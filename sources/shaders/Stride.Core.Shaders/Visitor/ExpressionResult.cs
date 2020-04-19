// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Shaders.Utility;

namespace Xenko.Core.Shaders.Visitor
{
    /// <summary>
    /// Result of an expression.
    /// </summary>
    public class ExpressionResult : LoggerResult
    {
        /// <summary>
        /// Gets or sets the result of an expression.
        /// </summary>
        /// <value>
        /// The result of an expression.
        /// </value>
        public double Value { get; set; }
    }
}
