// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Core.BuildEngine;
using Stride.Engine;
using Stride.Rendering;
using Stride.Rendering.Compositing;
using Stride.Assets.Presentation.AssetEditors.GameEditor;
using Stride.Assets.Presentation.AssetEditors.GameEditor.Game;
using Stride.Assets.Presentation.AssetEditors.GameEditor.Services;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Services;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels;
using Stride.Assets.Presentation.AssetEditors.Gizmos;
using Stride.Assets.Presentation.SceneEditor;
using Stride.Editor.EditorGame.Game;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Game
{
    public class EditorGameEntityTransformService : EditorGameMouseServiceBase, IEditorGameEntityTransformViewModelService
    {
        private readonly List<TransformationGizmo> transformationGizmos = new();

        // TODO: Referencing EntityTransformationViewModel should be enough
        private readonly EntityHierarchyEditorViewModel editor;
        private readonly IEditorGameController controller;

        private Scene editorScene;
        private TransformationGizmo activeTransformationGizmo;
        private Entity entityWithGizmo;
        private EntityHierarchyEditorGame game;
        private Transformation activeTransformation;
        private TransformationSpace space;
        private double gizmoSize = 1.0f;


        public EditorGameEntityTransformService([NotNull] EntityHierarchyEditorViewModel editor, [NotNull] IEditorGameController controller)
        {
            this.editor = editor ?? throw new ArgumentNullException(nameof(editor));
            this.controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }


        /// <inheritdoc/>
        public override bool IsControllingMouse { get; protected set; }

        /// <summary>
        ///   Gets the translation gizmo.
        /// </summary>
        public TranslationGizmo TranslationGizmo { get; private set; }

        /// <summary>
        ///   Gets the rotation gizmo.
        /// </summary>
        public RotationGizmo RotationGizmo { get; private set; }

        /// <summary>
        ///   Gets the scaling gizmo.
        /// </summary>
        public ScaleGizmo ScaleGizmo { get; private set; }

        /// <summary>
        ///   Gets the currently active gizmo.
        /// </summary>
        public TransformationGizmo ActiveTransformationGizmo
        {
            get => activeTransformationGizmo;

            private set
            {
                if (activeTransformationGizmo is not null)
                    activeTransformationGizmo.IsEnabled = false;

                activeTransformationGizmo = value;

                if (activeTransformationGizmo is not null && EntityWithGizmo is not null)
                {
                    activeTransformationGizmo.IsEnabled = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the <see cref="Entity"/> which has the gizmo attached.
        /// </summary>
        public Entity EntityWithGizmo
        {
            get => entityWithGizmo;

            set
            {
                entityWithGizmo = value;
                foreach (var gizmo in transformationGizmos)
                    gizmo.AnchorEntity = entityWithGizmo;

                ActiveTransformationGizmo = activeTransformationGizmo;
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<Type> Dependencies {  get { yield return typeof(IEditorGameEntitySelectionService); } }

        /// <inheritdoc/>
        Transformation IEditorGameTransformViewModelService.ActiveTransformation
        {
            get => activeTransformation;

            set
            {
                activeTransformation = value;
                TransformationGizmo nextGizmo = value switch
                {
                    Transformation.Translation => TranslationGizmo,
                    Transformation.Rotation => RotationGizmo,
                    Transformation.Scale => ScaleGizmo,

                    _ => throw new ArgumentOutOfRangeException(nameof(value))
                };
                controller.InvokeAsync(() => ActiveTransformationGizmo = nextGizmo);
            }
        }

        /// <inheritdoc/>
        TransformationSpace IEditorGameTransformViewModelService.TransformationSpace
        {
            get => space;

            set
            {
                space = value;
                controller.InvokeAsync(() => transformationGizmos.ForEach(gizmo => gizmo.Space = value));
            }
        }

        /// <inheritdoc/>
        double IEditorGameEntityTransformViewModelService.GizmoSize
        {
            get { return gizmoSize; }

            set
            {
                gizmoSize = value;
                controller.InvokeAsync(() => transformationGizmos.ForEach(gizmo => gizmo.SizeFactor = SmoothGizmoSize((float)value)));

                // Smoothes the changes in gizmo size so they don't grow linearly
                static float SmoothGizmoSize(float value)
                {
                    return value > 1 ? 1 + (value - 1) * 0.1f : (float) Math.Pow(value, 0.333);
                }
            }
        }

        /// <inheritdoc/>
        internal IEditorGameEntitySelectionService Selection => Services.Get<IEditorGameEntitySelectionService>();


        /// <inheritdoc />
        public override ValueTask DisposeAsync()
        {
            EnsureNotDestroyed(nameof(EditorGameEntityTransformService));

            var selectionService = Services.Get<IEditorGameEntitySelectionService>();
            if (selectionService is not null)
                selectionService.SelectionUpdated -= UpdateModifiedEntitiesList;

            return base.DisposeAsync();
        }

        protected override Task<bool> Initialize(EditorServiceGame editorGame)
        {
            game = (EntityHierarchyEditorGame) editorGame;
            editorScene = game.EditorScene;

            var transformMainGizmoRenderStage = new RenderStage("TransformGizmoOpaque", "Main");
            var transformTransparentGizmoRenderStage = new RenderStage("TransformGizmoTransparent", "Main") { SortMode = new BackToFrontSortMode() };
            game.EditorSceneSystem.GraphicsCompositor.RenderStages.Add(transformMainGizmoRenderStage);
            game.EditorSceneSystem.GraphicsCompositor.RenderStages.Add(transformTransparentGizmoRenderStage);

            var meshRenderFeature = game.EditorSceneSystem.GraphicsCompositor.RenderFeatures.OfType<MeshRenderFeature>().First();

            // Reset all stages for TransformationGrizmoGroup
            meshRenderFeature.RenderStageSelectors.Add(new SimpleGroupToRenderStageSelector
            {
                RenderGroup = TransformationGizmo.TransformationGizmoGroupMask
            });
            meshRenderFeature.RenderStageSelectors.Add(new MeshTransparentRenderStageSelector
            {
                EffectName = EditorGraphicsCompositorHelper.EditorForwardShadingEffect,
                RenderGroup = TransformationGizmo.TransformationGizmoGroupMask,
                OpaqueRenderStage = transformMainGizmoRenderStage,
                TransparentRenderStage = transformTransparentGizmoRenderStage
            });
            meshRenderFeature.PipelineProcessors.Add(new MeshPipelineProcessor { TransparentRenderStage = transformTransparentGizmoRenderStage });

            var editorCompositor = (EditorTopLevelCompositor) game.EditorSceneSystem.GraphicsCompositor.Game;
            editorCompositor.PostGizmoCompositors.Add(new ClearRenderer { ClearFlags = ClearRendererFlags.DepthOnly });
            editorCompositor.PostGizmoCompositors.Add(new SingleStageRenderer { RenderStage = transformMainGizmoRenderStage, Name = "Transform Opaque Gizmos" });
            editorCompositor.PostGizmoCompositors.Add(new SingleStageRenderer { RenderStage = transformTransparentGizmoRenderStage, Name = "Transform Transparent Gizmos" });

            TranslationGizmo = new TranslationGizmo();
            RotationGizmo = new RotationGizmo();
            ScaleGizmo = new ScaleGizmo();
            TranslationGizmo.TransformationEnded += OnGizmoTransformationFinished;
            ScaleGizmo.TransformationEnded += OnGizmoTransformationFinished;
            RotationGizmo.TransformationEnded += OnGizmoTransformationFinished;

            transformationGizmos.Add(TranslationGizmo);
            transformationGizmos.Add(RotationGizmo);
            transformationGizmos.Add(ScaleGizmo);

            Services.Get<IEditorGameEntitySelectionService>().SelectionUpdated += UpdateModifiedEntitiesList;

            // Initialize and add the Gizmo entities to the gizmo scene
            MicrothreadLocalDatabases.MountCommonDatabase();

            // Initialize the gizmo
            foreach (var gizmo in transformationGizmos)
                gizmo.Initialize(game.Services, editorScene);

            // Deactivate all transformation gizmo by default
            foreach (var gizmo in transformationGizmos)
                gizmo.IsEnabled = false;

            // Set the default active transformation gizmo
            ActiveTransformationGizmo = TranslationGizmo;

            // Start update script (with priority 1 so that it happens after UpdateModifiedEntitiesList
            // is called -- which usually happens from a EditorGameComtroller.PostAction() which has a default priority 0)
            game.Script.AddTask(Update, 1);
            return Task.FromResult(true);
        }

        //
        // Updates the transformation gizmos.
        //
        private async Task Update()
        {
            while (!IsDisposed)
            {
                if (IsActive)
                {
                    if (IsMouseAvailable)
                    {
                        if (game.Input.IsKeyPressed(SceneEditorSettings.SnapSelectionToGrid.GetValue()))
                        {
                            SnapSelectionToGrid();
                        }
                        if (game.Input.IsKeyPressed(SceneEditorSettings.TranslationGizmo.GetValue()))
                        {
                            await editor.Dispatcher.InvokeAsync(() => editor.Transform.ActiveTransformation = Transformation.Translation);
                        }
                        if (game.Input.IsKeyPressed(SceneEditorSettings.RotationGizmo.GetValue()))
                        {
                            await editor.Dispatcher.InvokeAsync(() => editor.Transform.ActiveTransformation = Transformation.Rotation);
                        }
                        if (game.Input.IsKeyPressed(SceneEditorSettings.ScaleGizmo.GetValue()))
                        {
                            await editor.Dispatcher.InvokeAsync(() => editor.Transform.ActiveTransformation = Transformation.Scale);
                        }
                        if (game.Input.IsKeyPressed(SceneEditorSettings.SwitchGizmo.GetValue()))
                        {
                            var current = activeTransformation;
                            var next = (int)(current + 1) % Enum.GetValues(typeof(Transformation)).Length;
                            await editor.Dispatcher.InvokeAsync(() => editor.Transform.ActiveTransformation = (Transformation) next);
                        }
                    }

                    IEnumerable<Task> tasks;
                    lock (transformationGizmos)
                    {
                        tasks = transformationGizmos.Select(x => x.Update());
                    }

                    IsControllingMouse = activeTransformationGizmo is not null && activeTransformationGizmo.IsUnderMouse() && IsMouseAvailable;

                    await Task.WhenAll(tasks);
                }

                await game.Script.NextFrame();
            }
        }

        //
        // Forces the selected entities to move to the nearest snap position.
        //
        private void SnapSelectionToGrid()
        {
            if (Selection.SelectedRootIdCount == 0)
                return;

            var transformations = new Dictionary<AbsoluteId, TransformationTRS>();
            foreach (var item in Selection.GetSelectedRootIds())
            {
                var entity = (Entity) controller.FindGameSidePart(item);
                entity.Transform.Position = MathUtil.Snap(entity.Transform.Position, TranslationGizmo.SnapValue);
                transformations.Add(item, new TransformationTRS(entity.Transform));
            }

            InvokeTransformationFinished(transformations);
        }

        //
        // Updates the gizmos when the selected entities have changed.
        //
        private void UpdateModifiedEntitiesList(object sender, [NotNull] EntitySelectionEventArgs e)
        {
            EntityWithGizmo = e.NewSelection.LastOrDefault();

            if (ActiveTransformationGizmo is not null && EntityWithGizmo is null)
            {
                // Reset the transformation axes if the selection is cleared
                ActiveTransformationGizmo.ClearTransformationAxes();
            }
            var modifiedEntities = new List<Entity>();
            modifiedEntities.AddRange(e.NewSelection);

            // Update the selected entities collections on transformation gizmo
            foreach (var gizmo in transformationGizmos)
            {
                gizmo.AnchorEntity = EntityWithGizmo;
                gizmo.ModifiedEntities = modifiedEntities;
            }
        }

        //
        // Applies the final transformations on the selected entities.
        //
        private void OnGizmoTransformationFinished(object sender, EventArgs e)
        {
            var transformations = new Dictionary<AbsoluteId, TransformationTRS>();

            foreach (var item in Selection.GetSelectedRootIds())
            {
                var entity = (Entity) controller.FindGameSidePart(item);
                transformations.Add(item, new TransformationTRS(entity.Transform));
            }

            InvokeTransformationFinished(transformations);
        }

        //
        // Applies the final transformations the gizmos have made to the scene.
        //
        private void InvokeTransformationFinished(IReadOnlyDictionary<AbsoluteId, TransformationTRS> transformation)
        {
            editor.Dispatcher.InvokeAsync(() => editor.UpdateTransformations(transformation));
        }

        //
        // Updates the snapping options for the current transformation gizmo.
        //
        void IEditorGameTransformViewModelService.UpdateSnap(Transformation transformation, float value, bool isActive)
        {
            controller.InvokeAsync(() =>
            {
                switch (transformation)
                {
                    case Transformation.Translation:
                        TranslationGizmo.SnapValue = value;
                        TranslationGizmo.UseSnap = isActive;
                        break;

                    case Transformation.Rotation:
                        RotationGizmo.SnapValue = value;
                        RotationGizmo.UseSnap = isActive;
                        break;

                    case Transformation.Scale:
                        ScaleGizmo.SnapValue = value;
                        ScaleGizmo.UseSnap = isActive;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(transformation));
                }
            });
        }
    }
}
