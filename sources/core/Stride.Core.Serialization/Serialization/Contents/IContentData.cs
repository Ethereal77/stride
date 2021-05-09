// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Serialization.Contents
{
    /// <summary>
    /// A content data storing its own Location.
    /// </summary>
    public interface IContentData
    {
        string Url { get; set; }
    }
}
