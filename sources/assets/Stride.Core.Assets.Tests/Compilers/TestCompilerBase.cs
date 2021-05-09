// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Assets.Analysis;
using Stride.Core.Assets.Compiler;
using Stride.Core.Serialization.Contents;

namespace Stride.Core.Assets.Tests.Compilers
{
    public abstract class TestCompilerBase : IAssetCompiler
    {
        public static HashSet<AssetItem> CompiledAssets;

        public abstract AssetCompilerResult Prepare(AssetCompilerContext context, AssetItem assetItem);

        public virtual IEnumerable<Type> GetRuntimeTypes(AssetItem assetItem) { yield break; }

        public virtual IEnumerable<ObjectUrl> GetInputFiles(AssetItem assetItem) { yield break; }

        public virtual IEnumerable<BuildDependencyInfo> GetInputTypes(AssetItem assetItem) { yield break; }

        public virtual IEnumerable<Type> GetInputTypesToExclude(AssetItem assetItem) { yield break; }

        public virtual bool AlwaysCheckRuntimeTypes { get; } = true;
    }
}