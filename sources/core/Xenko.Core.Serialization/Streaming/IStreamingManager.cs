// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Streaming
{
    /// <summary>
    /// Interface for Streaming Manager service.
    /// </summary>
    public interface IStreamingManager
    {
        /// <summary>
        /// Puts request to load given resource up to the maximum residency level.
        /// </summary>
        /// <param name="obj">The streamable resource object.</param>
        void FullyLoadResource(object obj);
    }
}
