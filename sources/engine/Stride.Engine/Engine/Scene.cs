// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Collections;
using Stride.Core.Mathematics;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;

namespace Stride.Engine
{
    /// <summary>
    ///   Represents a scene as a collection of <see cref="Entity"/>s related to some common space.
    /// </summary>
    /// <remarks>
    ///   Scenes can form transform hierarchies with other Scenes, the same way <see cref="Entity"/>s do, and can
    ///   have local transformations that are applied to all their children Entities.
    /// </remarks>
    [DataContract("Scene")]
    [ContentSerializer(typeof(DataContentSerializerWithReuse<Scene>))]
    [ReferenceSerializer, DataSerializerGlobal(typeof(ReferenceSerializer<Scene>), Profile = "Content")]
    public sealed class Scene : ComponentBase, IIdentifiable
    {
        private Scene parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// </summary>
        public Scene()
        {
            Id = Guid.NewGuid();
            Entities = new EntityCollection(this);

            Children = new SceneCollection(this);
        }

        [DataMember(-10)]
        [Display(Browsable = false)]
        [NonOverridable]
        public Guid Id { get; set; }

        /// <summary>
        ///   Gets or sets the parent scene.
        /// </summary>
        /// <value>The parent scene. The value will be <c>null</c> when this Scene is the Root Scene (it has no parent).</value>
        [DataMemberIgnore]
        public Scene Parent
        {
            get => parent;

            set
            {
                var oldParent = Parent;
                if (oldParent == value)
                    return;

                oldParent?.Children.Remove(this);
                value?.Children.Add(this);
            }
        }

        /// <summary>
        ///   Gets the collection of Entities belonging to this Scene.
        /// </summary>
        public TrackingCollection<Entity> Entities { get; }

        /// <summary>
        ///   Gets the collection of child Scenes belonging to this Scene.
        /// </summary>
        [DataMemberIgnore]
        public TrackingCollection<Scene> Children { get; }

        /// <summary>
        ///   An offset of the Scene relative to it's parent Scene applied to all its Entities' positions.
        /// </summary>
        public Vector3 Offset;

        /// <summary>
        ///   The absolute Transform applied to all entities of the Scene.
        /// </summary>
        /// <remarks>This field is overwritten by the <see cref="Processors.TransformProcessor"/> each frame.</remarks>
        public Matrix WorldMatrix;


        /// <summary>
        ///   Updates the world transform of the Scene.
        /// </summary>
        public void UpdateWorldMatrix()
        {
            UpdateWorldMatrixInternal(isRecursive: true);
        }

        /// <summary>
        ///   Updates the transform hierarchy of the Scene and its children recursively.
        /// </summary>
        /// <param name="isRecursive">A value indicating whether to also update the transform of child Scenes.</param>
        internal void UpdateWorldMatrixInternal(bool isRecursive)
        {
            if (parent is not null)
            {
                if (isRecursive)
                    parent.UpdateWorldMatrixInternal(isRecursive: true);

                WorldMatrix = parent.WorldMatrix;
            }
            else
            {
                WorldMatrix = Matrix.Identity;
            }

            WorldMatrix.TranslationVector += Offset;
        }

        public override string ToString() => $"Scene {Name}";


        /// <summary>
        ///   Represents a collection of <see cref="Entity"/>s of a <see cref="Scene"/>.
        /// </summary>
        [DataContract]
        public class EntityCollection : TrackingCollection<Entity>
        {
            private readonly Scene scene;

            public EntityCollection(Scene scene)
            {
                this.scene = scene;
            }

            /// <inheritdoc/>
            protected override void InsertItem(int index, Entity item)
            {
                // Root Entity in another Scene, or child of another Entity
                if (item.Scene is not null)
                    throw new InvalidOperationException("This Entity already has a Scene. Detach it first.");

                item.SceneValue = scene;

                base.InsertItem(index, item);
            }

            /// <inheritdoc/>
            protected override void RemoveItem(int index)
            {
                var item = this[index];

                if (item.SceneValue != scene)
                    throw new InvalidOperationException("This Entity's Scene is not the expected value.");

                item.SceneValue = null;

                base.RemoveItem(index);
            }
        }

        /// <summary>
        ///   Represents a collection of child <see cref="Scene"/>s of a <see cref="Scene"/>.
        /// </summary>
        [DataContract]
        public class SceneCollection : TrackingCollection<Scene>
        {
            private readonly Scene scene;

            public SceneCollection(Scene scene)
            {
                this.scene = scene;
            }

            /// <inheritdoc/>
            protected override void InsertItem(int index, Scene item)
            {
                if (item.Parent is not null)
                    throw new InvalidOperationException("This Scene already has a Parent. Detach it first.");

                item.parent = scene;

                base.InsertItem(index, item);
            }

            /// <inheritdoc/>
            protected override void RemoveItem(int index)
            {
                var item = this[index];

                if (item.Parent != scene)
                    throw new InvalidOperationException("This Scene's parent is not the expected value.");

                item.parent = null;

                base.RemoveItem(index);
            }
        }
    }
}
