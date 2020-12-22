// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.IO
{
    /// <summary>
    ///   Specifies whether to search the current directory, or the current directory and
    ///    all subdirectories.
    /// </summary>
    public enum VirtualSearchOption
    {
        /// <summary>
        ///   Includes only the current directory in a search operation.
        /// </summary>
        TopDirectoryOnly,

        /// <summary>
        ///   Includes the current directory and all its subdirectories in a search operation.
        /// </summary>
        AllDirectories
    }
}
