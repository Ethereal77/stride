// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Extensions;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Input;
using Stride.Rendering;
using Stride.Rendering.Compositing;

namespace Stride.Assets.Presentation.SceneEditor
{
    internal struct PickingObjectInfo
    {
        private const float MaxMaterialIndex = 1024;
        private const float MaxInstancingId = 1024;

        public float ModelComponentId;
        public float MeshMaterialIndex;

        public PickingObjectInfo(int componentId, int meshIndex, int materialIndex)
        {
            ModelComponentId = componentId;
            MeshMaterialIndex = meshIndex + (Math.Min(materialIndex, MaxMaterialIndex - 1) / MaxMaterialIndex); // Pack to: MeshIndex.MaterialIndex
        }

        public EntityPickingResult GetResult(Dictionary<int, Entity> idToEntity)
        {
            var mcFraction = ModelComponentId - Math.Floor(ModelComponentId);
            var mcIntegral = ModelComponentId - mcFraction;

            var mmiFraction = MeshMaterialIndex - Math.Floor(MeshMaterialIndex);
            var mmiIntegral = MeshMaterialIndex - mmiFraction;

            var result = new EntityPickingResult
            {
                ComponentId = (int)Math.Round(mcIntegral),
                InstanceId = (int)Math.Round(mcFraction * MaxInstancingId),
                MeshNodeIndex = (int)Math.Round(mmiIntegral),
                MaterialIndex = (int)Math.Round(mmiFraction * MaxMaterialIndex),
            };
            idToEntity.TryGetValue(result.ComponentId, out result.Entity);
            return result;
        }
    }

    public sealed class PickingSceneRenderer : SceneRendererBase
    {
        private const int PickingTargetSize = 512;

        private PickingObjectInfo pickingResult;
        private readonly Dictionary<int, Entity> idToEntity = new Dictionary<int, Entity>();
        private Texture pickingTexture;

        [DataMemberIgnore]
        public RenderStage PickingRenderStage { get; set; }

        protected override void CollectCore(RenderContext context)
        {
            base.CollectCore(context);

            // Fill RenderStage formats
            PickingRenderStage.Output = new RenderOutputDescription(PixelFormat.R32G32_Float, PixelFormat.D32_Float);
            PickingRenderStage.Output.ScissorTestEnable = true;

            context.RenderView.RenderStages.Add(PickingRenderStage);
        }

        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            if (pickingTexture is null)
            {
                // TODO: Release resources!
                pickingTexture = Texture.New2D(drawContext.GraphicsDevice, 1, 1, PickingRenderStage.Output.RenderTargetFormat0, TextureFlags.None, 1, GraphicsResourceUsage.Staging);
            }
            var inputManager = context.Services.GetSafeServiceAs<InputManager>();

            // Skip rendering if mouse position is the same
            var mousePosition = inputManager.MousePosition;

            // TODO: Use RenderFrame
            var pickingRenderTarget = PushScopedResource(context.Allocator.GetTemporaryTexture2D(PickingTargetSize, PickingTargetSize, PickingRenderStage.Output.RenderTargetFormat0));
            var pickingDepthStencil = PushScopedResource(context.Allocator.GetTemporaryTexture2D(PickingTargetSize, PickingTargetSize, PickingRenderStage.Output.DepthStencilFormat, TextureFlags.DepthStencil));

            var renderTargetSize = new Vector2(pickingRenderTarget.Width, pickingRenderTarget.Height);
            var positionInTexture = Vector2.Modulate(renderTargetSize, mousePosition);
            int x = Math.Max(0, Math.Min((int)renderTargetSize.X - 2, (int)positionInTexture.X));
            int y = Math.Max(0, Math.Min((int)renderTargetSize.Y - 2, (int)positionInTexture.Y));

            // Render the picking stage using the current view
            using (drawContext.PushRenderTargetsAndRestore())
            {
                drawContext.CommandList.Clear(pickingRenderTarget, Color.Transparent);
                drawContext.CommandList.Clear(pickingDepthStencil, DepthStencilClearOptions.DepthBuffer);

                drawContext.CommandList.SetRenderTargetAndViewport(pickingDepthStencil, pickingRenderTarget);
                drawContext.CommandList.SetScissorRectangle(new Rectangle(x, y, 1, 1));
                context.RenderSystem.Draw(drawContext, context.RenderView, PickingRenderStage);
                drawContext.CommandList.SetScissorRectangle(new Rectangle());
            }

            // Copy results to 1x1 target
            drawContext.CommandList.CopyRegion(pickingRenderTarget, 0, new ResourceRegion(x, y, 0, x + 1, y + 1, 1), pickingTexture, 0);

