// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Collections;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;
using Stride.Engine;
using Stride.Graphics;

namespace Stride.Rendering.Compositing
{
    /// <summary>
    ///   Represents a class that manages the rendering pipeline and the composition of the different <see cref="RenderFeature"/>s
    ///   and <see cref="RenderStage"/>s to render the <see cref="Scene"/>s through <see cref="RenderView"/>s and finally apply
    ///   post-processing if appropriate.
    /// </summary>
    [DataSerializerGlobal(typeof(ReferenceSerializer<GraphicsCompositor>), Profile = "Content")]
    [ReferenceSerializer, ContentSerializer(typeof(DataContentSerializerWithReuse<GraphicsCompositor>))]
    [DataContract]
    // Needed for indirect serialization of RenderSystem.RenderStages and RenderSystem.RenderFeatures
    // TODO: We would like an attribute to specify that serializing through the interface type is fine in this case (bypass type detection)
    [DataSerializerGlobal(null, typeof(FastTrackingCollection<RenderStage>))]
    [DataSerializerGlobal(null, typeof(FastTrackingCollection<RootRenderFeature>))]
    public class GraphicsCompositor : RendererBase
    {
        /// <summary>
        ///   A property key to get the current <see cref="GraphicsCompositor"/> from the <see cref="RenderContext.Tags"/>.
        /// </summary>
        public static readonly PropertyKey<GraphicsCompositor> Current = new PropertyKey<GraphicsCompositor>("GraphicsCompositor.Current", typeof(GraphicsCompositor));

        private readonly List<SceneInstance> initializedSceneInstances = new List<SceneInstance>();

        /// <summary>
        ///   Gets the <see cref="Rendering.RenderSystem"/> used with this graphics compositor.
        /// </summary>
        [DataMemberIgnore]
        public RenderSystem RenderSystem { get; } = new RenderSystem();

        /// <summary>
        ///   Gets the cameras used by this graphics compositor.
        /// </summary>
        /// <value>The collection of cameras in use by this graphics compositor.</value>
        /// <userdoc>The list of cameras used in the graphic pipeline.</userdoc>
        [DataMember(10)]
        [Category]
        [MemberCollection(NotNullItems = true)]
        public SceneCameraSlotCollection Cameras { get; } = new SceneCameraSlotCollection();

        /// <summary>
        ///   Gets the list of render stages registered in this graphics compositor.
        /// </summary>
        /// <value>A list of <see cref="RenderStage"/>s registered with this graphics compositor.</value>
        [DataMember(20)]
        [Category]
        [MemberCollection(NotNullItems = true)]
        public IList<RenderStage> RenderStages => RenderSystem.RenderStages;

        /// <summary>
        ///   Gets the list of render features registered in this graphics compositor.
        /// </summary>
        /// <value>A list of the available <see cref="RenderFeature"/>s registered with this graphics compositor.</value>
        [DataMember(30)]
        [Category]
        [MemberCollection(NotNullItems = true)]
        public IList<RootRenderFeature> RenderFeatures => RenderSystem.RenderFeatures;

        /// <summary>
        ///   Gets or sets the <see cref="ISceneRenderer"/> to use as entry point for the game compositor.
        /// </summary>
        public ISceneRenderer Game { get; set; }
        /// <summary>
        ///   Gets or sets the <see cref="ISceneRenderer"/> to use as entry point for a compositor that can render a single view,
        ///   like cubemaps, render textures, etc.
        /// </summary>
        public ISceneRenderer SingleView { get; set; }
        /// <summary>
        ///   Gets or sets the <see cref="ISceneRenderer"/> to use as entry point for a compositor used by the scene editor.
        /// </summary>
        public ISceneRenderer Editor { get; set; }

        /// <inheritdoc/>
        protected override void InitializeCore()
        {
            base.InitializeCore();

            RenderSystem.Initialize(Context);
        }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            // Dispose renderers
            Game?.Dispose();

            // Cleanup created visibility groups
            foreach (var sceneInstance in initializedSceneInstances)
            {
                for (var i = 0; i < sceneInstance.VisibilityGroups.Count; i++)
                {
                    var visibilityGroup = sceneInstance.VisibilityGroups[i];
                    if (visibilityGroup.RenderSystem == RenderSystem)
                    {
                        sceneInstance.VisibilityGroups.RemoveAt(i);
                        break;
                    }
                }
            }

            RenderSystem.Dispose();

            base.Destroy();
        }

        /// <inheritdoc/>
        protected override void DrawCore(RenderDrawContext context)
        {
            if (Game != null)
            {
                // Get or create VisibilityGroup for this RenderSystem + SceneInstance
                var sceneInstance = SceneInstance.GetCurrent(context.RenderContext);
                VisibilityGroup visibilityGroup = null;
                if (sceneInstance != null)
                {
                    // Find if VisibilityGroup
                    foreach (var currentVisibilityGroup in sceneInstance.VisibilityGroups)
                    {
                        if (currentVisibilityGroup.RenderSystem == RenderSystem)
                        {
                            visibilityGroup = currentVisibilityGroup;
                            break;
                        }
                    }

                    // If first time, let's create and register it
                    if (visibilityGroup == null)
                    {
                        sceneInstance.VisibilityGroups.Add(visibilityGroup = new VisibilityGroup(RenderSystem));
                        initializedSceneInstances.Add(sceneInstance);
                    }

                    // Reset & cleanup
                    visibilityGroup.Reset();
                }

                using (context.RenderContext.PushTagAndRestore(SceneInstance.CurrentVisibilityGroup, visibilityGroup))
                using (context.RenderContext.PushTagAndRestore(SceneInstance.CurrentRenderSystem, RenderSystem))
                using (context.RenderContext.PushTagAndRestore(SceneCameraSlotCollection.Current, Cameras))
                using (context.RenderContext.PushTagAndRestore(Current, this))
                {
                    // Set render system
                    context.RenderContext.RenderSystem = RenderSystem;
                    context.RenderContext.VisibilityGroup = visibilityGroup;

                    // Set start states for viewports and output (it will be used during the Collect phase)
                    var renderOutputs = new RenderOutputDescription();
                    renderOutputs.CaptureState(context.CommandList);
                    context.RenderContext.RenderOutput = renderOutputs;

                    var viewports = new ViewportState();
                    viewports.CaptureState(context.CommandList);
                    context.RenderContext.ViewportState = viewports;

                    try
                    {
                        // Collect in the game graphics compositor: Setup features/stages, enumerate views and populates VisibilityGroup
                        Game.Collect(context.RenderContext);

                        // Collect in render features
                        RenderSystem.Collect(context.RenderContext);

                        // Collect visibile objects from each view (that were not properly collected previously)
                        if (visibilityGroup != null)
                        {
                            foreach (var view in RenderSystem.Views)
                                visibilityGroup.TryCollect(view);
                        }

                        // Extract
                        RenderSystem.Extract(context.RenderContext);

                        // Prepare
                        RenderSystem.Prepare(context);

                        // Draw using the game graphics compositor
                        Game.Draw(context);

                        // Flush
                        RenderSystem.Flush(context);
                    }
                    finally
                    {
                        // Reset render context data
                        RenderSystem.Reset();
                    }
                }
            }
        }
    }
}
