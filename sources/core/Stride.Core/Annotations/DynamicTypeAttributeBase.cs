// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Annotations
{
    /// <summary>
    /// Base class for a dynamic type attribute.
    /// </summary>
    public abstract class DynamicTypeAttributeBase : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicTypeAttributeBase"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        protected DynamicTypeAttributeBase([NotNull] Type type)
        {
            TypeName = type.AssemblyQualifiedName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicTypeAttributeBase"/> class.
        /// </summary>
        /// <param name="typeName">The type.</param>
        protected DynamicTypeAttributeBase(string typeName)
        {
            TypeName = typeName;
        }

        /// <summary>
        /// Gets the name of the <see cref="DynamicTypeAttributeBase"/> type
        /// </summary>
        /// <value>The name of the serializable type.</value>
        public string TypeName { get; }
    }
}
