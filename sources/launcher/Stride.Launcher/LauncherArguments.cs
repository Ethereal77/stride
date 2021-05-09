// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.LauncherApp
{
    /// <summary>
    ///   Represents the arguments passed to the Launcher process.
    /// </summary>
    internal struct LauncherArguments
    {
        /// <summary>
        ///   Defines the types of actions this Launcher process should perform.
        /// </summary>
        public enum ActionType
        {
            Run,
            Uninstall
        }

        /// <summary>
        ///   The list of actions this process should perform.
        /// </summary>
        public List<ActionType> Actions;
    }
}
