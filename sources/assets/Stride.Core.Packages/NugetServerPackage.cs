// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using NuGet.Protocol.Core.Types;

using Stride.Core.Annotations;

namespace Stride.Core.Packages
{
    public class NugetServerPackage : NugetPackage
    {
        public NugetServerPackage([NotNull] IPackageSearchMetadata package, [NotNull] string source) : base(package)
        {
            Source = source;
        }

        [NotNull]
        public string Source { get; }
    }
}
