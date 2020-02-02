// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Presentation.Services
{
    /// <summary>
    /// An interface representing a modal file save dialog.
    /// </summary>
    public interface IFileSaveModalDialog : IFileModalDialog
    {
        /// <summary>
        /// Gets the file path selected by the user.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Gets or sets the default extension to apply when the user type a file name without extension.
        /// </summary>
        string DefaultExtension { get; set; }
    }
}
