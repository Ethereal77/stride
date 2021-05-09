// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Presentation.Services
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
