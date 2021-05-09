// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Compiler;

namespace Stride.Core.Assets
{
    /// <summary>
    /// Raw asset compiler.
    /// </summary>
    [AssetCompiler(typeof(RawAsset), typeof(AssetCompilationContext))]
    internal class RawAssetCompiler : AssetCompilerBase
    {
        protected override void Prepare(AssetCompilerContext context, AssetItem assetItem, string targetUrlInStorage, AssetCompilerResult result)
        {
            var asset = (RawAsset)assetItem.Asset;

            // Get absolute path of asset source on disk
            var assetSource = GetAbsolutePath(assetItem, asset.Source);
            var importCommand = new ImportStreamCommand(targetUrlInStorage, assetSource) { DisableCompression = !asset.Compress };

            result.BuildSteps = new AssetBuildStep(assetItem);
            result.BuildSteps.Add(importCommand);
        }
    }
}
