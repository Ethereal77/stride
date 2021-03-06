// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Core.Assets
{
    [DataContract("ProjectSourceCodeWithFileGeneratorAsset")]
    public abstract class ProjectSourceCodeWithFileGeneratorAsset : ProjectSourceCodeAsset, IProjectFileGeneratorAsset
    {
        /// <inheritdoc/>
        [DataMember(Mask = DataMemberAttribute.IgnoreMask)]
        [Display(Browsable = false)]
        public abstract string Generator { get; }

        /// <param name="assetItem"></param>
        /// <inheritdoc/>
        public abstract void SaveGeneratedAsset(AssetItem assetItem);
    }
}
