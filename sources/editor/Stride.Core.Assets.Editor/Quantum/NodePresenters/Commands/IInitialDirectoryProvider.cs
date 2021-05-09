// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.IO;

namespace Stride.Core.Assets.Editor.Quantum.NodePresenters.Commands
{
    /// <summary>
    /// An interface representing an object able to provide the initial directory for commands spawning file/folder/assets dialogs.
    /// </summary>
    public interface IInitialDirectoryProvider
    {
        /// <summary>
        /// Retrieves the initial directory to use for the command.
        /// </summary>
        /// <param name="currentPath">The path of the current value.</param>
        /// <returns>The initial directory to use for the command.</returns>
        UDirectory GetInitialDirectory(UDirectory currentPath);
    }
}
