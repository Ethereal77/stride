// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Reflection
{
    /// <summary>
    /// A factory to create an instance of a <see cref="ITypeDescriptor"/>
    /// </summary>
    public interface ITypeDescriptorFactory
    {
        /// <summary>
        /// Gets the attribute registry used by this factory.
        /// </summary>
        /// <value>The attribute registry.</value>
        IAttributeRegistry AttributeRegistry { get; }

        /// <summary>
        /// Tries to create an instance of a <see cref="ITypeDescriptor"/> from the type. Return null if this factory is not handling this type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>ITypeDescriptor.</returns>
        ITypeDescriptor Find(Type type);
    }
}
