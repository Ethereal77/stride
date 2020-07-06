// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.VisualStudio.Commands.Shaders
{
    /// <summary>
    ///   Reoresents a span of text in a source code file.
    /// </summary>
    [Serializable]
    public class RawSourceSpan
    {
        public RawSourceSpan() { }

        public RawSourceSpan(string file, int line, int column)
        {
            File = file;
            Line = line;
            Column = column;
            EndLine = line;
            EndColumn = column;
        }

        public string File { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public int EndLine { get; set; }

        public int EndColumn { get; set; }

        // TODO: Include span
        public override string ToString() => $"{File ?? string.Empty}({Line},{Column})";
    }
}
