// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Runtime.CompilerServices;

using Xenko.Core.Reflection;

namespace Xenko.Core.Assets.Quantum.Tests
{
    // Somehow it helps Resharper NUnit to run module initializer first (to determine unit test configuration).
    public class Module
    {
        [ModuleInitializer]
        internal static void Initialize()
        {
            AssemblyRegistry.Register(typeof(Module).Assembly, AssemblyCommonCategories.Assets);
            AssetQuantumRegistry.RegisterAssembly(typeof(Module).Assembly);
            RuntimeHelpers.RunModuleConstructor(typeof(Asset).Module.ModuleHandle);
        }
    }
}
