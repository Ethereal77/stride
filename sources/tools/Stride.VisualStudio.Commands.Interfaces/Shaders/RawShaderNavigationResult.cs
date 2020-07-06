// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Stride.VisualStudio.Commands.Shaders
{
    /// <summary>
    ///   Represents the result of shader definition navigation.
    /// </summary>
    [Serializable]
    public class RawShaderNavigationResult
    {
        public RawShaderNavigationResult()
        {
            Messages = new List<RawShaderAnalysisMessage>();
        }

        /// <summary>
        ///   Gets or sets the definition span.
        /// </summary>
        /// <value>The span that marks the definition.</value>
        public RawSourceSpan DefinitionSpan { get; set; }

        /// <summary>
        ///   Gets the parsing messages.
        /// </summary>
        /// <value>The parsing messages.</value>
        public List<RawShaderAnalysisMessage> Messages { get; private set; }
    }
}
