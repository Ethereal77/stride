// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Assets.Media;
using Stride.Audio;

namespace Stride.Editor.Preview
{
    [AssetCompiler(typeof(SoundAsset), typeof(EditorGameCompilationContext))]
    public class SoundAssetEditorGameCompiler : AssetCompilerBase
    {
        protected override void Prepare(AssetCompilerContext context, AssetItem assetItem, string targetUrlInStorage, AssetCompilerResult result)
        {
            result.BuildSteps.Add(new DummyAssetCommand<SoundAsset, Sound>(assetItem));
        }
    }
}
