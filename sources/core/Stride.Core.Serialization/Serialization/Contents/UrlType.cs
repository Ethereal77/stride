// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Serialization.Contents
{
    /// <summary>
    /// An enum representing the type of an url.
    /// </summary>
    [DataContract]
    public enum UrlType
    {
        /// <summary>
        /// The location is not valid.
        /// </summary>
        None,

        /// <summary>
        /// The location is a file on the disk.
        /// </summary>
        File,

        /// <summary>
        /// The location is a content url.
        /// </summary>
        Content,
    }
}
