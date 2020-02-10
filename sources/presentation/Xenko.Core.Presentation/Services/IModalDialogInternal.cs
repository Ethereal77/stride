// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Presentation.Services
{
    /// <summary>
    /// An internal interface representing a modal dialog.
    /// </summary>
    public interface IModalDialogInternal : IModalDialog
    {
        /// <summary>
        /// Gets or sets the result of the modal dialog.
        /// </summary>
        DialogResult Result { get; set; }
    }
}
