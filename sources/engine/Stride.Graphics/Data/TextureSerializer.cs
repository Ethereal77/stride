// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Serialization;

namespace Xenko.Graphics.Data
{
    /// <summary>
    /// Serializer for <see cref="Texture"/>.
    /// </summary>
    public class TextureSerializer : DataSerializer<Texture>
    {
        /// <inheritdoc/>
        public override void PreSerialize(ref Texture texture, ArchiveMode mode, SerializationStream stream)
        {
            // Do not create object during preserialize (OK because not recursive)
        }
        
        /// <inheritdoc/>
        public override void Serialize(ref Texture texture, ArchiveMode mode, SerializationStream stream)
        {
            TextureContentSerializer.Serialize(mode, stream, texture, false);
        }
    }
}
