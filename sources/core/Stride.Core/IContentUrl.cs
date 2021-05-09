// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core
{
    /// <summary>
    /// Interface for serializable object having an url (so referenceable by other assets and saved into a single blob file)
    /// </summary>
    public interface IContentUrl
    {
        /// <summary>
        /// The URL of this asset.
        /// </summary>
        string Url { get; set; }
    }
}
