// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Runtime.CompilerServices;

using Stride.Core.Reflection;

namespace Stride.Core.Assets.Quantum.Tests
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
