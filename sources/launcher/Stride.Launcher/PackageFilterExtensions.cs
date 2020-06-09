// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

using Stride.Core;
using Stride.Core.Packages;

namespace Stride.LauncherApp
{
    static class PackageFilterExtensions
    {
        public static IEnumerable<T> FilterStrideMainPackages<T>(this IEnumerable<T> packages) where T : NugetPackage
        {
            // Stride up to 3.0 package is Xenko, 3.x is Xenko.GameStudio, then Stride.GameStudio
            return packages.Where(package => (package.Id == "Xenko" && package.Version < new PackageVersion(3, 1, 0, 0)) ||
                                             (package.Id == "Xenko.GameStudio" && package.Version < new PackageVersion(4, 0, 0, 0)) ||
                                             (package.Id == "Stride.GameStudio"));
        }
    }
}
