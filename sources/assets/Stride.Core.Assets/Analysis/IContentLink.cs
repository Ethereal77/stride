// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;

namespace Stride.Core.Assets.Analysis
{
    /// <summary>
    /// The interface for types representing a link between elements.
    /// </summary>
    public interface IContentLink
    {
        /// <summary>
        /// The reference to the element at the opposite side of the link.
        /// </summary>
        IReference Element { get; }

        /// <summary>
        /// The type of the link.
        /// </summary>
        ContentLinkType Type { get; }
    }
}
