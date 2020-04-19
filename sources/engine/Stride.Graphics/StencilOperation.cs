// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Graphics
{
    /// <summary>
    /// TODO Comments
    /// </summary>
    [DataContract]
    public enum StencilOperation
    {
        /// <summary>
        /// 
        /// </summary>
        Keep = 1,
        /// <summary>
        /// 
        /// </summary>
        Zero = 2,
        /// <summary>
        /// 
        /// </summary>
        Replace = 3,
        /// <summary>
        /// 
        /// </summary>
        IncrementSaturation = 4,
        /// <summary>
        /// 
        /// </summary>
        DecrementSaturation = 5,
        /// <summary>
        /// 
        /// </summary>
        Invert = 6,
        /// <summary>
        /// 
        /// </summary>
        Increment = 7,
        /// <summary>
        /// 
        /// </summary>
        Decrement = 8,
    }
}
