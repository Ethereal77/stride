// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Games;

namespace Stride.Input
{
    /// <summary>
    /// Arguments for input pre update event
    /// </summary>
    public class InputPreUpdateEventArgs : EventArgs
    {
        /// <summary>
        /// The game time passed to <see cref="InputManager.Update"/>
        /// </summary>
        public GameTime GameTime;
    }
}