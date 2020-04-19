// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ComponentModel;

using Xenko.Core.Assets;
using Xenko.Core;
using Xenko.Animations;
using Xenko.Assets;

namespace Xenko.SpriteStudio.Offline
{
    [DataContract("SpriteStudioAnimationAsset")] // Name of the Asset serialized in YAML
    [AssetContentType(typeof(AnimationClip))]
    [AssetDescription(FileExtension)] // A description used to display in the asset editor
    [Display("SpriteStudio animation")]
    [AssetFormatVersion(XenkoConfig.PackageName, CurrentVersion, "2.0.0.0")]
    public class SpriteStudioAnimationAsset : AssetWithSource
    {
        public const string FileExtension = ".xkss4a";

        private const string CurrentVersion = "2.0.0.0";

        [DataMember(1)]
        [DefaultValue(AnimationRepeatMode.LoopInfinite)]
        public AnimationRepeatMode RepeatMode { get; set; } = AnimationRepeatMode.LoopInfinite;

        [DataMember(2)]
        [Display(Browsable = false)]
        [DefaultValue("")]
        public string AnimationName { get; set; } = "";
    }
}
