// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Presentation.Services
{
    /// <summary>
    /// An interface representing a modal folder selection dialog.
    /// </summary>
    public interface IFolderOpenModalDialog : IModalDialog
    {
        /// <summary>
        /// Gets or sets the initial directory of the folder dialog.
        /// </summary>
        string InitialDirectory { get; set; }

        /// <summary>
        /// Gets the directory selected by the user.
        /// </summary>
        string Directory { get; }
    }
}
