// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Assets.Tracking
{
    /// <summary>
    /// Describes a change related to the source files used by an asset.
    /// </summary>
    public enum SourceFileChangeType
    {
        /// <summary>
        /// The change occurred in an asset that now has a different list of source files.
        /// </summary>
        Asset,
        /// <summary>
        /// The change occurred in an source file that has been modified externally.
        /// </summary>
        SourceFile
    }
}
