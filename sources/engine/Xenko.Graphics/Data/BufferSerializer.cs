// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;
using Xenko.Core.Serialization;
using Xenko.Core.Serialization.Contents;

namespace Xenko.Graphics.Data
{
    /// <summary>
    /// Serializer for <see cref="Buffer"/>.
    /// </summary>
    public class BufferSerializer : DataSerializer<Buffer>
    {
        public override void PreSerialize(ref Buffer buffer, ArchiveMode mode, SerializationStream stream)
        {
            // Do not create object during preserialize (OK because not recursive)
        }

        public override void Serialize(ref Buffer buffer, ArchiveMode mode, SerializationStream stream)
        {
            if (mode == ArchiveMode.Deserialize)
            {
                var bufferData = stream.Read<BufferData>();

                var services = stream.Context.Tags.Get(ServiceRegistry.ServiceRegistryKey);
                if (services == null)
                {
                    buffer.SetSerializationData(bufferData);
                }
                else
                {
                    var graphicsDeviceService = services.GetSafeServiceAs<IGraphicsDeviceService>();

                    buffer.AttachToGraphicsDevice(graphicsDeviceService.GraphicsDevice);
                    buffer.InitializeFrom(bufferData.Content, bufferData.StructureByteStride, bufferData.BufferFlags, PixelFormat.None, bufferData.Usage);

                    // Setup reload callback (reload from asset manager)
                    var contentSerializerContext = stream.Context.Get(ContentSerializerContext.ContentSerializerContextProperty);
                    if (contentSerializerContext != null)
                    {
                        var assetManager = contentSerializerContext.ContentManager;
                        var url = contentSerializerContext.Url;

                        buffer.Reload = (graphicsResource) =>
                        {
                            // TODO: Avoid loading/unloading the same data
                            var loadedBufferData = assetManager.Load<BufferData>(url);
                            ((Buffer)graphicsResource).Recreate(loadedBufferData.Content);
                            assetManager.Unload(loadedBufferData);
                        };
                    }
                }
            }
            else
            {
                var bufferData = buffer.GetSerializationData();
                if (bufferData == null)
                    throw new InvalidOperationException("Trying to serialize a Buffer without CPU info.");

                stream.Write(bufferData);
            }
        }
    }
}
