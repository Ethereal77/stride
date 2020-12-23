// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;

using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Core.Serialization.Contents;
using Stride.Graphics;

using Buffer = Stride.Graphics.Buffer;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents the base implementation of <see cref="IGraphicsRenderer"/>.
    /// </summary>
    [DataContract]
    public abstract class RendererCoreBase : ComponentBase, IGraphicsRendererCore
    {
        private ProfilingKey profilingKey;

        private readonly List<GraphicsResource> scopedResources = new List<GraphicsResource>();
        private readonly List<IGraphicsRendererCore> subRenderersToUnload;

        private bool isInDrawCore;


        /// <summary>
        ///   Initializes a new instance of the <see cref="RendererCoreBase"/> class.
        /// </summary>
        protected RendererCoreBase() : this(name: null) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="RendererCoreBase" /> class.
        /// </summary>
        /// <param name="name">The name attached to this component. Specify <c>null</c> to use the name of the type automatically.</param>
        protected RendererCoreBase(string name) : base(name)
        {
            Enabled = true;
            subRenderersToUnload = new List<IGraphicsRendererCore>();
            Profiling = true;
        }


        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref="RendererCoreBase"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        /// <userdoc>Enabled if checked, disabled otherwise</userdoc>
        [DataMember(-20)]
        [DefaultValue(true)]
        public virtual bool Enabled { get; set; }

        [DataMemberIgnore]
        public bool Profiling { get; set; }

        [DataMemberIgnore]
        public ProfilingKey ProfilingKey => profilingKey ?? (profilingKey = new ProfilingKey(Name));

        [DataMemberIgnore]
        protected RenderContext Context { get; private set; }

        /// <summary>
        ///   Gets the registry of services.
        /// </summary>
        /// <value>The service registry.</value>
        [DataMemberIgnore]
        protected IServiceRegistry Services { get; private set; }

        /// <summary>
        ///   Gets the content manager.
        /// </summary>
        /// <value>The content manager.</value>
        [DataMemberIgnore]
        protected ContentManager Content { get; private set; }

        /// <summary>
        ///   Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        [DataMemberIgnore]
        protected GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        ///   Gets the effect system.
        /// </summary>
        /// <value>The effect system.</value>
        [DataMemberIgnore]
        protected EffectSystem EffectSystem { get; private set; }

        public bool Initialized { get; private set; }

        public void Initialize(RenderContext context)
        {
            if (context is null)
                throw new ArgumentNullException("context");

            // Unload the previous context if any
            if (Context != null)
            {
                Unload();
            }

            Context = context;
            subRenderersToUnload.Clear();

            // Initialize most common services related to rendering
            Services = Context.Services;
            Content = Services.GetSafeServiceAs<ContentManager>();
            EffectSystem = Services.GetSafeServiceAs<EffectSystem>();
            GraphicsDevice = Services.GetSafeServiceAs<IGraphicsDeviceService>().GraphicsDevice;

            InitializeCore();

            Initialized = true;

            // Notify that a particular renderer has been initialized.
            context.OnRendererInitialized(this);
        }

        protected virtual void InitializeCore() { }

        /// <summary>
        ///   Unloads this instance on dispose.
        /// </summary>
        protected virtual void Unload()
        {
            foreach (var drawEffect in subRenderersToUnload)
            {
                drawEffect.Dispose();
            }
            subRenderersToUnload.Clear();

            Context = null;
        }

        protected virtual void PreDrawCore(RenderDrawContext context) { }

        protected virtual void PostDrawCore(RenderDrawContext context) { }

        /// <summary>
        ///   Gets a buffer with the specified description, scoped for the duration of the <see cref="DrawEffect.DrawCore"/>.
        /// </summary>
        /// <param name="description">The description of the buffer to allocate.</param>
        /// <param name="viewFormat">The element format seen in the shader.</param>
        /// <returns>The new buffer.</returns>
        protected Buffer NewScopedBuffer(BufferDescription description, PixelFormat viewFormat = PixelFormat.None)
        {
            CheckIsInDrawCore();

            return PushScopedResource(Context.Allocator.GetTemporaryBuffer(description, viewFormat));
        }

        /// <summary>
        ///   Gets a typed buffer with the specified description, scoped for the duration of the <see cref="DrawEffect.DrawCore"/>.
        /// </summary>
        /// <param name="count">Number of elements in the buffer.</param>
        /// <param name="viewFormat">The element format seen in the shader.</param>
        /// <param name="isUnorderedAccess">A value indicating whether the buffer can be accessed in unordered mode.</param>
        /// <param name="usage">The usage flags for the buffer.</param>
        /// <returns>The new buffer.</returns>
        protected Buffer NewScopedTypedBuffer(int count, PixelFormat viewFormat, bool isUnorderedAccess, GraphicsResourceUsage usage = GraphicsResourceUsage.Default)
        {
            return NewScopedBuffer(
                new BufferDescription(
                    sizeInBytes: count * viewFormat.SizeInBytes(),
                    bufferFlags: BufferFlags.ShaderResource |
                                 (isUnorderedAccess ? BufferFlags.UnorderedAccess : BufferFlags.None),
                    usage),
                viewFormat);
        }

        /// <summary>
        ///   Pushes a new scoped resource to the current Draw.
        /// </summary>
        /// <param name="resource">The scoped resource</param>
        /// <returns>The scoped resource.</returns>
        protected T PushScopedResource<T>(T resource) where T : GraphicsResource
        {
            scopedResources.Add(resource);
            return resource;
        }

        /// <summary>
        ///   Checks that the current execution path is between a PreDraw / PostDraw sequence and throws and
        ///   exception if not.
        /// </summary>
        protected void CheckIsInDrawCore()
        {
            if (!isInDrawCore)
                throw new InvalidOperationException("The method execution path is not within a DrawCore operation.");
        }

        protected override void Destroy()
        {
            // If this instance is destroyed and not unloaded, force an unload before destryoing it completely
            if (Context != null)
            {
                Unload();
            }
            base.Destroy();
        }

        protected T ToLoadAndUnload<T>(T effect) where T : class, IGraphicsRendererCore
        {
            if (effect is null)
                throw new ArgumentNullException(nameof(effect));

            effect.Initialize(Context);
            subRenderersToUnload.Add(effect);
            return effect;
        }

        private void ReleaseAllScopedResources()
        {
            foreach (var scopedResource in scopedResources)
            {
                Context.Allocator.ReleaseReference(scopedResource);
            }
            scopedResources.Clear();
        }

        protected void PreDrawCoreInternal(RenderDrawContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            EnsureContext(context.RenderContext);

            if (ProfilingKey.Name != null && Profiling)
            {
                context.QueryManager.BeginProfile(Color.Green, ProfilingKey);
            }

            PreDrawCore(context);

            // Allow scoped allocation RenderTargets
            isInDrawCore = true;
        }

        protected void EnsureContext(RenderContext context)
        {
            if (Context is null)
            {
                Initialize(context);
            }
            else if (Context != context)
            {
                throw new InvalidOperationException("Cannot use a different context between Load and Draw.");
            }
        }

        protected void PostDrawCoreInternal(RenderDrawContext context)
        {
            isInDrawCore = false;

            // Release scoped RenderTargets
            ReleaseAllScopedResources();

            PostDrawCore(context);

            if (ProfilingKey.Name != null && Profiling)
            {
                context.QueryManager.EndProfile(ProfilingKey);
            }
        }
    }
}
