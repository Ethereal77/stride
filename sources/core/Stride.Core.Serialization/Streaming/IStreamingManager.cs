// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Streaming
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
