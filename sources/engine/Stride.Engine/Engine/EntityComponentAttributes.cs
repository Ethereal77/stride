// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

using Stride.Core.Annotations;

namespace Stride.Engine
{
    /// <summary>
    ///   Represents a compact way to query the attributes annotating an <see cref="EntityComponent"/>.
    /// </summary>
    public struct EntityComponentAttributes
    {
        private static readonly Dictionary<Type, EntityComponentAttributes> ComponentAttributes = new();


        private EntityComponentAttributes(bool allowMultipleComponents)
        {
            AllowMultipleComponents = allowMultipleComponents;
        }


        /// <summary>
        ///   Indicates whether the <see cref="EntityComponent"/> is supporting multiple Components of the same type on an Entity.
        /// </summary>
        public readonly bool AllowMultipleComponents;


        /// <summary>
        ///   Gets the attributes for the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the Component.</typeparam>
        /// <returns>The attributes for the specified type.</returns>
        public static EntityComponentAttributes Get<T>() where T : EntityComponent
        {
            return GetInternal(typeof(T));
        }

        /// <summary>
        ///   Gets the attributes for the specified type.
        /// </summary>
        /// <param name="type">The type of the Component.</param>
        /// <returns>The attributes for the specified type.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="type"/> must be of <see cref="EntityComponent"/> type.</exception>
        public static EntityComponentAttributes Get([NotNull] Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (!typeof(EntityComponent).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                throw new ArgumentException(@"The type must be derived from EntityComponent.", nameof(type));

            return GetInternal(type);
        }

        private static EntityComponentAttributes GetInternal([NotNull] Type type)
        {
            lock (ComponentAttributes)
            {
                if (!ComponentAttributes.TryGetValue(type, out EntityComponentAttributes attributes))
                {
                    attributes = new EntityComponentAttributes(type.GetTypeInfo().GetCustomAttribute<AllowMultipleComponentsAttribute>() is not null);
                    ComponentAttributes.Add(type, attributes);
                }
                return attributes;
            }
        }
    }
}
