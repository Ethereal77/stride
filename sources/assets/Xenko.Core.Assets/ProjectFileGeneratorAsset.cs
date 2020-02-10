// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Core.Assets
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
