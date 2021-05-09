// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Reflection
{
    /// <summary>
    /// A category used by <see cref="ITypeDescriptorBase"/>.
    /// </summary>
    public enum DescriptorCategory
    {
        /// <summary>
        /// A primitive.
        /// </summary>
        Primitive,

        /// <summary>
        /// A collection.
        /// </summary>
        Collection,

        /// <summary>
        /// An array
        /// </summary>
        Array,

        /// <summary>
        /// A dictionary
        /// </summary>
        Dictionary,

        /// <summary>
        /// An object
        /// </summary>
        Object,

        /// <summary>
        /// An unsupported object. This will be treated the same as Object.
        /// </summary>
        NotSupportedObject,

        /// <summary>
        /// A nullable value
        /// </summary>
        Nullable,

        /// <summary>
        /// A custom descriptor.
        /// </summary>
        Custom
    }
}
