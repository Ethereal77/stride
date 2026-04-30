// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System.Reflection;
using Stride.Core;
using Stride.Core.Assets;

namespace Stride.Samples.Tests
{
    internal class TestServerResolver
    {
        [ModuleInitializer(-100)]
        public static void ResolveReferences() =>
            NuGetAssemblyResolver.SetupNuGet("Stride.SamplesTestServer", StrideVersion.NuGetVersion, Assembly.GetCallingAssembly());
    }
}
