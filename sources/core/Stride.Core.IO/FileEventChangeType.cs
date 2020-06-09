// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Defines the type of changes a <see cref="DirectoryWatcher"/> may detect on a file.
    /// </summary>
    [Flags]
    public enum FileEventChangeType
    {
        // NOTE: This must match exactly the System.IO.WatcherChangeTypes

        Created = 1,
        Deleted = 2,
        Changed = 4,
        Renamed = 8,
        All = Renamed | Changed | Deleted | Created
    }
}
