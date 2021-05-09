// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Shaders.Utility;

namespace Stride.Core.Shaders.Visitor
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
