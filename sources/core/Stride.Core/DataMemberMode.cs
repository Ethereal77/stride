// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core
{
    /// <summary>
    ///   Specify the way to store a property or field of some class or structure.
    /// </summary>
    public enum DataMemberMode
    {
        /// <summary>
        ///   Use the default mode depending on the type of the field / property.
        /// </summary>
        Default,

        /// <summary>
        ///   When restored, a new object is created by using the parameters in the YAML data and assigned to the field / property.
        ///   When the field / property is writeable, this is the default.
        /// </summary>
        Assign,

        /// <summary>
        ///  Only valid for a field / property that has a class or struct type.
        ///  When restored, instead of recreating the whole class or struct, the members are independently restored. When the
        ///  field / property is not writeable this is the default.
        /// </summary>
        Content,

        /// <summary>
        ///  Only valid for a a field / property that has an array type of some value type. The content of the array is stored in
        ///  a binary format encoded in base-64.
        /// </summary>
        Binary,

        /// <summary>
        /// The field / property will not be stored.
        /// </summary>
        Never
    }
}
