// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.IO
{
    /// <summary>
    /// Change type of file used by <see cref="FileEvent"/> and <see cref="DirectoryWatcher"/>.
    /// </summary>
    [Flags]
    public enum FileEventChangeType
    {
        // This enum must match exactly the System.IO.WatcherChangeTypes

        Created = 1,
        Deleted = 2,
        Changed = 4,
        Renamed = 8,
        All = Renamed | Changed | Deleted | Created,
    }
}
