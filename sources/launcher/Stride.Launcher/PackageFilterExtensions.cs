// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Packages;

namespace Stride.LauncherApp
{
    static class PackageFilterExtensions
    {
        public static IEnumerable<T> FilterStrideMainPackages<T>(this IEnumerable<T> packages) where T : NugetPackage
        {
            // Stride up to 3.0 package is Stride, after it's Stride.GameStudio
            return packages.Where(x => x.Id != "Stride" || x.Version < new PackageVersion(3, 1, 0, 0));
        }
    }
}
