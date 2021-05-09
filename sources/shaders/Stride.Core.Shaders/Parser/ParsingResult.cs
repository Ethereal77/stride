// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Shaders.Ast;
using Stride.Core.Shaders.Utility;

namespace Stride.Core.Shaders.Parser
{

    /// <summary>
    /// A Parsing result.
    /// </summary>
    [DataContract]
    public class ParsingResult : LoggerResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the shader.
        /// </summary>
        /// <value>
        /// The shader.
        /// </value>
        public Shader Shader { get; set; }

        /// <summary>
        /// Gets or sets the time to parse in ms.
        /// </summary>
        /// <value>
        /// The time to parse ms.
        /// </value>
        public long TimeToParse { get; set; }

        /// <summary>
        /// Gets or sets the token count.
        /// </summary>
        /// <value>
        /// The token count.
        /// </value>
        public int TokenCount { get; set; }

        #endregion
    }
}
