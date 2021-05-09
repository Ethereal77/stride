// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Serialization;

namespace Stride.Core.IO
{
    /// <summary>
    /// Data serializer for Guid.
    /// </summary>
    [DataSerializerGlobal(typeof(UFileSerializer))]
    internal class UFileSerializer : DataSerializer<UFile>
    {
        /// <inheritdoc/>
        public override void Serialize(ref UFile obj, ArchiveMode mode, SerializationStream stream)
        {
            if (mode == ArchiveMode.Serialize)
            {
                var path = obj?.FullPath;
                stream.Serialize(ref path);
            }
            else if (mode == ArchiveMode.Deserialize)
            {
                string path = null;
                stream.Serialize(ref path);
                obj = new UFile(path);
            }
        }
    }
}
