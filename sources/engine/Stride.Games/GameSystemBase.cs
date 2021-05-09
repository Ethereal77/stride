// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Serialization.Contents;
using Stride.Graphics;

namespace Stride.Games
{
    /// <summary>
    ///   Represents the base class for a game system.
    /// </summary>
    /// <remarks>
    ///   A game system is a component of a game that can be updated and/or rendered in a frequent basis,
    ///   can load and unload content, can register services and do any kind of processing for the game.
    /// </remarks>
    public abstract class GameSystemBase : ComponentBase, IGameSystemBase, IUpdateable, IDrawable, IContentable
    {
        private int updateOrder, drawOrder;
        private bool enabled, visible;

        private IGraphicsDeviceService graphicsDeviceService;

        /// <summary>
        ///   Initializes a new instance of the <see cref="GameSystemBase" /> class.
        /// </summary>
        /// <param name="registry">The service registry.</param>
        /// <remarks>
        ///   The <c>GameSystemBase</c> is expecting the following services to be registered: <see cref="IGame"/> and <see cref="IContentManager"/>.
        /// </remarks>
        protected GameSystemBase([NotNull] IServiceRegistry registry)
        {
            Services = registry ?? throw new ArgumentNullException(nameof(registry));
            Game = (GameBase) Services.GetService<IGame>();
        }

        /// <summary>
        ///   Gets the <see cref="IGame"/> associated with this <see cref="GameSystemBase"/>.
        /// </summary>
        /// <value>The game. This value can be <c>null</c> in a mock environment.</value>
        [CanBeNull]
        public GameBase Game { get; }

        /// <summary>
        ///   Gets the services registry.
        /// </summary>
        /// <value>The services registry.</value>
        /// <remarks>
        ///   This registry can be used to query for services registered by other systems, or to register
        ///   services that can be consumed by other parts of the game.
        /// </remarks>
        [NotNull]
        public IServiceRegistry Services { get; }

        /// <summary>
        ///   Gets the content manager.
        /// </summary>
        /// <value>The content manager. This value can be <c>null</c> in a mock environment.</value>
        /// <remarks>
        ///   The content manager can be used to load and unload content and data needed by the system.
        /// </remarks>
        [CanBeNull]
        protected IContentManager Content { get; private set; }

        /// <summary>
        ///   Gets the graphics device.
        /// </summary>
        /// <value>The graphics device. This can be <c>null</c> if the graphics device has not yet been initialized.</value>
        protected GraphicsDevice GraphicsDevice => graphicsDeviceService?.GraphicsDevice;

        #region IDrawable Members

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        public virtual bool BeginDraw()
        {
            return true;
        }

        public virtual void Draw(GameTime gameTime) { }

        public virtual void EndDraw() { }

        public bool Visible
        {
            get => visible;

            set
            {
                if (visible != value)
                {
                    visible = value;
                    OnVisibleChanged(EventArgs.Empty);
                }
            }
        }

        public int DrawOrder
        {
            get => drawOrder;

            set
            {
                if (drawOrder != value)
                {
                    drawOrder = value;
                    OnDrawOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region IGameSystemBase Members

        public virtual void Initialize() { }

        protected void InitGraphicsDeviceService()
        {
            if (graphicsDeviceService is null)
                graphicsDeviceService = Services.GetService<IGraphicsDeviceService>();
        }

        #endregion

        #region IUpdateable Members

        public event EventHandler<EventArgs> EnabledChanged;

        public event EventHandler<EventArgs> UpdateOrderChanged;

        public virtual void Update(GameTime gameTime) { }

        public bool Enabled
        {
            get => enabled;

            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    OnEnabledChanged(EventArgs.Empty);
                }
            }
        }

        public int UpdateOrder
        {
            get => updateOrder;

            set
            {
                if (updateOrder != value)
                {
                    updateOrder = value;
                    OnUpdateOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        protected virtual void OnDrawOrderChanged(object source, EventArgs e)
        {
            DrawOrderChanged?.Invoke(source, e);
        }

        private void OnVisibleChanged(EventArgs e)
        {
            VisibleChanged?.Invoke(this, e);
        }

        private void OnEnabledChanged(EventArgs e)
        {
            EnabledChanged?.Invoke(this, e);
        }

        protected virtual void OnUpdateOrderChanged(object source, EventArgs e)
        {
            UpdateOrderChanged?.Invoke(source, e);
        }

        #region Implementation of IContentable

        void IContentable.LoadContent()
        {
            Content = Services.GetService<IContentManager>();

            InitGraphicsDeviceService();

            LoadContent();
        }

        void IContentable.UnloadContent()
        {
            UnloadContent();
        }

        protected virtual void LoadContent() { }

        protected virtual void UnloadContent() { }

        #endregion
    }
}
