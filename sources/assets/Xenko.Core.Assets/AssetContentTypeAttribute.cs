// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Serialization.Contents;

namespace Xenko.Core.Assets
{
    /// <summary>
    /// Describes which runtime-type, loadable through the <see cref="ContentManager"/>, corresponds to the associated asset type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AssetContentTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetContentTypeAttribute"/> class.
        /// </summary>
        /// <param name="contentType">The content type corresponding to the associated asset type.</param>
        public AssetContentTypeAttribute(Type contentType)
        {
            ContentType = contentType;
        }

        /// <summary>
        /// The content type corresponding to the associated asset type.
        /// </summary>
        public Type ContentType { get; }
    }
}
