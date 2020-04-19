// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Diagnostics;

namespace Xenko.Assets.SpriteFont.Compiler
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
