// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Threading;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Graphics;
using Stride.Games;
using Stride.Streaming;

using ComponentBase = Stride.Core.ComponentBase;
using IServiceRegistry = Stride.Core.IServiceRegistry;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents the context information and services needed for the Renderer in Stride.
    /// </summary>
    public sealed class RenderContext : ComponentBase
    {
        private const string SharedImageEffectContextKey = "__SharedRenderContext__";

        private readonly ThreadLocal<RenderDrawContext> threadContext;
        private readonly object threadContextLock = new object();

        // Used for API that don't support multiple command lists
        internal CommandList SharedCommandList;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderContext" /> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        internal RenderContext(IServiceRegistry services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Effects = services.GetSafeServiceAs<EffectSystem>();
            GraphicsDevice = services.GetSafeServiceAs<IGraphicsDeviceService>().GraphicsDevice;
            Allocator = services.GetSafeServiceAs<GraphicsContext>().Allocator ?? new GraphicsResourceAllocator(GraphicsDevice).DisposeBy(GraphicsDevice);
            StreamingManager = services.GetService<StreamingManager>();

            threadContext = new ThreadLocal<RenderDrawContext>(() =>
            {
                lock (threadContextLock)
                {
                    return new RenderDrawContext(Services, this, new GraphicsContext(GraphicsDevice, Allocator));
                }
            }, true);
        }

        /// <summary>
        ///   Occurs when a renderer is initialized.
        /// </summary>
        public event Action<IGraphicsRendererCore> RendererInitialized;

        /// <summary>
        ///   Gets the effects system.
        /// </summary>
        /// <value>The effects system.</value>
        public EffectSystem Effects { get; }

        /// <summary>
        ///   Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        ///   Gets the services registry.
        /// </summary>
        /// <value>The services registry.</value>
        public IServiceRegistry Services { get; }

        /// <summary>
        ///   Gets or sets the time.
        /// </summary>
        public GameTime Time { get; set; }

        /// <summary>
        ///   Gets the <see cref="GraphicsResource"/> allocator.
        /// </summary>
        /// <value>The allocator for graphics resources.</value>
        public GraphicsResourceAllocator Allocator { get; }

        /// <summary>
        ///   Gets or sets the current render system.
        /// </summary>
        /// <value>The current render system.</value>
        public RenderSystem RenderSystem { get; set; }

        /// <summary>
        ///   Gets or sets the current visibility group from the <see cref="RenderSystem"/>.
        /// </summary>
        public VisibilityGroup VisibilityGroup { get; set; }

        /// <summary>
        ///   The current render output description (used during the collect phase).
        /// </summary>
        public RenderOutputDescription RenderOutput;

        /// <summary>
        ///   The current render viewport (used during the collect phase).
        /// </summary>
        public ViewportState ViewportState;

        /// <summary>
        ///   Gets the current render view.
        /// </summary>
        /// <value>The current <see cref="Rendering.RenderView"/>.</value>
        public RenderView RenderView { get; set; }

        /// <summary>
        ///   Gets or sets the streaming manager.
        /// </summary>
        /// <value>The streaming manager.</value>
        [CanBeNull]
        public StreamingManager StreamingManager { get; set; }


        protected override void Destroy()
        {
            foreach (var renderDrawContext in threadContext.Values)
            {
                renderDrawContext.Dispose();
            }
            threadContext.Dispose();

            base.Destroy();
        }

        /// <summary>
        ///   Gets the global shared context.
        /// </summary>
        /// <param name="services">The services registry.</param>
        /// <returns>The global shared <see cref="RenderContext"/>.</returns>
        public static RenderContext GetShared(IServiceRegistry services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            // Store RenderContext shared into the GraphicsDevice
            var graphicsDevice = services.GetSafeServiceAs<IGraphicsDeviceService>().GraphicsDevice;
            return graphicsDevice.GetOrCreateSharedData(SharedImageEffectContextKey, d => new RenderContext(services));
        }

        /// <summary>
        ///   Saves a viewport and restores it after using it.
        /// </summary>
        public ViewportRestore SaveViewportAndRestore()
        {
            return new ViewportRestore(this);
        }

        /// <summary>
        ///   Saves a render output and restores it after using it.
        /// </summary>
        public RenderOutputRestore SaveRenderOutputAndRestore()
        {
            return new RenderOutputRestore(this);
        }

        /// <summary>
        ///   Pushes a render view and restores it after using it.
        /// </summary>
        /// <param name="renderView">The render view.</param>
        public RenderViewRestore PushRenderViewAndRestore(RenderView renderView)
        {
            var result = new RenderViewRestore(this);
            RenderView = renderView;
            return result;
        }

        public RenderDrawContext GetThreadContext() => threadContext.Value;

        public void Reset()
        {
            // NOTE: Returns IList so don't use foreach otherwise an object will be allocated
            var contextList = threadContext.Values;
            for (int i = 0; i < contextList.Count; i++)
            {
                var context = contextList[i];
                context.ResourceGroupAllocator.Reset(context.CommandList);
            }
        }

        public void Flush()
        {
            // NOTE: Returns IList so don't use foreach otherwise an object will be allocated
            var contextList = threadContext.Values;
            for (int i = 0; i < contextList.Count; i++)
            {
                var context = contextList[i];
                context.ResourceGroupAllocator.Flush();
                context.QueryManager.Flush();
            }
        }

        internal void OnRendererInitialized(IGraphicsRendererCore obj)
        {
            RendererInitialized?.Invoke(obj);
        }

        public struct ViewportRestore : IDisposable
        {
            private readonly RenderContext context;
            private readonly ViewportState previousValue;

            public ViewportRestore(RenderContext renderContext)
            {
                context = renderContext;
                previousValue = renderContext.ViewportState;
            }

            public void Dispose()
            {
                context.ViewportState = previousValue;
            }
        }

        public struct RenderOutputRestore : IDisposable
        {
            private readonly RenderContext context;
            private readonly RenderOutputDescription previousValue;

            public RenderOutputRestore(RenderContext renderContext)
            {
                context = renderContext;
                previousValue = renderContext.RenderOutput;
            }

            public void Dispose()
            {
                context.RenderOutput = previousValue;
            }
        }

        public struct RenderViewRestore : IDisposable
        {
            private readonly RenderContext context;
            private readonly RenderView previousValue;

            public RenderViewRestore(RenderContext renderContext)
            {
                context = renderContext;
                previousValue = renderContext.RenderView;
            }

            public void Dispose()
            {
                context.RenderView = previousValue;
            }
        }
    }
}
