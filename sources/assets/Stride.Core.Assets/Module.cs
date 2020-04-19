// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Analysis;
using Xenko.Core.Assets.Templates;
using Xenko.Core.Assets.Tracking;
using Xenko.Core;
using Xenko.Core.Reflection;
using Xenko.Core.Yaml;

namespace Xenko.Core.Assets
{
    internal class Module
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            // Shadow object is always enabled when we are using assets, so we force it here
            ShadowObject.Enable = true;

            // Make sure that this assembly is registered
            AssemblyRegistry.Register(typeof(Module).Assembly, AssemblyCommonCategories.Assets);

            AssetYamlSerializer.Default.PrepareMembers += SourceHashesHelper.AddSourceHashesMember;
        }
    }
}
