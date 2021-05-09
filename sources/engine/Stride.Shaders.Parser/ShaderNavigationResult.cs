// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Shaders.Utility;

namespace Stride.Shaders.Parser
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
        public Stride.Core.Shaders.Ast.SourceSpan DefinitionLocation { get; set; }

        /// <summary>
        /// Gets the parsing messages.
        /// </summary>
        /// <value>The messages.</value>
        public LoggerResult Messages { get; set; }
    }
}
