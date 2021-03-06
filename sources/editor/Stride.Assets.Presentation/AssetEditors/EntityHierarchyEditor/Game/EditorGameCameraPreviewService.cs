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
using Stride.Assets.SpriteFont;
using Stride.Assets.SpriteFont.Compiler;
using Stride.Graphics;
using Stride.Graphics.Font;
using Stride.Rendering;
using Stride.Rendering.Compositing;
using Stride.Engine;
using Stride.Assets.Presentation.AssetEditors.GameEditor.Game;
using Stride.Assets.Presentation.AssetEditors.GameEditor.Services;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Services;
using Stride.Editor.Build;
using Stride.Editor.EditorGame.Game;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Game
{
    public class EditorGameCameraPreviewService : EditorGameServiceBase, IEditorGameCameraPreviewService, IEditorGameCameraPreviewViewModelService
    {
        private readonly IEditorGameController controller;
        private SpriteBatch incrustBatch;
        private bool isActive;
        private Entity selectedEntity;
        private Graphics.SpriteFont defaultFont;

        private EntityHierarchyEditorGame game;

        private EffectInstance spriteEffect;
        private MutablePipelineState borderPipelineState;
        private Graphics.Buffer borderVertexBuffer;

        private GenerateIncrustRenderer generateIncrustRenderer;
        private RenderIncrustRenderer renderIncrustRenderer;

        public EditorGameCameraPreviewService(IEditorGameController controller)
        {
            this.controller = controller;
        }

        public override bool IsActive { get => isActive; set => isActive = value; }

        public override IEnumerable<Type> Dependencies
        {
            get
            {
                yield return typeof(IEditorGameEntitySelectionService);
                yield return typeof(IEditorGameRenderModeService);
            }
        }

        bool IEditorGameCameraPreviewViewModelService.IsActive
        {
            get => isActive;

            set
            {
                isActive = value;
                controller.InvokeAsync(() => IsActive = value);
            }
        }

        private IEditorGameRenderModeService Rendering => game.EditorServices.Get<IEditorGameRenderModeService>();

        protected override Task<bool> Initialize(EditorServiceGame editorGame)
        {
            game = (EntityHierarchyEditorGame) editorGame;

            // Create the default font
            var fontItem = OfflineRasterizedSpriteFontFactory.Create();
            fontItem.FontType.Size = 8;
            defaultFont = OfflineRasterizedFontCompiler.Compile(game.Services.GetService<IFontFactory>(), fontItem, game.GraphicsDevice.ColorSpace == ColorSpace.Linear);

            incrustBatch = new SpriteBatch(game.GraphicsDevice);

            // SpriteEffect will be used to draw camera incrust border
            spriteEffect = new EffectInstance(new Graphics.Effect(game.GraphicsDevice, SpriteEffect.Bytecode));
            spriteEffect.Parameters.Set(TexturingKeys.Texture0, game.GraphicsDevice.GetSharedWhiteTexture());
            spriteEffect.UpdateEffect(game.GraphicsDevice);

            borderPipelineState = new MutablePipelineState(game.GraphicsDevice);
            borderPipelineState.State.RootSignature = spriteEffect.RootSignature;
            borderPipelineState.State.EffectBytecode = spriteEffect.Effect.Bytecode;
            borderPipelineState.State.InputElements = VertexPositionTexture.Layout.CreateInputElements();
            borderPipelineState.State.PrimitiveType = PrimitiveType.LineStrip;
            borderPipelineState.State.RasterizerState = RasterizerStates.CullNone;

            unsafe
            {
                var borderVertices = new[]
                {
                    new VertexPositionTexture(new Vector3(0.0f, 0.0f, 0.0f), Vector2.Zero),
                    new VertexPositionTexture(new Vector3(0.0f, 1.0f, 0.0f), Vector2.Zero),
                    new VertexPositionTexture(new Vector3(1.0f, 1.0f, 0.0f), Vector2.Zero),
                    new VertexPositionTexture(new Vector3(1.0f, 0.0f, 0.0f), Vector2.Zero),
                    new VertexPositionTexture(new Vector3(0.0f, 0.0f, 0.0f), Vector2.Zero),

                    // Extra vertex so that left-top corner is not missing (likely due to rasterization rule)
                    new VertexPositionTexture(new Vector3(0.0f, 1.0f, 0.0f), Vector2.Zero)
                };
                fixed (VertexPositionTexture* borderVerticesPtr = borderVertices)
                    borderVertexBuffer = Graphics.Buffer.Vertex.New(game.GraphicsDevice, new DataPointer(borderVerticesPtr, VertexPositionTexture.Size * borderVertices.Length));
            }

            if (game.EditorSceneSystem.GraphicsCompositor.Game is EditorTopLevelCompositor editorTopLevelCompositor)
            {
                // Display it as incrust
                editorTopLevelCompositor.PostGizmoCompositors.Add(renderIncrustRenderer = new RenderIncrustRenderer(this));
            }

            Services.Get<IEditorGameEntitySelectionService>().SelectionUpdated += UpdateModifiedEntitiesList;

            return Task.FromResult(true);
        }

        public override void UpdateGraphicsCompositor([NotNull] EditorServiceGame game)
        {
            base.UpdateGraphicsCompositor(game);

            renderIncrustRenderer.IncrustRenderer = null;

            if (game.SceneSystem.GraphicsCompositor.Game is EditorTopLevelCompositor editorTopLevelCompositor)
            {
                generateIncrustRenderer = new GenerateIncrustRenderer(this)
                {
                    Content = editorTopLevelCompositor.Child,
                    GameSettingsAccessor = game.PackageSettings,
                };

                // Render camera view
                editorTopLevelCompositor.PostGizmoCompositors.Add(generateIncrustRenderer);

                renderIncrustRenderer.IncrustRenderer = generateIncrustRenderer;
            }
        }

        public override ValueTask DisposeAsync()
        {
            EnsureNotDestroyed(nameof(EditorGameCameraPreviewService));

            // Unregister events
            var selectionService = Services.Get<IEditorGameEntitySelectionService>();
            if (selectionService is not null)
                selectionService.SelectionUpdated -= UpdateModifiedEntitiesList;

            // Remove renderers
            if (game.SceneSystem.GraphicsCompositor.Game is EditorTopLevelCompositor gameTopLevelCompositor &&
                game.EditorSceneSystem.GraphicsCompositor.Game is EditorTopLevelCompositor editorTopLevelCompositor)
            {
                gameTopLevelCompositor.PostGizmoCompositors.Remove(generateIncrustRenderer);
                editorTopLevelCompositor.PostGizmoCompositors.Remove(renderIncrustRenderer);
            }

            defaultFont?.Dispose();
            defaultFont = null;

            spriteEffect?.Dispose();
            spriteEffect = null;

            borderPipelineState = null;
            borderVertexBuffer?.Dispose();
            borderVertexBuffer = null;

            return base.DisposeAsync();
        }

        private void UpdateModifiedEntitiesList(object sender, EntitySelectionEventArgs e)
        {
            selectedEntity = e.NewSelection.Count == 1 ? e.NewSelection.Single() : null;
        }

        private class RenderIncrustRenderer : SceneRendererBase
        {
            private readonly EditorGameCameraPreviewService previewService;

            public GenerateIncrustRenderer IncrustRenderer { get; set; }

            public RenderIncrustRenderer(EditorGameCameraPreviewService previewService)
            {
                this.previewService = previewService;
            }

            protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
            {
                if (IncrustRenderer is null || !IncrustRenderer.IsIncrustEnabled)
                    return;

                var currentViewport = drawContext.CommandList.Viewport;

                // Copy incrust back to render target
                var position = new Vector2(currentViewport.Width - IncrustRenderer.Viewport.Width - 16,
                                           currentViewport.Height - IncrustRenderer.Viewport.Height - 16);

                // Camera incrust border
                previewService.borderPipelineState.State.Output.CaptureState(drawContext.CommandList);
                previewService.borderPipelineState.Update();
                drawContext.CommandList.SetPipelineState(previewService.borderPipelineState.CurrentState);
                previewService.spriteEffect.Parameters.Set(SpriteBaseKeys.MatrixTransform, Matrix.Scaling((float) (IncrustRenderer.Viewport.Width + 1) * 2.0f / (float)currentViewport.Width, -(float)(IncrustRenderer.Viewport.Height + 1) * 2.0f / (float)currentViewport.Height, 1.0f) * Matrix.Translation(position.X * 2.0f / currentViewport.Width - 1.0f, -(position.Y * 2.0f / currentViewport.Height - 1.0f), 0.0f));
                previewService.spriteEffect.Apply(drawContext.GraphicsContext);

                drawContext.CommandList.SetVertexBuffer(0, previewService.borderVertexBuffer, 0, VertexPositionTexture.Size);
                drawContext.CommandList.Draw(previewService.borderVertexBuffer.SizeInBytes / VertexPositionTexture.Size);

                // Camera incrust render
                previewService.incrustBatch.Begin(drawContext.GraphicsContext, blendState: BlendStates.Default, depthStencilState: DepthStencilStates.None);
                previewService.incrustBatch.Draw(IncrustRenderer.GeneratedIncrust, position);
                previewService.incrustBatch.End();

                // Camera name
                if (IncrustRenderer.Camera?.Entity?.Name is not null)
                {
                    previewService.incrustBatch.Begin(drawContext.GraphicsContext, blendState: BlendStates.AlphaBlend, depthStencilState: DepthStencilStates.None);
                    previewService.incrustBatch.DrawString(previewService.defaultFont, IncrustRenderer.Camera?.Entity.Name, new Vector2(position.X, position.Y - 16), Color.White);
                    previewService.incrustBatch.End();
                }

                drawContext.GraphicsContext.Allocator.ReleaseReference(IncrustRenderer.GeneratedIncrust);
            }
        }

        private class GenerateIncrustRenderer : SceneRendererBase
        {
            private readonly EditorGameCameraPreviewService previewService;
            internal bool IsIncrustEnabled;
            internal CameraComponent Camera;

            public GenerateIncrustRenderer(EditorGameCameraPreviewService previewService)
            {
                this.previewService = previewService;
            }

            /// <summary>
            ///   The inner compositor to draw inside the viewport.
            /// </summary>
            public new ISceneRenderer Content { get; set; }

            public IGameSettingsAccessor GameSettingsAccessor { get; set; }

            /// <summary>
            ///   The render view created and used by this compositor.
            /// </summary>
            public RenderView RenderView { get; } = new RenderView();

            public Viewport Viewport;

            public Texture GeneratedIncrust { get; private set; }

            protected override void CollectCore(RenderContext context)
            {
                base.CollectCore(context);

                Camera = previewService.selectedEntity?.Components.Get<CameraComponent>();

                // Enable camera incrust only if we have an active camera and IsActive is true
                // Also disabled when previewing game graphics compositor
                IsIncrustEnabled = previewService.isActive && !previewService.Rendering.RenderMode.PreviewGameGraphicsCompositor && Camera is not null;

                if (IsIncrustEnabled)
                {
                    var width = context.ViewportState.Viewport0.Width * 0.3f;
                    var height = context.ViewportState.Viewport0.Height * 0.2f;
                    var aspectRatio = width / height;

                    if (Camera.UseCustomAspectRatio)
                    {
                        // Make sure to respect aspect ratio
                        aspectRatio = Camera.AspectRatio;
                    }
                    else if (GameSettingsAccessor is not null)
                    {
                        var renderingSettings = GameSettingsAccessor.GetConfiguration<RenderingSettings>();
                        if (renderingSettings is not null)
                        {
                            aspectRatio = (float) renderingSettings.DefaultBackBufferWidth / renderingSettings.DefaultBackBufferHeight;
                        }
                    }

                    if (width > height * aspectRatio)
                    {
                        width = height * aspectRatio;
                    }
                    else
                    {
                        height = width / aspectRatio;
                    }

                    if (width < 32.0f || height < 32.0f)
                    {
                        // Do not display incrust if too small
                        IsIncrustEnabled = false;
                    }

                    context.RenderSystem.Views.Add(RenderView);
                    context.RenderView = RenderView;

                    // Setup viewport
                    Viewport = new Viewport(0, 0, (int)width, (int)height);

                    using (context.SaveViewportAndRestore())
                    {
                        context.ViewportState = new ViewportState { Viewport0 = Viewport };

                        SceneCameraRenderer.UpdateCameraToRenderView(context, RenderView, Camera);

                        using (context.PushRenderViewAndRestore(RenderView))
                        using (context.PushTagAndRestore(CameraComponentRendererExtensions.Current, Camera))
                        {
                            Content.Collect(context);
                        }
                    }
                }
            }

            protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
            {
                if (!IsIncrustEnabled)
                    return;

                using (context.PushRenderViewAndRestore(RenderView))
                using (drawContext.PushTagAndRestore(CameraComponentRendererExtensions.Current, Camera))
                {
                    var oldViewport = drawContext.CommandList.Viewport;

                    // Allocate a RT for the incrust
                    GeneratedIncrust = drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(TextureDescription.New2D((int)Viewport.Width, (int)Viewport.Height, 1, drawContext.CommandList.RenderTarget.Format, TextureFlags.ShaderResource | TextureFlags.RenderTarget));
                    var depthBuffer = drawContext.CommandList.DepthStencilBuffer is not null ? drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(TextureDescription.New2D((int)Viewport.Width, (int)Viewport.Height, 1, drawContext.CommandList.DepthStencilBuffer.Format, TextureFlags.DepthStencil)) : null;

                    // Push and set render target
                    using (drawContext.PushRenderTargetsAndRestore())
                    {
                        drawContext.CommandList.SetRenderTarget(depthBuffer, GeneratedIncrust);

                        drawContext.CommandList.SetViewport(Viewport);
                        Content.Draw(drawContext);

                        drawContext.CommandList.SetViewport(oldViewport);
                    }

                    // Note: GeneratedIncrust is released by RenderIncrustCompositorPart
                    if (depthBuffer is not null)
                        drawContext.GraphicsContext.Allocator.ReleaseReference(depthBuffer);
                }
            }
        }
    }
}
