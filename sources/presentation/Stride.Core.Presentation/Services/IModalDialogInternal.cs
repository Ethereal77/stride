// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Presentation.Services
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
