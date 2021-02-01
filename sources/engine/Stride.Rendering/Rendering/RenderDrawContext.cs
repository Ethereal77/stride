// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Stride.Core;
using Stride.Graphics;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents an object that provides context information during <see cref="IGraphicsRenderer.Draw"/>.
    /// </summary>
    public sealed class RenderDrawContext : ComponentBase
    {
        // States
        private int currentStateIndex = -1;
        private readonly List<RenderTargetsState> allocatedStates = new List<RenderTargetsState>(10);

        private readonly Dictionary<Type, DrawEffect> sharedEffects = new Dictionary<Type, DrawEffect>();


        public RenderDrawContext(IServiceRegistry services, RenderContext renderContext, GraphicsContext graphicsContext)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            RenderContext = renderContext;
            ResourceGroupAllocator = graphicsContext.ResourceGroupAllocator;
            GraphicsDevice = RenderContext.GraphicsDevice;
            GraphicsContext = graphicsContext;
            CommandList = graphicsContext.CommandList;
            Resolver = new ResourceResolver(this);
            QueryManager = new QueryManager(CommandList, renderContext.Allocator);
        }


        /// <summary>
        ///   Gets the render context.
        /// </summary>
        public RenderContext RenderContext { get; }

        /// <summary>
        ///   Gets the <see cref="ResourceGroup"/> allocator.
        /// </summary>
        public ResourceGroupAllocator ResourceGroupAllocator { get; }

        /// <summary>
        ///   Gets the command list.
        /// </summary>
        public CommandList CommandList { get; }

        public GraphicsContext GraphicsContext { get; }

        public GraphicsDevice GraphicsDevice { get; }

        public ResourceResolver Resolver { get; }

        public QueryManager QueryManager { get; }

        /// <summary>
        ///   Locks the command list until <see cref="DefaultCommandListLock.Dispose()"/> is called on the returned object.
        /// </summary>
        /// <returns>A <see cref="DefaultCommandListLock"/> that can be used to lock and unlock the command list.</returns>
        public DefaultCommandListLock LockCommandList()
        {
            // This is necessary only during Collect(), Extract() and Prepare() phases, not during Draw().
            // Some graphics API might not require actual locking, in which case this object might do nothing.

            // TODO: Temporary, for now we use the CommandList itself as a lock
            return new DefaultCommandListLock(CommandList);
        }

        /// <summary>
        ///   Remembewrs the render targets and viewport states and restores them whenever <see cref="RenderTargetRestore.Dispose()"/>
        ///   is called on the returned object.
        /// </summary>
        public RenderTargetRestore PushRenderTargetsAndRestore()
        {
            // Check if we need to allocate a new StateAndTargets
            RenderTargetsState newState;
            currentStateIndex++;
            if (currentStateIndex == allocatedStates.Count)
            {
                newState = new RenderTargetsState();
                allocatedStates.Add(newState);
            }
            else
            {
                newState = allocatedStates[currentStateIndex];
            }
            newState.Capture(CommandList);

            return new RenderTargetRestore(this);
        }

        /// <summary>
        ///   Restores the render targets and viewport states previously pushed with <see cref="PushRenderTargetsAndRestore()"/>.
        /// </summary>
        public void PopRenderTargets()
        {
            if (currentStateIndex < 0)
                throw new InvalidOperationException("Cannot pop more than push.");

            var oldState = allocatedStates[currentStateIndex--];
            oldState.Restore(CommandList);
        }

        /// <summary>
        ///   Gets or creates a shared effect.
        /// </summary>
        /// <typeparam name="T">Type of the shared effect.</typeparam>
        /// <returns>A singleton instance of <typeparamref name="T"/>.</returns>
        public T GetSharedEffect<T>() where T : DrawEffect, new()
        {
            // TODO: Add a way to support custom constructor
            lock (sharedEffects)
            {
                if (!sharedEffects.TryGetValue(typeof(T), out DrawEffect effect))
                {
                    effect = new T();
                    sharedEffects.Add(typeof(T), effect);
                    effect.Initialize(RenderContext);
                }

                return (T) effect;
            }
        }

        /// <summary>
        ///   Holds current viewports and render targets.
        /// </summary>
        private class RenderTargetsState
        {
            private const int MaxRenderTargetCount = 8;
            private const int MaxViewportAndScissorRectangleCount = 16;

            public int RenderTargetCount;
            public int ViewportCount;

            public readonly Viewport[] Viewports = new Viewport[MaxViewportAndScissorRectangleCount];
            public readonly Texture[] RenderTargets = new Texture[MaxRenderTargetCount];
            public Texture DepthStencilBuffer;

            public void Capture(CommandList commandList)
            {
                RenderTargetCount = commandList.RenderTargetCount;
                ViewportCount = commandList.ViewportCount;
                DepthStencilBuffer = commandList.DepthStencilBuffer;

                // TODO: Backup scissor rectangles and restore them

                for (int i = 0; i < RenderTargetCount; i++)
                {
                    RenderTargets[i] = commandList.RenderTargets[i];
                }

                for (int i = 0; i < ViewportCount; i++)
                {
                    Viewports[i] = commandList.Viewports[i];
                }
            }

            public void Restore(CommandList commandList)
            {
                commandList.SetRenderTargets(DepthStencilBuffer, RenderTargetCount, RenderTargets);
                commandList.SetViewports(ViewportCount, Viewports);
            }
        }

        /// <summary>
        ///   Represents an object that can restore a previous viewport and render target state when disposed.
        /// </summary>
        public readonly struct RenderTargetRestore : IDisposable
        {
            private readonly RenderDrawContext context;

            public RenderTargetRestore(RenderDrawContext context)
            {
                this.context = context;
            }

            public void Dispose()
            {
                context.PopRenderTargets();
            }
        }
    }
}
