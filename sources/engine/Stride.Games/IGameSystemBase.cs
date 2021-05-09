// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Games
{
    /// <summary>
    /// Defines a generic game system.
    /// </summary>
    public interface IGameSystemBase : IComponent
    {
        /// <summary>
        /// This method is called when the component is added to the game.
        /// </summary>
        /// <remarks>
        /// This method can be used for tasks like querying for services the component needs and setting up non-graphics resources.
        /// </remarks>
        void Initialize();
    }
}
