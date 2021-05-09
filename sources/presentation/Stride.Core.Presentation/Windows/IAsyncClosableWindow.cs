// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

namespace Stride.Core.Presentation.Windows
{
    /// <summary>
    /// Represents a window that can asynchronously close and/or cancel a request to close.
    /// </summary>
    public interface IAsyncClosableWindow
    {
        /// <summary>
        /// Attempts to close the window.
        /// </summary>
        /// <returns>
        /// A task that completes either when the window is closed, or when the request to close has been cancelled.
        /// The result of the task indicates whether the window has been closed.
        /// </returns>
        Task<bool> TryClose();
    }
}