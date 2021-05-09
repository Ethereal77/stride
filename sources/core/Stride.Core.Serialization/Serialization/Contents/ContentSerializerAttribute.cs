// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Serialization.Contents
{
    /// <summary>
    /// Allows customization of IContentSerializer through an attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class ContentSerializerAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentSerializerAttribute"/> class.
        /// </summary>
        /// <param name="contentSerializerType">Type of the content serializer.</param>
        public ContentSerializerAttribute(Type contentSerializerType)
        {
            ContentSerializerType = contentSerializerType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentSerializerAttribute"/> class.
        /// </summary>
        public ContentSerializerAttribute()
        {
        }

        /// <summary>
        /// Gets the type of the content serializer.
        /// </summary>
        /// <value>
        /// The type of the content serializer.
        /// </value>
        public Type ContentSerializerType { get; private set; }
    }
}
