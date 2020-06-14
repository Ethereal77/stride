// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.IO;

using Stride.Core;
using Stride.Core.Assets;
using Stride.Shaders.Parser.Mixins;

namespace Stride.Assets.Effect
{
    /// <summary>
    ///   Represents a shader effect asset (SDFX).
    /// </summary>
    [DataContract("EffectCompositorAsset")]
    [AssetDescription(FileExtension, AlwaysMarkAsRoot = true, AllowArchetype = false)]
    public sealed partial class EffectCompositorAsset : ProjectSourceCodeWithFileGeneratorAsset
    {
        /// <summary>
        ///   The default file extension used by the <see cref="EffectCompositorAsset"/>.
        /// </summary>
        public const string FileExtension = ".sdfx";

        public override string Generator => "StrideEffectCodeGenerator";

        public override void SaveGeneratedAsset(AssetItem assetItem)
        {
            var generatedFileData = ShaderKeyFileHelper.GenerateCode(assetItem.FullPath, Text);

            // Generate the .sdfx.cs files
            File.WriteAllBytes(assetItem.GetGeneratedAbsolutePath(), generatedFileData);
        }
    }
}
