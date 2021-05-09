// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Diagnostics
{
    /// <summary>
    /// Defines how the console is opened.
    /// </summary>
    public enum ConsoleLogMode
    {
        /// <summary>
        /// The console should be visible only in debug and if there is a message, otherwise it is not visible.
        /// </summary>
        Auto,

        /// <summary>
        /// Same as <see cref="Auto"/>
        /// </summary>
        Default = Auto,

        /// <summary>
        /// The console should not be visible.
        /// </summary>
        None,

        /// <summary>
        /// The console should be always visible
        /// </summary>
        Always,
    }
}
