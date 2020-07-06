// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.VisualStudio.Commands.Shaders
{
    /// <summary>
    ///   Represents a diagnostics message for a particular line of shader source code.
    /// </summary>
    [Serializable]
    public class RawShaderAnalysisMessage
    {
        /// <summary>
        ///   Gets or sets the text span this message refers to.
        /// </summary>
        /// <value>The text span this message refers to.</value>
        public RawSourceSpan Span { get; set; }

        /// <summary>
        ///   Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Text { get; set; }

        /// <summary>
        ///   Gets or sets the shader source code.
        /// </summary>
        /// <value>The shader source code.</value>
        public string Code { get; set; }

        /// <summary>
        ///   Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        public override string ToString() => $"{Span}: {Type} {Code} : {Text}";
    }
}
