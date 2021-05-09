// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;
using Stride.Core.Serialization.Contents;
using Stride.Graphics;

namespace Stride.Games
{
    /// <summary>
    ///   Defines the interface of a class that represents a game application.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        ///   Occurs when the application is activated (is in the foreground).
        /// </summary>
        event EventHandler<EventArgs> Activated;

        /// <summary>
        ///   Occurs when the application is deactivated (is not in the foreground).
        /// </summary>
        event EventHandler<EventArgs> Deactivated;

        /// <summary>
        ///   Occurs when the application is exiting.
        /// </summary>
        event EventHandler<EventArgs> Exiting;

        /// <summary>
        ///   Occurs when the main window has been created.
        /// </summary>
        event EventHandler<EventArgs> WindowCreated;


        /// <summary>
        ///   Gets the current game time.
        /// </summary>
        /// <value>The current game time.</value>
        GameTime UpdateTime { get; }

        /// <summary>
        ///   Gets the current draw time.
        /// </summary>
        /// <value>The current draw time.</value>
        GameTime DrawTime { get; }

        /// <summary>
        ///   Gets the draw interpolation factor.
        /// </summary>
        /// <value>
        ///   The draw interpolation factor: (<see cref="UpdateTime"/> - <see cref="DrawTime"/>) / <see cref="TargetElapsedTime"/>.
        ///   <para/>
        ///   If <see cref="IsFixedTimeStep"/> is <c>false</c>, it will be 0 as <see cref="UpdateTime"/> and <see cref="DrawTime"/> will be equal.
        /// </value>
        float DrawInterpolationFactor { get; }

        /// <summary>
        ///   Gets the content manager.
        /// </summary>
        /// <value>The content manager.</value>
        ContentManager Content { get; }

        /// <summary>
        ///   Gets the game systens registered with this game.
        /// </summary>
        /// <value>The game systens.</value>
        GameSystemCollection GameSystems { get; }

        /// <summary>
        ///   Gets the game context.
        /// </summary>
        /// <value>The game context.</value>
        GameContext Context { get; }

        /// <summary>
        ///   Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        ///   Gets the graphics context.
        /// </summary>
        /// <value>The graphics context.</value>
        GraphicsContext GraphicsContext { get; }

        /// <summary>
        ///   Gets or sets the inactive sleep time.
        /// </summary>
        /// <value>The inactive sleep time.</value>
        TimeSpan InactiveSleepTime { get; set; }

        /// <summary>
        ///   Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        bool IsActive { get; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance should use a fixed time step.
        /// </summary>
        /// <value><c>true</c> if this instance should use a fixed time step; otherwise, <c>false</c>.</value>
        bool IsFixedTimeStep { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether draw can happen as fast as possible, even when <see cref="IsFixedTimeStep"/> is set.
        /// </summary>
        /// <value><c>true</c> if this instance allows desychronized drawing; otherwise, <c>false</c>.</value>
        bool IsDrawDesynchronized { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the mouse should be visible.
        /// </summary>
        /// <value><c>true</c> if the mouse should be visible; otherwise, <c>false</c>.</value>
        bool IsMouseVisible { get; set; }

        /// <summary>
        ///   Gets the launch parameters.
        /// </summary>
        /// <value>The launch parameters.</value>
        LaunchParameters LaunchParameters { get; }

        /// <summary>
        ///   Gets a value indicating whether the game loop is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        ///   Gets the service container.
        /// </summary>
        /// <value>The service container.</value>
        ServiceRegistry Services { get; }

        /// <summary>
        ///   Gets or sets the target elapsed time.
        /// </summary>
        /// <value>The target elapsed time.</value>
        TimeSpan TargetElapsedTime { get; set; }

        /// <summary>
        ///   Gets the abstract window.
        /// </summary>
        /// <value>The window.</value>
        GameWindow Window { get; }
    }
}
