// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering.Compositing;

namespace Stride.Rendering.Skyboxes
{
    public class CubemapSceneRenderer : CubemapRendererBase
    {
        private readonly ISceneRendererContext context;
        private readonly ISceneRenderer gameCompositor;

        public CubemapSceneRenderer(ISceneRendererContext context, int textureSize)
            : base(context.GraphicsDevice, textureSize, PixelFormat.R16G16B16A16_Float, true)
        {
            this.context = context;

            var renderContext = RenderContext.GetShared(context.Services);
            DrawContext = new RenderDrawContext(context.Services, renderContext, context.GraphicsContext);

            // Replace graphics compositor (don't want post fx, etc...)
            gameCompositor = context.SceneSystem.GraphicsCompositor.Game;
            context.SceneSystem.GraphicsCompositor.Game = new SceneExternalCameraRenderer { Child = context.SceneSystem.GraphicsCompositor.SingleView, ExternalCamera = Camera };
        }

        public override void Dispose()
        {
            base.Dispose();

            context.SceneSystem.GraphicsCompositor.Game = gameCompositor;
        }

        public static Texture GenerateCubemap(ISceneRendererContext context, Vector3 position, int textureSize)
        {
            return GenerateCubemap(new CubemapSceneRenderer(context, textureSize), position);
        }

        protected override void DrawImpl()
        {
            context.GameSystems.Draw(context.DrawTime);
        }
    }
}
