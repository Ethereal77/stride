// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Diagnostics;

namespace Stride.Assets.SpriteFont.Compiler
{
    /// <summary>
    /// Result of a compilation.
    /// </summary>
    internal sealed class ContentCompilerResult
    {
        public bool IsContentGenerated { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors
        {
            get { return Logger.HasErrors; }
        }

        /// <summary>
        /// Gets the logger containing compilation messages..
        /// </summary>
        /// <value>The logger.</value>
        public LoggerResult Logger { get; set; }
    }
}
