// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

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
    public class CollectionDescriptor : ObjectDescriptor
    {
        private static readonly object[] EmptyObjects = Array.Empty<object>();
        private static readonly List<string> ListOfMembersToRemove = new List<string> { "Capacity", "Count", "IsReadOnly", "IsFixedSize", "IsSynchronized", "SyncRoot", "Comparer" };

        private readonly Func<object, bool> IsReadOnlyFunction;
        private readonly Func<object, int> GetCollectionCountFunction;
        private readonly Func<object, int, object> GetIndexedItem;
        private readonly Action<object, int, object> SetIndexedItem;
        private readonly Action<object, object> CollectionAddFunction;
        private readonly Action<object, int, object> CollectionInsertFunction;
        private readonly Action<object, int> CollectionRemoveAtFunction;
        private readonly Action<object, object> CollectionRemoveFunction;
        private readonly Action<object> CollectionClearFunction;

        public override DescriptorCategory Category => DescriptorCategory.Collection;

        /// <summary>
        ///   Gets the type of the elements in the collection.
        /// </summary>
        /// <value>The type of the elements.</value>
        public Type ElementType { get; }

        /// <summary>
        ///   Gets a value indicating whether this instance is a pure collection (no public properties nor fields).
        /// </summary>
        /// <value><c>true</c> if this instance is a pure collection; otherwise, <c>false</c>.</value>
        /// <remarks>
        ///   A pure collection is one that can be represented exclusively by the elements it contains, because
        ///   it has no other state publicly accesible through fields or properties.
        /// </remarks>
        public bool IsPureCollection { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether this collection type has <c>Add</c> method.
        /// </summary>
        /// <value><c>true</c> if this instance has <c>Add</c>; otherwise, <c>false</c>.</value>
        public bool HasAdd => CollectionAddFunction != null;

        /// <summary>
        ///   Gets a value indicating whether this collection type has <c>Insert</c> method.
        /// </summary>
        /// <value><c>true</c> if this instance has <c>Insert</c>; otherwise, <c>false</c>.</value>
        public bool HasInsert => CollectionInsertFunction != null;

        /// <summary>
        ///   Gets a value indicating whether this collection type has <c>RemoveAt</c> method.
        /// </summary>
        /// <value><c>true</c> if this instance has <c>RemoveAt</c>; otherwise, <c>false</c>.</value>
        public bool HasRemoveAt => CollectionRemoveAtFunction != null;

        /// <summary>
        ///   Gets a value indicating whether this collection type has <c>Remove</c> method.
        /// </summary>
        /// <value><c>true</c> if this instance has <c>Remove</c>; otherwise, <c>false</c>.</value>
        public bool HasRemove => CollectionRemoveFunction != null;

        /// <summary>
        ///   Gets a value indicating whether this collection type has valid indexer accessors.
        ///   If so, <see cref="SetValue(object, object, object)"/> and <see cref="GetValue(object, object)"/> can be invoked.
        /// </summary>
        /// <value><c>true</c> if this instance has a valid indexer accesors; otherwise, <c>false</c>.</value>
        public bool HasIndexerAccessors { get; }

        /// <summary>
        ///   Gets a value indicating whether this collection implements <see cref="IList"/> or <see cref="IList{T}"/>.
        /// </summary>
        public bool IsList { get; }


        /// <summary>
        ///   Initializes a new instance of the <see cref="CollectionDescriptor" /> class.
        /// </summary>
        /// <param name="factory">The type descriptors factory.</param>
        /// <param name="type">The type of <see cref="ICollection"/>.</param>
        /// <exception cref="ArgumentException">Expecting a type inheriting from System.Collections.ICollection;type</exception>
        public CollectionDescriptor(ITypeDescriptorFactory factory, Type type, bool emitDefaultValues, IMemberNamingConvention namingConvention)
            : base(factory, type, emitDefaultValues, namingConvention)
        {
            // Gets the element type
            ElementType = type.GetInterface(typeof(IEnumerable<>))?.GetGenericArguments()[0] ?? typeof(object);

            // If it implements IList
            if (typeof(IList).IsAssignableFrom(type))
            {
                CollectionAddFunction = (obj, value) => ((IList)obj).Add(value);
                CollectionClearFunction = obj => ((IList)obj).Clear();
                CollectionInsertFunction = (obj, index, value) => ((IList)obj).Insert(index, value);
                CollectionRemoveAtFunction = (obj, index) => ((IList)obj).RemoveAt(index);
                GetCollectionCountFunction = o => ((IList)o).Count;
                GetIndexedItem = (obj, index) => ((IList)obj)[index];
                SetIndexedItem = (obj, index, value) => ((IList)obj)[index] = value;
                IsReadOnlyFunction = obj => ((IList)obj).IsReadOnly;
                HasIndexerAccessors = true;
                IsList = true;
            }
            // If it implements ICollection<T>
            else if (type.GetInterface(typeof(ICollection<>)) is Type itype)
            {
                var add = itype.GetMethod(nameof(ICollection<object>.Add), new[] { ElementType });
                CollectionAddFunction = (obj, value) => add.Invoke(obj, new[] { value });
                var remove = itype.GetMethod(nameof(ICollection<object>.Remove), new[] { ElementType });
                CollectionRemoveFunction = (obj, value) => remove.Invoke(obj, new[] { value });
                if (typeof(IDictionary).IsAssignableFrom(type))
                {
                    CollectionClearFunction = obj => ((IDictionary)obj).Clear();
                    GetCollectionCountFunction = o => ((IDictionary)o).Count;
                    IsReadOnlyFunction = obj => ((IDictionary)obj).IsReadOnly;
                }
                else
                {
                    var clear = itype.GetMethod(nameof(ICollection<object>.Clear), Type.EmptyTypes);
                    CollectionClearFunction = obj => clear.Invoke(obj, EmptyObjects);
                    var countMethod = itype.GetProperty(nameof(ICollection<object>.Count)).GetGetMethod();
                    GetCollectionCountFunction = o => (int)countMethod.Invoke(o, null);
                    var isReadOnly = itype.GetProperty(nameof(ICollection<object>.IsReadOnly)).GetGetMethod();
                    IsReadOnlyFunction = obj => (bool)isReadOnly.Invoke(obj, null);
                }
                // Implements IList<T>
                itype = type.GetInterface(typeof(IList<>));
                if (itype != null)
                {
                    var insert = itype.GetMethod(nameof(IList<object>.Insert), new[] { typeof(int), ElementType });
                    CollectionInsertFunction = (obj, index, value) => insert.Invoke(obj, new[] { index, value });
                    var removeAt = itype.GetMethod(nameof(IList<object>.RemoveAt), new[] { typeof(int) });
                    CollectionRemoveAtFunction = (obj, index) => removeAt.Invoke(obj, new object[] { index });
                    var getItem = itype.GetMethod("get_Item", new[] { typeof(int) });
                    GetIndexedItem = (obj, index) => getItem.Invoke(obj, new object[] { index });
                    var setItem = itype.GetMethod("set_Item", new[] { typeof(int), ElementType });
                    SetIndexedItem = (obj, index, value) => setItem.Invoke(obj, new[] { index, value });
                    HasIndexerAccessors = true;
                    IsList = true;
                }
                else
                {
                    // Attempt to retrieve IList<> accessors from ICollection.
                    var insert = type.GetMethod(nameof(IList<object>.Insert), new[] { typeof(int), ElementType });
                    if (insert != null)
                        CollectionInsertFunction = (obj, index, value) => insert.Invoke(obj, new[] { index, value });

                    var removeAt = type.GetMethod(nameof(IList<object>.RemoveAt), new[] { typeof(int) });
                    if (removeAt != null)
                        CollectionRemoveAtFunction = (obj, index) => removeAt.Invoke(obj, new object[] { index });

                    var getItem = type.GetMethod("get_Item", new[] { typeof(int) });
                    if (getItem != null)
                        GetIndexedItem = (obj, index) => getItem.Invoke(obj, new object[] { index });

                    var setItem = type.GetMethod("set_Item", new[] { typeof(int), ElementType });
                    if (setItem != null)
                        SetIndexedItem = (obj, index, value) => setItem.Invoke(obj, new[] { index, value });

                    HasIndexerAccessors = getItem != null && setItem != null;
                }
            }
            else
            {
                throw new ArgumentException($"Type [{(type)}] is not supported as a modifiable collection");
            }
        }

        public override void Initialize(IComparer<object> keyComparer)
        {
            base.Initialize(keyComparer);

            IsPureCollection = Count == 0;
        }


        /// <summary>
        ///   Returns the value matching the given index in the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index of the value to get.</param>
        public object GetValue(object collection, object index)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (!(index is int))
                throw new ArgumentException("The index must be an int.");

            return GetValue(collection, (int) index);
        }

        /// <summary>
        ///   Returns the value matching the given index in the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index of the value to get.</param>
        public object GetValue(object collection, int index)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            return GetIndexedItem(collection, index);
        }

        public void SetValue(object list, object index, object value)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            if (!(index is int))
                throw new ArgumentException("The index must be an int.");

            SetValue(list, (int) index, value);
        }

        public void SetValue(object list, int index, object value)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            SetIndexedItem(list, index, value);
        }

        /// <summary>
        ///   Clears the specified collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public void Clear(object collection)
        {
            CollectionClearFunction(collection);
        }

        /// <summary>
        ///   Adds a value to a collection of the type described by this descriptor.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="value">The value to add to this collection.</param>
        public void Add(object collection, object value)
        {
            CollectionAddFunction(collection, value);
        }

        /// <summary>
        ///   Insert a value to a collection of the type described by this descriptor.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index where to insert the value.</param>
        /// <param name="value">The value to insert to this collection.</param>
        public void Insert(object collection, int index, object value)
        {
            CollectionInsertFunction(collection, index, value);
        }

        /// <summary>
        ///   Remove item at the given index from the collections of the same type.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index of the item to remove from the collection.</param>
        public void RemoveAt(object collection, int index)
        {
            CollectionRemoveAtFunction(collection, index);
        }

        /// <summary>
        ///   Removes a value from a collection of the type described by this descriptor.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item to remove from the collection.</param>
        public void Remove(object collection, object item)
        {
            CollectionRemoveFunction(collection, item);
        }

        /// <summary>
        ///   Determines whether the specified collection is read only.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns><c>true</c> if the specified collection is read only; otherwise, <c>false</c>.</returns>
        public bool IsReadOnly(object collection)
        {
            return collection is null || IsReadOnlyFunction is null || IsReadOnlyFunction(collection);
        }

        /// <summary>
        ///   Determines the number of elements of a collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>The number of elements in the collection; or <c>-1</c> if it cannot determine the number of elements.</returns>
        public int GetCollectionCount(object collection)
        {
            return collection is null || GetCollectionCountFunction is null ? -1 : GetCollectionCountFunction(collection);
        }

        /// <summary>
        ///   Determines whether the specified <see cref="Type"/> represents a collection.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified <see cref="Type"/> is a collection; otherwise, <c>false</c>.</returns>
        public static bool IsCollection(Type type)
        {
            return TypeHelper.IsCollection(type);
        }

        protected override bool PrepareMember(MemberDescriptorBase member, MemberInfo metadataClassMemberInfo)
        {
            // Filter members
            if (member is PropertyDescriptor && ListOfMembersToRemove.Contains(member.OriginalName))
            //if (member is PropertyDescriptor && (member.DeclaringType.Namespace ?? string.Empty).StartsWith(SystemCollectionsNamespace) && ListOfMembersToRemove.Contains(member.Name))
            {
                return false;
            }

            return !IsCompilerGenerated && base.PrepareMember(member, metadataClassMemberInfo);
        }
    }
}
