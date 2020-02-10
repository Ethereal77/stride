// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Shaders.Utility;

namespace Xenko.Shaders.Parser
{
    /// <summary>
    /// Results of a <see cref="ShaderNavigation"/>
    /// </summary>
    public class ShaderNavigationResult
    {
        public ShaderNavigationResult()
        {
            Messages = new LoggerResult();
        }

        /// <summary>
        /// Gets or sets the definition location.
        /// </summary>
        /// <value>The definition location.</value>
        public Xenko.Core.Shaders.Ast.SourceSpan DefinitionLocation { get; set; }

        /// <summary>
        /// Gets the parsing messages.
        /// </summary>
        /// <value>The messages.</value>
        public LoggerResult Messages { get; set; }
    }
}
