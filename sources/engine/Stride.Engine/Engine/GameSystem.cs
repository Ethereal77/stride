// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Core;
using Stride.Games;

namespace Stride.Engine
{
    public abstract class GameSystem : GameSystemBase
    {
        protected GameSystem(IServiceRegistry registry) : base(registry)
        {
        }

        /// <summary>
        /// Gets the <see cref="Game"/> associated with this <see cref="GameSystemBase"/>. This value can be null in a mock environment.
        /// </summary>
        /// <value>The game.</value>
        /// <remarks>This value can be null</remarks>
        public new Game Game => (Game)base.Game;
    }
}
