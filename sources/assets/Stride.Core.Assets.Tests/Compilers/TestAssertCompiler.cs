// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Stride.Core.Assets.Compiler;
using Stride.Core.BuildEngine;

namespace Stride.Core.Assets.Tests.Compilers
{
    public class TestAssertCompiler<T> : TestCompilerBase where T : Asset
    {
        private class AssertCommand : AssetCommand<T>
        {
            private readonly AssetItem assetItem;
            private readonly Action<string, T, IAssetFinder> assertFunc;

            public AssertCommand(AssetItem assetItem, T parameters, IAssetFinder assetFinder, Action<string, T, IAssetFinder> assertFunc)
                : base(assetItem.Location, parameters, assetFinder)
            {
                this.assetItem = assetItem;
                this.assertFunc = assertFunc;
            }

            protected override Task<ResultStatus> DoCommandOverride(ICommandContext commandContext)
            {
                assertFunc?.Invoke(Url, Parameters, AssetFinder);
                CompiledAssets.Add(assetItem);
                return Task.FromResult(ResultStatus.Successful);
            }
        }

        public override AssetCompilerResult Prepare(AssetCompilerContext context, AssetItem assetItem)
        {
            var result = new AssetCompilerResult(GetType().Name)
            {
                BuildSteps = new AssetBuildStep(assetItem)
            };
            result.BuildSteps.Add(new AssertCommand(assetItem, (T)assetItem.Asset, assetItem.Package, DoCommandAssert));
            return result;
        }

        protected virtual void DoCommandAssert(string url, T parameters, IAssetFinder package)
        {

        }
    }
}