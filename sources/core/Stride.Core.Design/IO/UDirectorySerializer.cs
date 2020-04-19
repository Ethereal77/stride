// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Serialization;

namespace Xenko.Core.IO
{
    [DataSerializerGlobal(typeof(UDirectorySerializer))]
    internal class UDirectorySerializer : DataSerializer<UDirectory>
    {
        /// <inheritdoc/>
        public override void Serialize(ref UDirectory obj, ArchiveMode mode, SerializationStream stream)
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
                obj = new UDirectory(path);
            }
        }
    }
}
