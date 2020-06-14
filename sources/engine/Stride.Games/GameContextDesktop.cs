// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Games
{
    /// <summary>
    ///   Base class for all <see cref="GameContext"/> on the Windows platform.
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    public abstract class GameContextDesktop<TControl> : GameContext<TControl>
    {
        /// <inheritDoc/>
        protected GameContextDesktop(TControl control, int requestedWidth = 0, int requestedHeight = 0, bool isUserManagingRun = false)
            : base(control, requestedWidth, requestedHeight)
        {
            IsUserManagingRun = isUserManagingRun;
        }
    }
}
