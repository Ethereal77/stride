// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma warning disable SA1402 // File may only contain a single type

using System;
using System.Reflection;

using Xenko.Graphics;

namespace Xenko.Games
{
    /// <summary>
    /// Contains context used to render the game (Control for WinForm, a DrawingSurface for WP8...etc.).
    /// </summary>
    public abstract class GameContext
    {
        /// <summary>
        /// Context type of this instance.
        /// </summary>
        public AppContextType ContextType { get; protected set; }

        /// <summary>
        /// Indicating whether the user will call the main loop. E.g. Xenko is used as a library.
        /// </summary>
        public bool IsUserManagingRun { get; protected set; }

        // TODO: remove these requested values.

        /// <summary>
        /// The requested width.
        /// </summary>
        internal int RequestedWidth;

        /// <summary>
        /// The requested height.
        /// </summary>
        internal int RequestedHeight;

        /// <summary>
        /// The requested back buffer format.
        /// </summary>
        internal PixelFormat RequestedBackBufferFormat;

        /// <summary>
        /// The requested depth stencil format.
        /// </summary>
        internal PixelFormat RequestedDepthStencilFormat;

        /// <summary>
        /// THe requested graphics profiles.
        /// </summary>
        internal GraphicsProfile[] RequestedGraphicsProfile;

        /// <summary>
        /// The device creation flags that will be used to create the <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <value>The device creation flags.</value>
        public DeviceCreationFlags DeviceCreationFlags;

        /// <summary>
        /// Indicate whether the game must initialize the default database when it starts running.
        /// </summary>
        public bool InitializeDatabase = true;

        /// <summary>
        /// Product name of game.
        /// TODO: Provide proper access title through code and game studio
        /// </summary>
        internal static string ProductName
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                var productAttribute = assembly?.GetCustomAttribute<AssemblyProductAttribute>();
                return productAttribute?.Product ?? "Xenko Game";
            }
        }

        /// <summary>
        /// Product location of game.
        /// TODO: Only used for retrieving game's icon. See ProductName for future refactoring
        /// </summary>
        public static string ProductLocation
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                return assembly?.Location;
            }
        }

        // This code is for backward compatibility only where the generated games
        // would not explicitly create the context, but would just use a Winform
#if XENKO_UI_WINFORMS || XENKO_UI_WPF
        /// <summary>
        /// Performs an implicit conversion from <see cref="Control"/> to <see cref="GameContextWinforms"/>.
        /// </summary>
        /// <param name="control">Winform control</param>
        /// <returns>The result of the conversion.</returns>
        [Obsolete("Use new GameContextWinforms(control) instead.")]
        public static implicit operator GameContext(System.Windows.Forms.Control control)
        {
            return new GameContextWinforms(control);
        }
#endif
    }

    /// <summary>
    /// Generic version of <see cref="GameContext"/>. The later is used to describe a generic game Context.
    /// This version enables us to constraint the game context to a specifc toolkit and ensures a better cohesion
    /// between the various toolkit specific classes, such as InputManager, GameWindow.
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    public abstract class GameContext<TK> : GameContext
    {
        /// <summary>
        /// Underlying control associated with context.
        /// </summary>
        public TK Control { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameContext" /> class.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        protected GameContext(TK control, int requestedWidth = 0, int requestedHeight = 0)
        {
            Control = control;
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
        }
    }
}
