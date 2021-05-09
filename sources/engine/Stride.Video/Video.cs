// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Diagnostics;

using Stride.Core;
using Stride.Core.IO;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;

namespace Stride.Video
{
    /// <summary>
    /// Video content.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    [ContentSerializer(typeof(DataContentSerializer<Video>))]
    [ReferenceSerializer, DataSerializerGlobal(typeof(ReferenceSerializer<Video>), Profile = "Content")]
    [DataSerializer(typeof(VideoSerializer))]

    public sealed class Video : ComponentBase
    {
        internal DatabaseFileProvider FileProvider;

        public string CompressedDataUrl { get; set; }
    }
}
