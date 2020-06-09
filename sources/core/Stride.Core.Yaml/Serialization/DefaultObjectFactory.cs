// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

using Stride.Core.Reflection;

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    ///   Represents the default <see cref="IObjectFactory"/> implementation that creates objects using
    ///   <see cref="Activator.CreateInstance"/>.
    /// </summary>
    public sealed class DefaultObjectFactory : IObjectFactory
    {
        private static readonly Type[] EmptyTypes = Array.Empty<Type>();

        private static readonly Dictionary<Type, Type> DefaultInterfaceImplementations = new Dictionary<Type, Type>
        {
            {typeof(IList),           typeof(List<object>)},
            {typeof(IDictionary),     typeof(Dictionary<object, object>)},
            {typeof(IEnumerable<>),   typeof(List<>)},
            {typeof(ICollection<>),   typeof(List<>)},
            {typeof(IList<>),         typeof(List<>)},
            {typeof(IDictionary<,>),  typeof(Dictionary<,>)}
        };

        /// <summary>
        ///   Gets the default implementation for a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   The <see cref="Type"/> of the implementation for the specified <paramref name="type"/>;
        ///   or the same type as input if there is no default implementation.
        /// </returns>
        public static Type GetDefaultImplementation(Type type)
        {
            if (type is null)
                return null;

            // TODO change this code. Make it configurable?
            if (type.IsInterface)
            {
                if (type.IsGenericType)
                {
                    if (DefaultInterfaceImplementations.TryGetValue(type.GetGenericTypeDefinition(), out Type implementationType))
                    {
                        type = implementationType.MakeGenericType(type.GetGenericArguments());
                    }
                }
                else
                {
                    if (DefaultInterfaceImplementations.TryGetValue(type, out Type implementationType))
                    {
                        type = implementationType;
                    }
                }
            }
            return type;
        }

        public object Create(Type type)
        {
            type = GetDefaultImplementation(type);

            // We can't instantiate primitive or arrays
            if (PrimitiveDescriptor.IsPrimitive(type) || type.IsArray)
                return null;

            if (type.GetConstructor(EmptyTypes) != null || type.IsValueType)
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (Exception ex)
                {
                    throw new InstanceCreationException($"'{typeof(Activator)}' failed to create instance of type '{type}'.", ex);
                }
            }

            return null;
        }

        public class InstanceCreationException : Exception
        {
            public InstanceCreationException(string message, Exception innerException) : base(message, innerException) { }
        }
    }
}
