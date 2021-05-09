// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows.Input;

namespace Stride.Core.Presentation.Commands
{
    /// <summary>
    /// An interface representing an implementation of <see cref="ICommand"/> with additional properties.
    /// </summary>
    public interface ICommandBase : ICommand
    {
        /// <summary>
        /// Indicates whether the command can be executed or not.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Executes the command with a <c>null</c> parameter.
        /// </summary>
        void Execute();
    }
}
