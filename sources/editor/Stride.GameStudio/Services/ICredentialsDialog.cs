// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Presentation.Services;

namespace Stride.GameStudio.Services
{
    /// <summary>
    /// An interface representing a modal dialog to prompt user some credentials
    /// to access a remote host.
    /// </summary>
    public interface ICredentialsDialog : IModalDialog
    {
        /// <summary>
        /// Are credentials obtained valid after closing the modal dialog?
        /// </summary>
        bool AreCredentialsValid { get; }
    }

}
