// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Input
{
    /// <summary>
    /// Event for when a <see cref="IGamePadDevice"/>'s index changed
    /// </summary>
    public class GamePadIndexChangedEventArgs : EventArgs
    {
        /// <summary>
        /// New device index
        /// </summary>
        public int Index;

        /// <summary>
        /// if <c>true</c>, this change was initiate by the device
        /// </summary>
        public bool IsDeviceSideChange;
    }
}