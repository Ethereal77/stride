// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Serialization;

namespace Stride.Graphics.Data
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
