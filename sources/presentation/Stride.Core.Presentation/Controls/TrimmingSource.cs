// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Presentation.Controls
{
    /// <summary>
    /// Represents the position at which the text will be trimmed and the ellipsis will be inserted.
    /// </summary>
    public enum TrimmingSource
    {
        /// <summary>
        /// The text will be trimmed from the beginning.
        /// </summary>
        Begin,
        /// <summary>
        /// The text will be trimmed from the middle.
        /// </summary>
        Middle,
        /// <summary>
        /// The text will be trimmed from the end.
        /// </summary>
        End
    }
}
