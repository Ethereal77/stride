// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Serialization
{
    /// <summary>
    ///   Enumerates the different modes of serialization (either serialization or deserialization).
    /// </summary>
    public enum ArchiveMode
    {
        /// <summary>
        ///   The serializer is in serialize mode.
        /// </summary>
        Serialize,

        /// <summary>
        ///   The serializer is in deserialize mode.
        /// </summary>
        Deserialize
    }
}
