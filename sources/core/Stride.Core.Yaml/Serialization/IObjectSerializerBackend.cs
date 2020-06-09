// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Reflection;
using Stride.Core.Yaml.Serialization.Serializers;

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    ///   Provides methods for serializing and deserializing member names and values, collection and dictionary items for
    ///   <see cref="ObjectSerializer"/>, <see cref="DictionarySerializer"/> and <see cref="CollectionSerializer"/>.
    /// </summary>
    /// <remarks>
    ///   The default implementation of this interface is <see cref="DefaultObjectSerializerBackend"/>. It can be derived
    ///   to provide aditionnal behavior.
    /// </remarks>
    // TODO: Explain why this interface is used and how it can be extended for specific scenarios.
    public interface IObjectSerializerBackend
    {
        /// <summary>
        ///   Gets the style that will be used to serialize the object provided by <see cref="ObjectContext.Instance"/>.
        /// </summary>
        /// <param name="objectContext">The object context which contains the object instance to serialize.</param>
        /// <returns>The <see cref="DataStyle"/> to use when serializing the object instance.</returns>
        DataStyle GetStyle(ref ObjectContext objectContext);

        /// <summary>
        ///   Allows to transform the name of the member while reading for the specified object context.
        /// </summary>
        /// <param name="objectContext">The object context to which the member name belongs.</param>
        /// <param name="memberName">Name of the member read from the current YAML stream.</param>
        /// <param name="skipMember">Value indicating whether to skip this member.</param>
        /// <returns>The name that will be used to get the <see cref="IMemberDescriptor"/> from the current object context.</returns>
        string ReadMemberName(ref ObjectContext objectContext, string memberName, out bool skipMember);

        /// <summary>
        ///   Reads the value for the specified member from the current YAML stream.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        /// <param name="member">The member.</param>
        /// <param name="memberValue">The member value.</param>
        /// <param name="memberType">Type of the member.</param>
        /// <returns>The value read from YAML stream.</returns>
        // TODO: Explain memberValue when can be not null
        object ReadMemberValue(ref ObjectContext objectContext, IMemberDescriptor member, object memberValue, Type memberType);

        /// <summary>
        ///   Reads the collection item from the current YAML stream.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        /// <param name="value">The value.</param>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="index">The current item index in the collection.</param>
        /// <returns>The collection item read from YAML stream.</returns>
        object ReadCollectionItem(ref ObjectContext objectContext, object value, Type itemType, int index);

        /// <summary>
        ///   Reads the key of the dictionary item from the current YAML stream.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        /// <param name="keyType">Type of the key.</param>
        /// <returns>The key of the dictionary item read from YAML stream.</returns>
        object ReadDictionaryKey(ref ObjectContext objectContext, Type keyType);

        /// <summary>
        ///   Reads the value of the dictionary item from the current YAML stream.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="key">The key corresponding to the value.</param>
        /// <returns>The value of the dictionary item read from YAML stream.</returns>
        object ReadDictionaryValue(ref ObjectContext objectContext, Type valueType, object key);


        /// <summary>
        ///   Writes the name of the member.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        /// <param name="member">The member.</param>
        /// <param name="memberName">Name of the member.</param>
        void WriteMemberName(ref ObjectContext objectContext, IMemberDescriptor member, string memberName);

        /// <summary>
        ///   Writes the member value.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        /// <param name="member">The member.</param>
        /// <param name="memberValue">The member value.</param>
        /// <param name="memberType">Type of the member.</param>
        void WriteMemberValue(ref ObjectContext objectContext, IMemberDescriptor member, object memberValue, Type memberType);

        /// <summary>
        ///   Writes the collection item.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        /// <param name="item">The item.</param>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="index">The current index of the item in the collection.</param>
        void WriteCollectionItem(ref ObjectContext objectContext, object item, Type itemType, int index);

        /// <summary>
        ///   Writes the key of the dictionary item.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        /// <param name="key">The key of the dictionary item.</param>
        /// <param name="keyType">Type of the key.</param>
        void WriteDictionaryKey(ref ObjectContext objectContext, object key, Type keyType);

        /// <summary>
        ///   Writes the value of the dictionary item.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        /// <param name="key"></param>
        /// <param name="value">The value of the dictionary item.</param>
        /// <param name="valueType">Type of the value.</param>
        void WriteDictionaryValue(ref ObjectContext objectContext, object key, object value, Type valueType);


        /// <summary>
        ///   Determines if the given member should be serialized.
        /// </summary>
        /// <param name="member">The member to evaluate.</param>
        /// <param name="objectContext">The object context.</param>
        /// <returns><c>true</c> if the member should be serialized; <c>false</c> otherwise.</returns>
        bool ShouldSerialize(IMemberDescriptor member, ref ObjectContext objectContext);
    }
}
