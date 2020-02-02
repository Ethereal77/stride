// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Xenko.Core;
using Xenko.Core.Serialization;

namespace Xenko.Graphics.Font
{
    /// <summary>
    /// Serializer for <see cref="RuntimeRasterizedSpriteFont"/>.
    /// </summary>
    internal class OfflineRasterizedSpriteFontSerializer : DataSerializer<OfflineRasterizedSpriteFont>, IDataSerializerGenericInstantiation
    {
        private DataSerializer<SpriteFont> parentSerializer;

        public override void PreSerialize(ref OfflineRasterizedSpriteFont texture, ArchiveMode mode, SerializationStream stream)
        {
            // Do not create object during pre-serialize (OK because not recursive)
        }

        public override void Initialize(SerializerSelector serializerSelector)
        {
            parentSerializer = SerializerSelector.Default.GetSerializer<SpriteFont>();
            if (parentSerializer == null)
            {
                throw new InvalidOperationException(string.Format("Could not find parent serializer for type {0}", "Xenko.Graphics.SpriteFont"));
            }
        }

        public override void Serialize(ref OfflineRasterizedSpriteFont font, ArchiveMode mode, SerializationStream stream)
        {
            SpriteFont spriteFont = font;
            parentSerializer.Serialize(ref spriteFont, mode, stream);
            font = (OfflineRasterizedSpriteFont)spriteFont;

            if (mode == ArchiveMode.Deserialize)
            {
                var services = stream.Context.Tags.Get(ServiceRegistry.ServiceRegistryKey);
                var fontSystem = services.GetSafeServiceAs<FontSystem>();

                font.CharacterToGlyph = stream.Read<Dictionary<char, Glyph>>();
                font.StaticTextures = stream.Read<List<Texture>>();

                font.FontSystem = fontSystem;
            }
            else
            {
                stream.Write(font.CharacterToGlyph);
                stream.Write(font.StaticTextures);
            }
        }

        public void EnumerateGenericInstantiations(SerializerSelector serializerSelector, IList<Type> genericInstantiations)
        {
            genericInstantiations.Add(typeof(Dictionary<char, Glyph>));
            genericInstantiations.Add(typeof(List<Texture>));
        }
    }
}
