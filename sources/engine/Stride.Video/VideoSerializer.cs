// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.IO;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;

namespace Stride.Video
{
    /// <summary>
    ///   Used internally to serialize a <see cref="Video"/>.
    /// </summary>
    internal sealed class VideoSerializer : DataSerializer<Video>
    {
        public override void Serialize(ref Video video, ArchiveMode mode, SerializationStream stream)
        {
            if (mode == ArchiveMode.Deserialize)
            {
                var services = stream.Context.Tags.Get(ServiceRegistry.ServiceRegistryKey);

                video.FileProvider = services.GetService<IDatabaseFileProviderService>()?.FileProvider;
                video.CompressedDataUrl = stream.ReadString();
            }
            else
            {
                stream.Write(video.CompressedDataUrl);
            }
        }
    }
}
