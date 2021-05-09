// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core
{
    /// <summary>
    ///   Specifies the metadata class to associate with a serializable class.
    ///   The main usage of this class is to allow a sub-class to override property
    ///   attributes such as <see cref="Stride.Core.DefaultValueAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DataContractMetadataTypeAttribute : Attribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="DataContractMetadataTypeAttribute"/> class.
        /// </summary>
        /// <param name="metadataClassType">The type alias name when serializing to a textual format.</param>
        /// <exception cref="ArgumentException"><paramref name="metadataClassType"/> is <c>null</c></exception>
        public DataContractMetadataTypeAttribute(Type metadataClassType)
        {
            MetadataClassType = metadataClassType;
        }

        /// <summary>
        ///   Gets the metadata class that is associated with a serializable class.
        /// </summary>
        public Type MetadataClassType { get; }
    }
}
