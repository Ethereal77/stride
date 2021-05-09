// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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