            // Get data
            var data = new PickingObjectInfo[1];
            pickingTexture.GetData(drawContext.CommandList, data);
            pickingResult = data[0];
        }

        /// <summary>
        ///   Caches the identifier of all the <see cref="EntityComponent"/>s of the specified <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The entity for which to cache its components.</param>
        /// <param name="isRecursive">A value indicating whether to also cache the components of child entities, recursively.</param>
        public void CacheEntity([NotNull] Entity entity, bool isRecursive)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            foreach (var component in entity.Components)
            {
                idToEntity[RuntimeIdHelper.ToRuntimeId(component)] = component.Entity;
            }

            if (isRecursive)
                foreach (var component in entity.GetChildren().BreadthFirst(x => x.GetChildren()).SelectMany(e => e.Components))
                {
                    idToEntity[RuntimeIdHelper.ToRuntimeId(component)] = component.Entity;
                }
        }

        /// <summary>
        ///   Caches the identifier of a component.
        /// </summary>
        /// <param name="component">The component to cache.</param>
        public void CacheEntityComponent([NotNull] EntityComponent component)
        {
            if (component is null)
                throw new ArgumentNullException(nameof(component));

            idToEntity[RuntimeIdHelper.ToRuntimeId(component)] = component.Entity;
        }

        /// <summary>
        ///   Invalidates the cached identifiers of all the <see cref="EntityComponent"/>s of the specified <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The entity for which to invalidate the cached identifiers of its components.</param>
        /// <param name="isRecursive">A value indicating whether to also invalidate the cached identifiers of the components of child entities, recursively.</param>
        public void UncacheEntity([NotNull] Entity entity, bool isRecursive)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            foreach (var component in entity.Components)
            {
                idToEntity.Remove(RuntimeIdHelper.ToRuntimeId(component));
            }

            if (isRecursive)
                foreach (var component in entity.GetChildren().BreadthFirst(x => x.GetChildren()).SelectMany(e => e.Components))
                {
                    idToEntity.Remove(RuntimeIdHelper.ToRuntimeId(component));
                }
        }

        /// <summary>
        ///   Invalidates a cached identifier of a component.
        /// </summary>
        /// <param name="component">The cached component to invalidate.</param>
        public void UncacheEntityComponent([NotNull] EntityComponent component)
        {
            if (component is null)
                throw new ArgumentNullException(nameof(component));

            idToEntity.Remove(RuntimeIdHelper.ToRuntimeId(component));
        }

        /// <summary>
        ///   Caches the identifiers of all the <see cref="EntityComponent"/>s of all the <see cref="Entity"/>s of a <see cref="Scene"/>.
        /// </summary>
        /// <param name="scene">The scene for which to cache the entity components.</param>
        /// <param name="isRecursive">A value indicating whether to also cache the identifiers of the components of child entities, recursively.</param>
        public void CacheScene([NotNull] Scene scene, bool isRecursive)
        {
            if (scene is null)
                throw new ArgumentNullException(nameof(scene));

            foreach (var entity in scene.Entities)
            {
                CacheEntity(entity, true);
            }

            if (isRecursive)
                foreach (var entity in scene.Children.BreadthFirst(s => s.Children).SelectMany(s => s.Entities))
                {
                    CacheEntity(entity, true);
                }
        }

        /// <summary>
        ///   Invalidates the cached identifiers of all the <see cref="EntityComponent"/>s of all the <see cref="Entity"/>s of a <see cref="Scene"/>.
        /// </summary>
        /// <param name="scene">The scene for which to invalidate the cached identifiers of entity components.</param>
        /// <param name="isRecursive">A value indicating whether to also invalidate the cached identifiers of the components of child entities, recursively.</param>
        public void UncacheScene([NotNull] Scene scene, bool isRecursive)
        {
            if (scene is null)
                throw new ArgumentNullException(nameof(scene));

            foreach (var entity in scene.Entities)
            {
                UncacheEntity(entity, true);
            }

            if (isRecursive)
                foreach (var entity in scene.Children.BreadthFirst(s => s.Children).SelectMany(s => s.Entities))
                {
                    UncacheEntity(entity, true);
                }
        }

        /// <summary>
        ///   Tries to get the <see cref="Entity"/> under the mouse cursor.
        /// </summary>
        /// <returns>An <see cref="EntityPickingResult"/> structure containing information of the picked <see cref="Entity"/>, if any.</returns>
        public EntityPickingResult Pick()
        {
            return pickingResult.GetResult(idToEntity);
        }
    }
}
