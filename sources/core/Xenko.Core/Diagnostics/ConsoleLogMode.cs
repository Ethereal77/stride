// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Diagnostics
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
