// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;

namespace Stride.Graphics
{
    internal class ImageSerializer : ContentSerializerBase<Image>
    {
        public override void Serialize(ContentSerializerContext context, SerializationStream stream, Image textureData)
        {
            if (context.Mode == ArchiveMode.Deserialize)
            {
                var image = Image.Load(stream.NativeStream);
                textureData.InitializeFrom(image);
            }
            else
            {
                textureData.Save(stream.NativeStream, ImageFileType.Stride);
            }
        }

        public override object Construct(ContentSerializerContext context)
        {
            return new Image();
        }
    }
}
