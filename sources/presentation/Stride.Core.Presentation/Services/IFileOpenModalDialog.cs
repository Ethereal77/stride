// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Core.Presentation.Services
{
    /// <summary>
    /// An interface representing a modal file open dialog.
    /// </summary>
    public interface IFileOpenModalDialog : IFileModalDialog
    {
        /// <summary>
        /// Gets or sets whether multi-selection is allowed.
        /// </summary>
        bool AllowMultiSelection { get; set; }

        /// <summary>
        /// Gets the list of file paths selected by the user.
        /// </summary>
        IReadOnlyCollection<string> FilePaths { get; }
    }
}
