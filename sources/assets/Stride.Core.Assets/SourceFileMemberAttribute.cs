// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Assets
{
    /// <summary>
    /// An attribute indicating whether a member of an asset represents the path to a source file for this asset.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class SourceFileMemberAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceFileMemberAttribute"/> class.
        /// </summary>
        /// <param name="updateAssetIfChanged">If true, the asset should be updated when the related source file changes.</param>
        public SourceFileMemberAttribute(bool updateAssetIfChanged)
        {
            UpdateAssetIfChanged = updateAssetIfChanged;
        }

        /// <summary>
        /// Gets whether the asset should be updated when the related source file changes.
        /// </summary>
        public bool UpdateAssetIfChanged { get; }

        /// <summary>
        /// Gets or sets whether this source file is optional for the compilation of the asset.
        /// </summary>
        public bool Optional { get; set; }
    }
}
