// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Assets;
using Stride.Core.Assets.Analysis;
using Stride.Core.Assets.Compiler;
using Stride.Assets.Materials;
using Stride.Assets.Models;

namespace Stride.Editor.Preview
{
    [AssetCompiler(typeof(AnimationAsset), typeof(PreviewCompilationContext))]
    public class AnimationAssetPreviewCompiler : AnimationAssetCompiler
    {
        public override IEnumerable<BuildDependencyInfo> GetInputTypes(AssetItem assetItem)
        {
            foreach (var inputFile in base.GetInputTypes(assetItem))
            {
                yield return inputFile;
            }

            yield return new BuildDependencyInfo(typeof(ModelAsset), typeof(AssetCompilationContext), BuildDependencyType.Runtime | BuildDependencyType.CompileContent);
            yield return new BuildDependencyInfo(typeof(MaterialAsset), typeof(AssetCompilationContext), BuildDependencyType.Runtime | BuildDependencyType.CompileContent);
        }
    }
}
