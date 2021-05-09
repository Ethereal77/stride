// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.IO;

using Stride.Core.Serialization;
using Stride.Core.Storage;

namespace Stride.Navigation
{
    public static class NavigationSettingsExtensions
    {
        /// <summary>
        /// Computes the hash of the <see cref="NavigationSettings.Groups"/> field
        /// </summary>
        public static ObjectId ComputeGroupsHash(this NavigationSettings settings)
        {
            using (DigestStream stream = new DigestStream(Stream.Null))
            {
                BinarySerializationWriter writer = new BinarySerializationWriter(stream);
                writer.Write(settings.Groups);
                return stream.CurrentHash;
            }
        }
    }
}
