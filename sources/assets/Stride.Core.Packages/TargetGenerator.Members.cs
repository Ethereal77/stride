// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Core.Packages
{
    partial class TargetGenerator
    {
        private readonly NugetStore store;
        private readonly List<NugetLocalPackage> packages;
        private readonly string packageVersionPrefix;

        internal TargetGenerator(NugetStore store, List<NugetLocalPackage> packages, string packageVersionPrefix)
        {
            this.store = store;
            this.packages = packages;
            this.packageVersionPrefix = packageVersionPrefix;
        }
    }
}
