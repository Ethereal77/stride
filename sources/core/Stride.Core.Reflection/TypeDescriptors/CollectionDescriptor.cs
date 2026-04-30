// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Stride.Core.Yaml.Serialization;

namespace Stride.Core.Reflection
{
    /// <summary>
    ///   Provides a descriptor for a <see cref="ICollection"/>.
    /// </summary>
    public abstract class CollectionDescriptor : ObjectDescriptor
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="CollectionDescriptor" /> class.
        /// </summary>
        /// <param name="factory">The type descriptors factory.</param>
        /// <param name="type">The type of <see cref="ICollection"/>.</param>
        /// <exception cref="ArgumentException">Expecting a type inheriting from System.Collections.ICollection;type</exception>
        public CollectionDescriptor(ITypeDescriptorFactory factory, Type type, bool emitDefaultValues, IMemberNamingConvention namingConvention)
            : base(factory, type, emitDefaultValues, namingConvention)
        { }

        /// <summary>
        ///   Determines whether the specified type is a collection type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is collection; otherwise, <c>false</c>.</returns>
        public static bool IsCollection(Type type)
        {
            return TypeHelper.IsCollection(type);
        }

        /// <summary>
        ///   Gets or sets the type of elements in the collection.
        /// </summary>
        public Type ElementType { get; protected set; }

        /// <summary>
        ///   Gets a value indicating whether this instance is a pure collection (no public properties / fields).
        /// </summary>
        public bool IsPureCollection { get; protected set; }

        /// <summary>
        ///   Gets a value indicating whether this collection type has an Add method.
        /// </summary>
        public bool HasAdd { get; protected set; }

        /// <summary>
        ///   Gets a value indicating whether this collection type has a Remove method.
        /// </summary>
        public bool HasRemove { get; protected set; }

        /// <summary>
        ///   Gets a value indicating whether this collection type has an Insert method.
        /// </summary>
        public bool HasInsert { get; protected set; }

        /// <summary>
        ///   Gets a value indicating whether this collection type has a RemoveAt method.
        /// </summary>
        public bool HasRemoveAt { get; protected set; }

        /// <summary>
        ///   Gets a value indicating whether this collection type has valid indexer accessors.
        ///   If it does, <see cref="SetValue(object, object, object)"/> and <see cref="GetValue(object, object)"/>
        ///   can be invoked.
        /// </summary>
        public virtual bool HasIndexerAccessors { get; protected set; }

        /// <summary>
        ///   Determines whether the specified collection is read-only.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns><c>true</c> if the specified collection is read-only; otherwise, <c>false</c>.</returns>
        public abstract bool IsReadOnly(object collection);

        /// <summary>
        ///   Returns the value matching the given index in the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index of the value to get.</param>
        public abstract object GetValue(object collection, object index);

        /// <summary>
        ///   Returns the value matching the given index in the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index of the value to get.</param>
        public abstract object GetValue(object collection, int index);

        public abstract void SetValue(object list, object index, object value);

        /// <summary>
        ///   Adds a value to the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="value">The value to add to this collection.</param>
        public abstract void Add(object collection, object value);

        /// <summary>
        ///   Insert a value to the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index where to insert the value.</param>
        /// <param name="value">The value to insert to this collection.</param>
        public abstract void Insert(object collection, int index, object value);

        /// <summary>
        ///   Removes the item at the given index from the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index of the item to remove from the collection.</param>
        public abstract void Remove(object collection, object item);

        /// <summary>
        ///   Removes a value from a collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item to remove from the collection.</param>
        public abstract void RemoveAt(object collection, int index);

        /// <summary>
        ///   Clears the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public abstract void Clear(object collection);

        /// <summary>
        ///   Determines the number of elements of a collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>The number of elements in the collection; or <c>-1</c> if it cannot determine the number of elements.</returns>
        public abstract int GetCollectionCount(object collection);
    }
}
