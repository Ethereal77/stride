// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stride.Core.Serialization.Contents
{
    /// <summary>
    /// Base class for Content Serializer with empty virtual implementation.
    /// </summary>
    /// <typeparam name="T">Runtime type being serialized.</typeparam>
    public class ContentSerializerBase<T> : IContentSerializer<T>
    {
        private static readonly bool HasParameterlessConstructor = typeof(T).GetTypeInfo().DeclaredConstructors.Any(x => !x.IsStatic && x.IsPublic && !x.GetParameters().Any());

        /// <inheritdoc/>
        public virtual Type SerializationType
        {
            get { return typeof(T); }
        }

        /// <inheritdoc/>
        public virtual Type ActualType
        {
            get { return typeof(T); }
        }
        
        /// <inheritdoc/>
        public virtual object Construct(ContentSerializerContext context)
        {
            return HasParameterlessConstructor ? Activator.CreateInstance<T>() : default(T);
        }

        /// <inheritdoc/>
        public virtual void Serialize(ContentSerializerContext context, SerializationStream stream, T obj)
        {
        }

        /// <inheritdoc/>
        public void Serialize(ContentSerializerContext context, SerializationStream stream, object obj)
        {
            var objT = (T)obj;
            Serialize(context, stream, objT);
        }
    }
}
