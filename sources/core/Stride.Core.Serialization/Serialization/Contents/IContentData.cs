// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Serialization.Contents
{
    /// <summary>
    /// A content data storing its own Location.
    /// </summary>
    public interface IContentData
    {
        string Url { get; set; }
    }
}
