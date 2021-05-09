// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using Stride.Core;
using Stride.Core.Collections;
using Stride.Core.Diagnostics;

namespace Stride.Engine
{
    /// <summary>
    ///   A collection of <see cref="EntityComponent"/> managed exclusively by an <see cref="Entity"/>.
    /// </summary>
    [DataContract("EntityComponentCollection")]
    [DebuggerTypeProxy(typeof(CollectionDebugView))]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class EntityComponentCollection : FastCollection<EntityComponent>
    {
        private readonly Entity entity;


        public EntityComponentCollection() { }

        internal EntityComponentCollection(Entity entity)
        {
            this.entity = entity;
        }


        /// <summary>
        ///   This property is only used when merging.
        /// </summary>
        // NOTE: This property set to true internally in some very rare case (merging)
        internal static bool AllowReplaceForeignEntity { get; set; }


        /// <summary>
        ///   Gets the first component of the specified type or derived type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="EntityComponent"/>.</typeparam>
        /// <returns>
        ///   The first component of type <typeparamref name="T"/> found;
        ///   or <c>null</c> if no component of that type were found.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get<T>() where T : EntityComponent
        {
            for (int index = 0; index < Count; index++)
            {
                if (this[index] is T component)
                    return component;
            }

            return null;
        }

        /// <summary>
        ///   Gets the <c>N</c>th component of the specified type or derived type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="EntityComponent"/>.</typeparam>
        /// <param name="index">Index of the component of the same type, starting from zero.</param>
        /// <returns>
        ///   The <paramref name="index"/>th component of type <typeparamref name="T"/> found;
        ///   or <c>null</c> if no component of that type were found or <paramref name="index"/> was out of range.
        /// </returns>
        /// <remarks>
        ///   <list type="bullet">
        ///     <item>A non-zero positive <paramref name="index"/> will search from the beginning of the collection to the end.</item>
        ///     <item>A non-zero negative <paramref name="index"/> will search starting from the end of the collection to the beginning.</item>
        ///     <item>An <paramref name="index"/> of 0 is equivalent to calling <see cref="Get{T}()"/>.</item>
        ///   </list>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get<T>(int index) where T : EntityComponent
        {
            // Start from the end
            if (index < 0)
            {
                for (int i = Count - 1; i >= 0; i--)
                {
                    if (this[i] is T component && ++index == 0)
                        return component;
                }
            }
            // Start from the beginning
            else
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] is T component && index-- == 0)
                        return component;
                }
            }

            return null;
        }

        /// <summary>
        ///   Removes the first component of the specified type or derived type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="EntityComponent"/>.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove<T>() where T : EntityComponent
        {
            for (int index = 0; index < Count; index++)
            {
                if (this[index] is T)
                {
                    RemoveAt(index);
                    break;
                }
            }
        }

        /// <summary>
        ///   Removes all components of the specified type or derived type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="EntityComponent"/>.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAll<T>() where T : EntityComponent
        {
            for (int index = Count - 1; index >= 0; index--)
            {
                if (this[index] is T)
                {
                    RemoveAt(index);
                }
            }
        }

        /// <summary>
        ///   Gets all the components of the specified type or derived type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="EntityComponent"/>.</typeparam>
        /// <returns>An enumeration of the components matching the specified type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<T> GetAll<T>() where T : EntityComponent
        {
            for (int index = 0; index < Count; index++)
            {
                if (this[index] is T component)
                {
                    yield return component;
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClearItems()
        {
            for (int index = Count - 1; index >= 0; index--)
                RemoveItem(index);

            base.ClearItems();
        }

        /// <inheritdoc/>
        protected override void InsertItem(int index, EntityComponent item)
        {
            ValidateItem(index, item, isReplacing: false);

            base.InsertItem(index, item);

            // Notify the entity about this component being updated
            entity?.OnComponentChanged(index, oldComponent: null, newComponent: item);
        }

        /// <inheritdoc/>
        protected override void RemoveItem(int index)
        {
            var item = this[index];
            if (item is TransformComponent)
            {
                entity.TransformValue = null;
            }
            item.Entity = null;

            base.RemoveItem(index);

            // Notify the entity about this component being updated
            entity?.OnComponentChanged(index, oldComponent: item, newComponent: null);
        }

        /// <inheritdoc/>
        protected override void SetItem(int index, EntityComponent item)
        {
            var oldItem = ValidateItem(index, item, isReplacing: true);

            // Detach entity from previous component only when it's different from the new one
            if (item != oldItem)
                oldItem.Entity = null;

            base.SetItem(index, item);

            // Notify the entity about this component being updated
            entity?.OnComponentChanged(index, oldItem, item);
        }

        //
        // Checks whether a Component is valid for adding to the Entity.
        //
        private EntityComponent ValidateItem(int index, EntityComponent component, bool isReplacing)
        {
            if (component is null)
                throw new ArgumentNullException(nameof(component), @"Cannot add a null Component to the Entity.");

            var componentType = component.GetType();
            var attributes = EntityComponentAttributes.Get(componentType);

            var onlySingleComponent = !attributes.AllowMultipleComponents;

            EntityComponent previousItem = null;
            for (int i = 0; i < Count; i++)
            {
                var existingItem = this[i];

                if (index == i && isReplacing)
                {
                    previousItem = existingItem;
                }
                else
                {
                    if (ReferenceEquals(existingItem, component))
                        throw new InvalidOperationException($"Cannot add the same Component multiple times. Already set at index [{i}].");

                    if (onlySingleComponent && componentType == existingItem.GetType())
                        throw new InvalidOperationException($"Cannot add a Component of type [{componentType}] multiple times.");
                }
            }

            if (!AllowReplaceForeignEntity && entity is not null && component.Entity is not null)
                throw new InvalidOperationException($"This Component is already attached to Entity [{component.Entity}] and cannot be attached to [{entity}].");

            if (entity is not null)
            {
                if(component is TransformComponent transform)
                {
                    entity.TransformValue = transform;
                }
                else if (previousItem is TransformComponent)
                {
                    // If previous item was a transform component but we are actually replacing it, we should
                    entity.TransformValue = null;
                }

                component.Entity = entity;
            }

            return previousItem;
        }
    }
}
