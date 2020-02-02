// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Serialization;
using Xenko.Core.Serialization.Contents;

namespace Xenko.Core.Assets.Analysis
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
