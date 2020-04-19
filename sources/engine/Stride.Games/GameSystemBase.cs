// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;
using Xenko.Core.Annotations;
using Xenko.Core.Serialization.Contents;
using Xenko.Graphics;

namespace Xenko.Games
{
    /// <summary>
    /// Base class for a <see cref="GameSystemBase"/> component.
    /// </summary>
    /// <remarks>
    /// A <see cref="GameSystemBase"/> component can be used to 
    /// </remarks>
    public abstract class GameSystemBase : ComponentBase, IGameSystemBase, IUpdateable, IDrawable, IContentable
    {
        private int drawOrder;
        private bool enabled;
        private int updateOrder;
        private bool visible;
        private IGraphicsDeviceService graphicsDeviceService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSystemBase" /> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <remarks>
        /// The GameSystem is expecting the following services to be registered: <see cref="IGame"/> and <see cref="IContentManager"/>.
        /// </remarks>
        protected GameSystemBase([NotNull] IServiceRegistry registry)
        {
            Services = registry ?? throw new ArgumentNullException(nameof(registry));
            Game = (GameBase)Services.GetService<IGame>();
        }

        /// <summary>
        /// Gets the <see cref="Game"/> associated with this <see cref="GameSystemBase"/>. This value can be null in a mock environment.
        /// </summary>
        /// <value>The game.</value>
        /// <remarks>This value can be null</remarks>
        [CanBeNull]
        public GameBase Game { get; }

        /// <summary>
        /// Gets the services registry.
        /// </summary>
        /// <value>The services registry.</value>
        [NotNull]
        public IServiceRegistry Services { get; }

        /// <summary>
        /// Gets the content manager. This value can be null in a mock environment.
        /// </summary>
        /// <value>The content.</value>
        [CanBeNull]
        protected IContentManager Content { get; private set; }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        protected GraphicsDevice GraphicsDevice => graphicsDeviceService?.GraphicsDevice;

        #region IDrawable Members

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        public virtual bool BeginDraw()
        {
            return true;
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public virtual void EndDraw()
        {
        }

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

        public virtual void Initialize()
        {
        }

        private void InitGraphicsDeviceService()
        {
            if (graphicsDeviceService == null)
            {
                graphicsDeviceService = Services.GetService<IGraphicsDeviceService>();
            }
        }

        #endregion

        #region IUpdateable Members

        public event EventHandler<EventArgs> EnabledChanged;

        public event EventHandler<EventArgs> UpdateOrderChanged;

        public virtual void Update(GameTime gameTime)
        {
        }

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

        protected virtual void LoadContent()
        {
        }

        protected virtual void UnloadContent()
        {
        }

        #endregion
    }
}
