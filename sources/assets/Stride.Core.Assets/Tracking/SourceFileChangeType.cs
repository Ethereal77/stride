// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Assets.Tracking
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
