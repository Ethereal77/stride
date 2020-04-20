// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Games
{
    /// <summary>
    /// An interface that is called by <see cref="GameBase.Update"/>.
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Occurs when the <see cref="Enabled"/> property changes.
        /// </summary>
        event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Occurs when the <see cref="UpdateOrder"/> property changes.
        /// </summary>
        event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// This method is called when this game component is updated.
        /// </summary>
        /// <param name="gameTime">The current timing.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Gets a value indicating whether the game component's Update method should be called by <see cref="GameBase.Update"/>.
        /// </summary>
        /// <value><c>true</c> if update is enabled; otherwise, <c>false</c>.</value>
        bool Enabled { get; }

        /// <summary>
        /// Gets the update order relative to other game components. Lower values are updated first.
        /// </summary>
        /// <value>The update order.</value>
        int UpdateOrder { get; }
    }
}
