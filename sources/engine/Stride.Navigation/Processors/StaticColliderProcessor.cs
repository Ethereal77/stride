// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Engine;
using Stride.Physics;

namespace Stride.Navigation.Processors
{
    internal class StaticColliderProcessor : EntityProcessor<StaticColliderComponent, StaticColliderData>
    {
        public delegate void CollectionChangedEventHandler(StaticColliderComponent component, StaticColliderData data);

        public event CollectionChangedEventHandler ColliderAdded;
        public event CollectionChangedEventHandler ColliderRemoved;

        /// <inheritdoc />
        protected override StaticColliderData GenerateComponentData(Entity entity, StaticColliderComponent component)
        {
            return new StaticColliderData { Component = component, };
        }

        /// <inheritdoc />
        protected override bool IsAssociatedDataValid(Entity entity, StaticColliderComponent component, StaticColliderData associatedData)
        {
            return component == associatedData.Component;
        }

        /// <inheritdoc />
        protected override void OnEntityComponentAdding(Entity entity, StaticColliderComponent component, StaticColliderData data)
        {
            ColliderAdded?.Invoke(component, data);
        }

        /// <inheritdoc />
        protected override void OnEntityComponentRemoved(Entity entity, StaticColliderComponent component, StaticColliderData data)
        {
            ColliderRemoved?.Invoke(component, data);
        }
    }
}
