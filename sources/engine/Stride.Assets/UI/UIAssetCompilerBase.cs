// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Core.BuildEngine;
using Stride.Core;
using Stride.Core.Serialization.Contents;
using Stride.UI;

namespace Stride.Assets.UI
{
    public abstract class UIAssetCompilerBase<T> : AssetCompilerBase
        where T : UIAssetBase
    {
        public override IEnumerable<Type> GetRuntimeTypes(AssetItem assetItem)
        {
            yield return typeof(UIElement);
        }

        protected sealed override void Prepare(AssetCompilerContext context, AssetItem assetItem, string targetUrlInStorage, AssetCompilerResult result)
        {
            var asset = (T)assetItem.Asset;
            result.BuildSteps = new AssetBuildStep(assetItem);
            result.BuildSteps.Add(Create(targetUrlInStorage, asset, assetItem.Package));
        }

        protected abstract UIConvertCommand Create(string url, T parameters, Package package);

        protected abstract class UIConvertCommand : AssetCommand<T>
        {
            protected UIConvertCommand(string url, T parameters, IAssetFinder assetFinder)
                : base(url, parameters, assetFinder)
            {
            }

            protected sealed override Task<ResultStatus> DoCommandOverride(ICommandContext commandContext)
            {
                var assetManager = new ContentManager(MicrothreadLocalDatabases.ProviderService);

                var uiObject = Create(commandContext);
                assetManager.Save(Url, uiObject);

                return Task.FromResult(ResultStatus.Successful);
            }

            protected abstract ComponentBase Create(ICommandContext commandContext);
        }
    }
}
