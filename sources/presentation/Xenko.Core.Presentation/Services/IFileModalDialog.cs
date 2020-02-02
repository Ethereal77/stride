// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

namespace Xenko.Core.Presentation.Services
{
    /// <summary>
    /// An interface representing modal file dialogs.
    /// </summary>
    public interface IFileModalDialog : IModalDialog
    {
        /// <summary>
        /// Gets or sets the list of filter to use in the file dialog.
        /// </summary>
        IList<FileDialogFilter> Filters { get; set; }

        /// <summary>
        /// Gets or sets the initial directory of the file dialog.
        /// </summary>
        string InitialDirectory { get; set; }

        /// <summary>
        /// Gets or sets the default file name to display when opening the file dialog.
        /// </summary>
        string DefaultFileName { get; set; }
    }
}
