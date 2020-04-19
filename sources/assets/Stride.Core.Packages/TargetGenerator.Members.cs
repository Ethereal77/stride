// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

namespace Xenko.Core.Packages
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
