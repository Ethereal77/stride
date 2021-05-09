// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;

namespace Stride.Rendering
{
    /// <summary>
    ///   Defines an entry-point for implementing rendering features.
    /// </summary>
    [DataContract(Inherited = true, DefaultMemberMode = DataMemberMode.Never)]
    public abstract class RenderFeature : ComponentBase, IGraphicsRendererCore
    {
        private RenderSystem renderSystem;

        /// <summary>
        ///   Gets a context object providing access to information and services for rendering.
        /// </summary>
        protected RenderContext Context { get; private set; }

        /// <summary>
        ///   Gets the render system to which this render feature is associated.
        /// </summary>
        public RenderSystem RenderSystem
        {
            get => renderSystem;

            internal set
            {
                renderSystem = value;
                OnRenderSystemChanged();
            }
        }

        /// <summary>
        ///   Gets a value indicating if this render feature has been initialized.
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether this render feature is currently enabled.
        /// </summary>
        public bool Enabled
        {
            get => true;
            set => throw new NotSupportedException();
        }


        /// <summary>
        ///   Initializes the current render feature, where it can query for specific buffers or resources and
        ///   prepare its internal state.
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(RenderContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (Context != null)
                Unload();

            Context = context;

            InitializeCore();

            Initialized = true;

            // Notify that a particular renderer has been initialized
            context.OnRendererInitialized(this);
        }

        /// <summary>
        ///   Unloads the current render feature.
        /// </summary>
        public virtual void Unload()
        {
            Context = null;
        }

        /// <summary>
        ///   Disposes the current render feature and its associated state.
        /// </summary>
        protected override void Destroy()
        {
            // If this instance is destroyed and not unload, force an unload before destryoing it completely
            if (Context != null)
                Unload();

            base.Destroy();
        }

        /// <summary>
        ///   Initializes this instance. Override in a derived class to query for specific constant buffers (either new ones, like
        ///   <c>PerMaterial</c>, or parts of existing ones, like <c>PerObject=>Skinning</c>).
        /// </summary>
        protected virtual void InitializeCore() { }

        /// <summary>
        ///   Performs pipeline initialization, enumerates views and populates visibility groups.
        ///   Override in a derived class to implement custom logic for a specific rendering technique.
        /// </summary>
        public virtual void Collect() { }

        /// <summary>
        ///   Extracts data from entities.
        ///   Override in a derived class to implement custom logic for a specific rendering technique.
        /// </summary>
        /// <remarks>
        ///   This method should be as fast as possible to not block the simulation loop. It should be mostly copies, and the
        ///   actual processing should be part of <see cref="Prepare"/>.
        /// </remarks>
        public virtual void Extract() { }

        /// <summary>
        ///   Prepares the effects and their configurations before <see cref="Prepare"/>.
        ///   Override in a derived class to implement custom logic for a specific rendering technique.
        /// </summary>
        /// <param name="context">Context object providing access to services and information about the renderer.</param>
        public virtual void PrepareEffectPermutations(RenderDrawContext context) { }

        /// <summary>
        ///   Performs most of the work (computation and resource preparation). Later game simulation might be running during this step.
        ///   Override in a derived class to implement custom logic for a specific rendering technique.
        /// </summary>
        /// <param name="context">Context object providing access to services and information about the renderer.</param>
        public virtual void Prepare(RenderDrawContext context) { }

        /// <summary>
        ///   Issues the GPU updates and/or drawing commands for a specific render view and stage.
        ///   Override in a derived class to implement custom logic for a specific rendering technique.
        /// </summary>
        /// <param name="context">Context object providing access to services and information about the renderer.</param>
        /// <param name="renderView">The current render view to draw.</param>
        /// <param name="renderViewStage">The current stage to draw.</param>
        public virtual void Draw(RenderDrawContext context, RenderView renderView, RenderViewStage renderViewStage)
        {
        }

        /// <summary>
        ///   Issues the GPU updates and/or drawing commands for a specific render view and stage.
        ///   Override in a derived class to implement custom logic for a specific rendering technique.
        /// </summary>
        /// <param name="context">Context object providing access to services and information about the renderer.</param>
        /// <param name="renderView">The current render view to draw.</param>
        /// <param name="renderViewStage">The current stage to draw.</param>
        public virtual void Draw(RenderDrawContext context, RenderView renderView, RenderViewStage renderViewStage, int startIndex, int endIndex) { }

        /// <summary>
        ///   Releases temporary resources and cleans the state. Should be called once after all <see cref="Draw"/> calls have finished.
        ///   Override in a derived class to implement custom logic for a specific rendering technique.
        /// </summary>
        /// <param name="context">Context object providing access to services and information about the renderer.</param>
        public virtual void Flush(RenderDrawContext context) { }

        /// <summary>
        ///   Method called when the render system associated with this render feature has changed.
        ///   Override in a derived class to implement custom logic for a specific rendering technique.
        /// </summary>
        protected virtual void OnRenderSystemChanged() { }
    }
}
