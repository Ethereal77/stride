// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Streaming;

namespace Stride.Graphics.Data
{
    /// <summary>
    /// Used internally to find the currently active textures streaming service
    /// </summary>
    public interface ITexturesStreamingProvider
    {
        /// <summary>
        /// Loads the texture in a streaming service.
        /// </summary>
        /// <param name="obj">The texture object.</param>
        /// <param name="imageDescription">The image description.</param>
        /// <param name="storageHeader">The storage header.</param>
        void FullyLoadTexture(Texture obj, ref ImageDescription imageDescription, ref ContentStorageHeader storageHeader);

        /// <summary>
        /// Registers the texture in a streaming service.
        /// </summary>
        /// <param name="obj">The texture object.</param>
        /// <param name="imageDescription">The image description.</param>
        /// <param name="storageHeader">The storage header.</param>
        void RegisterTexture(Texture obj, ref ImageDescription imageDescription, ref ContentStorageHeader storageHeader);

        /// <summary>
        /// Unregisters the texture.
        /// </summary>
        /// <param name="obj">The texture object.</param>
        void UnregisterTexture(Texture obj);
    }
}
