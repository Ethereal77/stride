// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Reflection
{
    /// <summary>
    /// Describes what is targeted by an override information.
    /// </summary>
    public enum OverrideTarget
    {
        /// <summary>
        /// The content itself.
        /// </summary>
        Content,
        /// <summary>
        /// An item of the content if it's a collection, or a value of the content if it's a dictionary.
        /// </summary>
        Item,
        /// <summary>
        /// A key of the content. This is valid only for dictionary.
        /// </summary>
        Key
    }
}
