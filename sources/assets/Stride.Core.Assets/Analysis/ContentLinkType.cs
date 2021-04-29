// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Core.Assets.Analysis
{
    /// <summary>
    ///   Defines flags to mark the different types of links between content elements.
    /// </summary>
    [Flags]
    public enum ContentLinkType
    {
        /// <summary>
        ///   A simple reference to an <see cref="Asset"/>.
        /// </summary>
        Reference = 1,

        /// <summary>
        ///   Represents every possible type of link.
        /// </summary>
        All = Reference
    }
}
