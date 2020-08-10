// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Reflection;

using Stride.Graphics;

namespace Stride.Games
{
    /// <summary>
    ///   Represents a class that contains context information used to render the game (Control for WinForm, DrawingSurface for WPF, etc).
    /// </summary>
    public abstract class GameContext
    {
        /// <summary>
        ///   Gets the context type of this instance.
        /// </summary>
        public AppContextType ContextType { get; protected set; }

        /// <summary>
        ///   Gets a value indicating whether the user will call the main loop.
        /// </summary>
        /// <remarks>
        ///    This is useful if Stride is used as a library.
        /// </remarks>
        public bool IsUserManagingRun { get; protected set; }

        /// <summary>
        ///   Gets the main loop callback to be called when <see cref="IsUserManagingRun"/> is <c>true</c>.
        /// </summary>
        /// <value>The run loop.</value>
        public Action RunCallback { get; internal set; }

        /// <summary>
        ///   Gets the exit callback to be called when exiting the game when <see cref="IsUserManagingRun"/> is <c>true</c>.
        /// </summary>
        /// <value>The exit callback.</value>
        public Action ExitCallback { get; internal set; }

        // TODO: Remove these requested values.

        /// <summary>
        ///   The requested width.
        /// </summary>
        internal int RequestedWidth;

        /// <summary>
        ///   The requested height.
        /// </summary>
        internal int RequestedHeight;

        /// <summary>
        ///   The requested back buffer format.
        /// </summary>
        internal PixelFormat RequestedBackBufferFormat;

        /// <summary>
        ///   The requested depth stencil format.
        /// </summary>
        internal PixelFormat RequestedDepthStencilFormat;

        /// <summary>
        ///   THe requested graphics profiles.
        /// </summary>
        internal GraphicsProfile[] RequestedGraphicsProfile;

        /// <summary>
        ///   The device creation flags that will be used to create the <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <value>The device creation flags.</value>
        public DeviceCreationFlags DeviceCreationFlags;

        /// <summary>
        ///   Indicate whether the game must initialize the default database when it starts running.
        /// </summary>
        public bool InitializeDatabase = true;

        /// <summary>
        ///   Gets the product name of the game.
        /// </summary>
        // TODO: Provide proper access title through code and Game Studio
        internal static string ProductName
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                var productAttribute = assembly?.GetCustomAttribute<AssemblyProductAttribute>();
                return productAttribute?.Product ?? "Stride Game";
            }
        }

        /// <summary>
        ///   Gets the location of the game.
        /// </summary>
        // TODO: Only used for retrieving game's icon. See ProductName for future refactoring
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
#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF
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
    ///   Represents a generic <see cref="GameContext"/> constrained to a specific toolkit (WinForms, WPF, etc).
    /// </summary>
    /// <typeparam name="TControl">Type of control on which the game will render.</typeparam>
    /// <remarks>
    ///   This version ensures a better cohesion between the various toolkit specific classes, such as InputManager, GameWindow, etc.
    /// </remarks>
    public abstract class GameContext<TControl> : GameContext
    {
        /// <summary>
        ///   Gets the underlying control associated with this context.
        /// </summary>
        public TControl Control { get; internal set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GameContext" /> class.
        /// </summary>
        /// <param name="control">The control on which the game will run.</param>
        /// <param name="requestedWidth">Requested width of the control.</param>
        /// <param name="requestedHeight">Requested height of the control.</param>
        /// <param name="isUserManagingRun">A value indicating whether the user manages the main loop.</param>
        protected GameContext(TControl control, int requestedWidth = 0, int requestedHeight = 0, bool isUserManagingRun = false)
        {
            Control = control;
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            IsUserManagingRun = isUserManagingRun;
        }
    }
}
