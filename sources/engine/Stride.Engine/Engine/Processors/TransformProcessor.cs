// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Stride.Core.Collections;
using Stride.Core.Threading;
using Stride.Rendering;

namespace Stride.Engine.Processors
{
    /// <summary>
    ///   Represents an <see cref="EntityProcessor"/> that handles the <see cref="TransformComponent.Children"/> of an <see cref="Entity"/>
    ///   and updates the <see cref="TransformComponent.WorldMatrix"/>.
    /// </summary>
    public class TransformProcessor : EntityProcessor<TransformComponent>
    {
        /// <summary>
        ///   List of root entities <see cref="TransformComponent"/> of every <see cref="Entity"/> in <see cref="EntityManager"/>.
        /// </summary>
        internal readonly HashSet<TransformComponent> TransformationRoots = new();

        /// <summary>
        ///   List of the components that are not special roots.
        /// </summary>
        /// <remarks>This field is instantiated here to avoid reallocation every frame.</remarks>
        private readonly FastCollection<TransformComponent> notSpecialRootComponents = new();
        private readonly FastCollection<TransformComponent> modelNodeLinkComponents = new();

        private ModelNodeLinkProcessor modelNodeLinkProcessor;
        private ModelNodeLinkProcessor ModelNodeLinkProcessor
        {
            get
            {
                if (modelNodeLinkProcessor is null)
                    modelNodeLinkProcessor = EntityManager.Processors.OfType<ModelNodeLinkProcessor>().FirstOrDefault();

                return modelNodeLinkProcessor;
            }
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="TransformProcessor" /> class.
        /// </summary>
        public TransformProcessor()
        {
            Order = -200;
        }


        /// <inheritdoc/>
        protected internal override void OnSystemAdd() { }

        /// <inheritdoc/>
        protected internal override void OnSystemRemove()
        {
            TransformationRoots.Clear();
        }

        /// <inheritdoc/>
        protected override void OnEntityComponentAdding(Entity entity, TransformComponent component, TransformComponent data)
        {
            if (component.Parent is null)
                TransformationRoots.Add(component);

            foreach (var child in data.Children)
                InternalAddEntity(child.Entity);
        }

        /// <inheritdoc/>
        protected override void OnEntityComponentRemoved(Entity entity, TransformComponent component, TransformComponent data)
        {
            var entityToRemove = new List<Entity>(capacity: data.Children.Count);
            foreach (var child in data.Children)
                entityToRemove.Add(child.Entity);

            foreach (var childEntity in entityToRemove)
                InternalRemoveEntity(childEntity, removeParent: false);

            if (component.Parent is null)
                TransformationRoots.Remove(component);
        }

        /// <summary>
        ///   Traverses the transformation hierarchy updating the world matrices of each Entity's <see cref="TransformComponent"/>.
        /// </summary>
        /// <param name="transformationComponents">The transformation components to update.</param>
        internal void UpdateTransformations(FastCollection<TransformComponent> transformationComponents)
        {
            Dispatcher.ForEach(transformationComponents, UpdateTransformationAndChildren);

            // Re-update Model Node Links to avoid one frame delay compared to reference Model
            // TODO: Entities should be sorted to avoid this in future
            if (ModelNodeLinkProcessor != null)
            {
                modelNodeLinkComponents.Clear();
                foreach (var modelNodeLink in ModelNodeLinkProcessor.ModelNodeLinkComponents)
                {
                    modelNodeLinkComponents.Add(modelNodeLink.Entity.Transform);
                }
                Dispatcher.ForEach(modelNodeLinkComponents, UpdateTransformationAndChildren);
            }
        }

        //
        // Updates the transformation of an Entity and recursively does the same with its children.
        //
        private static void UpdateTransformationAndChildren(TransformComponent transformation)
        {
            UpdateTransformation(transformation);

            // Recurse
            if (transformation.Children.Count > 0)
                UpdateTransformationsRecursive(transformation.Children);
        }

        //
        // Updates the transformation of a collection of Entities and recursively does the same with their children.
        //
        private static void UpdateTransformationsRecursive(FastCollection<TransformComponent> transformationComponents)
        {
            foreach (var transformation in transformationComponents)
            {
                UpdateTransformation(transformation);

                // Recurse
                if (transformation.Children.Count > 0)
                    UpdateTransformationsRecursive(transformation.Children);
            }
        }

        //
        // Updates the local and world matrices of an Entity based on the transform hierarchy.
        //
        private static void UpdateTransformation(TransformComponent transform)
        {
            // Update transform
            transform.UpdateLocalMatrix();
            transform.UpdateWorldMatrixInternal(false);
        }

        /// <summary>
        ///   Updates all the <see cref="TransformComponent.WorldMatrix"/> prior to drawing the frame.
        /// </summary>
        /// <param name="context">The render context.</param>
        public override void Draw(RenderContext context)
        {
            notSpecialRootComponents.Clear();
            foreach (var root in TransformationRoots)
                notSpecialRootComponents.Add(root);

            // Update scene transforms
            // TODO: Entity processors should not be aware of scenes
            var sceneInstance = EntityManager as SceneInstance;
            if (sceneInstance?.RootScene is not null)
            {
                UpdateTransfromationsRecursive(sceneInstance.RootScene);
            }

            // Special roots are already filtered out
            UpdateTransformations(notSpecialRootComponents);
        }

        //
        // Updates the transformation of the Entities of a Scene and recursively does the same with its children Scenes.
        //
        private static void UpdateTransfromationsRecursive(Scene scene)
        {
            scene.UpdateWorldMatrixInternal(isRecursive: false);

            foreach (var childScene in scene.Children)
                UpdateTransfromationsRecursive(childScene);
        }

        /// <summary>
        ///   Notifies that the collection of child Entities of an Entity has changed and there is a Entity that was
        ///   added or removed that has to be tracked accordingly.
        /// </summary>
        /// <param name="transformComponent">The transformation added or removed.</param>
        /// <param name="isAdded"></param>
        internal void NotifyChildrenCollectionChanged(TransformComponent transformComponent, bool isAdded)
        {
            // Ignore if transform component is being moved inside the same root Scene (no need to Add / Remove)
            if (transformComponent.IsMovingInsideRootScene)
            {
                // Still need to update transformation roots
                if (transformComponent.Parent is null)
                {
                    if(isAdded)
                        TransformationRoots.Add(transformComponent);
                    else
                        TransformationRoots.Remove(transformComponent);
                }
            }
            // Added / Removed children of Entities in the Entity Manager have to be Added / Removed of the Entity Manager
            else
            {
                if (isAdded)
                    InternalAddEntity(transformComponent.Entity);
                else
                    InternalRemoveEntity(transformComponent.Entity, false);
            }
        }
    }
}
