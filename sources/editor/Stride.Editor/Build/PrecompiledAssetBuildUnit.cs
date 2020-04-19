// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets;
using Xenko.Core.Assets.Compiler;
using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.BuildEngine;

namespace Xenko.Editor.Build
{
    public class PrecompiledAssetBuildUnit : AssetBuildUnit
    {
        private readonly ListBuildStep buildStep;

        private readonly bool mergeInCommonDatabase;

        public PrecompiledAssetBuildUnit(AssetBuildUnitIdentifier identifier, ListBuildStep buildStep, bool mergeInCommonDatabase = false)
            : base(identifier)
        {
            this.buildStep = buildStep;
            this.mergeInCommonDatabase = mergeInCommonDatabase;
        }

        protected override ListBuildStep Prepare()
        {
            return buildStep;
        }

        protected override void PostBuild()
        {
            base.PostBuild();
            if (mergeInCommonDatabase)
            {
                MicrothreadLocalDatabases.AddToSharedGroup(buildStep.OutputObjects);
            }
        }
    }
}
