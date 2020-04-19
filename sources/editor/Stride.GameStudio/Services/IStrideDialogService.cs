// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.Services;

namespace Xenko.GameStudio.Services
{
    /// <summary>
    /// This interface represents the dialog service used for the GameStudio. It extends <see cref="IEditorDialogService"/> with some GameStudio-specific dialogs.
    /// </summary>
    public interface IXenkoDialogService : IEditorDialogService
    {
        /// <summary>
        /// Creates and displays the credentials dialog
        /// </summary>
        /// <returns>An instace of the <see cref="ICredentialsDialog"/> interface.</returns>
        ICredentialsDialog CreateCredentialsDialog();

        void ShowAboutPage();

    }
}
