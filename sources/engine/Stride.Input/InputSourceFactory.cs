// Copyright (c) Stride contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using Stride.Games;

namespace Stride.Input
{
    /// <summary>
    ///   Defines functions to create an <see cref="IInputSource"/> for a platform-specific window given
    ///   the associated <see cref="GameContext"/>.
    /// </summary>
    public static class InputSourceFactory
    {
        /// <summary>
        ///   Creates a new input source for the window provided by a <see cref="GameContext"/>.
        /// </summary>
        /// <param name="context">The context containing the platform specific window.</param>
        /// <returns>An input source for the window.</returns>
        public static IInputSource NewWindowInputSource(GameContext context)
        {
            switch (context.ContextType)
            {
#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF
                case AppContextType.Desktop:
                    var winformsContext = (GameContextWinforms) context;
                    return new InputSourceWinforms(winformsContext.Control);
#endif
                default:
                    throw new InvalidOperationException("GameContext type is not supported by the InputManager.");
            }
        }
    }
}
